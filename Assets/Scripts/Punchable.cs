using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punchable : MonoBehaviour
{
    //Multiplies the amount of force applied to the punchable object
    public float punchForceMultiplier = 1;

    //The rigidbody attached to the punchable object
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    /// <summary>
    /// What happens when the punchable object is punched.
    /// </summary>
    /// <param name="force">
    /// The magnitude of the punch based on controller velocity.
    /// </param>
    public void Punch(Vector3 force)
    {
        rb.isKinematic = false;
        rb.AddForce(force * punchForceMultiplier, ForceMode.Impulse);
    }
}