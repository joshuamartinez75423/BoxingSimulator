using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleAttractor : MonoBehaviour
{
    public Transform cupCenter;
    public string ballTag = "Ball";

    [Header("Attraction")]
    public float maxPullForce = 12f;   // strength near hole
    public float damping = 2.5f;       // reduces orbiting
    public float maxSpeed = 6f;        // optional clamp

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(ballTag)) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 toCup = cupCenter.position - rb.position;
        float dist = toCup.magnitude;
        if (dist < 0.001f) return;

        float strength = Mathf.Clamp01(1f / dist); // ramps up closer
        Vector3 pull = toCup.normalized * (maxPullForce * strength);
        Vector3 damp = -rb.velocity * damping;

        rb.AddForce(pull + damp, ForceMode.Acceleration);

        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }
}

