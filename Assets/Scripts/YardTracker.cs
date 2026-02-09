using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class YardTrackerUntilStop : MonoBehaviour
{
    public bool horizontalOnly = true;

    [Header("Stop Detection")]
    public float stopSpeed = 0.20f;
    public float stopTime = 0.75f;
    public bool restartOnNewPunch = true;

    public float DistanceMeters { get; private set; }
    public float DistanceYards => DistanceMeters * 1.0936133f;

    // Per-object event (still useful if you ever want it)
    public event Action<float> OnStoppedYards;

    // GLOBAL event: YardDisplay can listen once and get results from ANY object
    public static event Action<string, float> AnyStoppedYards;

    private Rigidbody rb;
    private Vector3 startPos;
    private bool tracking;
    private float underSpeedTimer;
    private Punchable punchable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        punchable = GetComponent<Punchable>();
    }

    public void BeginTracking()
    {
        if (tracking && !restartOnNewPunch) return;

        startPos = transform.position;
        tracking = true;
        underSpeedTimer = 0f;
        DistanceMeters = 0f;
    }

    void Update()
    {
        if (!tracking) return;

        float speed = rb.velocity.magnitude;

        if (speed < stopSpeed) underSpeedTimer += Time.deltaTime;
        else underSpeedTimer = 0f;

        if (underSpeedTimer >= stopTime)
        {
            tracking = false;

            Vector3 delta = transform.position - startPos;
            if (horizontalOnly) delta.y = 0f;

            DistanceMeters = delta.magnitude;

            // Fire BOTH events
            AnyStoppedYards?.Invoke(gameObject.name, DistanceYards);
            OnStoppedYards?.Invoke(DistanceYards);
            
            //Reset the punchable object once it's stopped.
            punchable.ResetPunchable();
        }
    }
}
