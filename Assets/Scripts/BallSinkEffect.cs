using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallSinkEffect : MonoBehaviour
{
    public float sinkDuration = 0.45f;
    public float sinkDepth = 0.18f;     // how far down it goes
    public float shrinkTo = 0.2f;       // final scale multiplier

    Rigidbody rb;
    Collider col;
    Vector3 originalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        originalScale = transform.localScale;
    }

    public IEnumerator SinkInto(Transform cupCenter)
    {
        // Stop physics & prevent extra collisions
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        if (col) col.enabled = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = cupCenter.position + Vector3.down * sinkDepth;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = originalScale * shrinkTo;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, sinkDuration);
            float eased = 1f - Mathf.Pow(1f - t, 3f); // ease-out cubic

            transform.position = Vector3.Lerp(startPos, endPos, eased);
            transform.localScale = Vector3.Lerp(startScale, endScale, eased);

            yield return null;
        }

        // Hide the ball (you can also Destroy(gameObject) if you prefer)
        gameObject.SetActive(false);
    }

    // Call this when you reset/spawn the ball again
    public void RestoreBall()
    {
        gameObject.SetActive(true);
        transform.localScale = originalScale;
        rb.isKinematic = false;
        if (col) col.enabled = true;
    }
}

