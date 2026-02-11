using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class BallTrailController : MonoBehaviour
{
    public float showSpeed = 1.0f;     // speed threshold to show trail
    public float stopSpeed = 0.3f;     // speed threshold to hide trail
    public float stopDelay = 0.4f;     // wait before hiding after slowing

    private TrailRenderer tr;
    private Rigidbody rb;
    private Coroutine stopRoutine;

    void Awake()
    {
        tr = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        tr.emitting = false;
        tr.Clear();
    }

    void Update()
    {
        if (rb == null) return;

        float speed = rb.velocity.magnitude;

        if (speed >= showSpeed)
        {
            if (stopRoutine != null) { StopCoroutine(stopRoutine); stopRoutine = null; }
            if (!tr.emitting)
            {
                tr.Clear();          // prevents a line from “starting” at old position
                tr.emitting = true;
            }
        }
        else if (tr.emitting && speed <= stopSpeed)
        {
            if (stopRoutine == null)
                stopRoutine = StartCoroutine(StopAfterDelay());
        }
    }

    IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(stopDelay);
        tr.emitting = false;
        stopRoutine = null;
    }
}

