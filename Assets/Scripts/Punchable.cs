using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Punchable : MonoBehaviour
{
    //Multiplies the amount of force applied to the punchable object
    public float punchForceMultiplier = 1;
    public TextMeshProUGUI punchCountText;
    public TextMeshProUGUI punchTimeText;
    public float extraFriction;
    public float slowSpeed = 1;

    private bool punchable = true;
    private int punchCounter = 0;
    private float punchTimer = 0;

    //The rigidbody attached to the punchable object
    Rigidbody rb;

    void Awake()
    {
        punchCounter = 0;
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        Debug.LogError($"{name} has Punchable but NO Rigidbody on the same GameObject!");

        rb.isKinematic = true;
        punchTimer = 0;
    }

    private void Update()
    {
        if (!punchable)
        {
            punchTimer += Time.deltaTime;
            if (punchTimeText)
            {
                punchTimeText.text = "Punch Timer:" + punchTimer.ToString();
            }
            
            if (punchTimer > 6 && rb.velocity.magnitude < slowSpeed)
            {
                rb.velocity *= extraFriction;
            }
        }
    }

    /// <summary>
    /// What happens when the punchable object is punched.
    /// </summary>
    /// <param name="force">
    /// The magnitude of the punch based on controller velocity.
    /// </param>
    public void Punch(Vector3 force)
    {
        if (punchable)
        {
            var tracker = GetComponent<YardTrackerUntilStop>();
            Debug.Log($"{name} Punch() called. Tracker found: {(tracker != null)}");

            if (tracker != null) tracker.BeginTracking();

            rb.isKinematic = false;
            rb.AddForce(force * punchForceMultiplier, ForceMode.Impulse);
            punchable = false;
            UpdateScore(1);
        }
    }

    //The punchable object becomes kinematic once again, and is moved up to a position where it can be punched again.
    public void ResetPunchable()
    {
        rb.isKinematic = true;

        //bring the ball up to where it is reachable.
        MoveUp(1.7f);

        punchable = true;

    }

    void UpdateScore(int value)
    {
        punchCounter++;
        punchCountText.text = "Punches: " + punchCounter.ToString() + "\nPunch Timer:" + punchTimer.ToString();
    }  
    
    void ResetScore()
    {
        punchCounter = 0;
        punchCountText.text = "Punches: " + punchCounter.ToString();
    }

    //starts a coroutine to move the ball up
    public void MoveUp(float moveAmount)
    {
        StartCoroutine(LerpUp(moveAmount));
    }

    //Smoothly moves the ball up over the course of a given time
    IEnumerator LerpUp (float moveAmount)
    {
        Debug.Log("LerpUp Called");
        Vector3 start = transform.position;
        Vector3 target = start + Vector3.up * moveAmount;

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target; // snap perfectly at end
    }
}