using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGrass : MonoBehaviour
{
    public PhysicMaterial fairway;
    public PhysicMaterial rough;
    private Rigidbody rb;
    private bool grounded;
    private float drag;
    private float angularDrag;
    private float fairwayTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        drag = rb.drag;
        angularDrag = rb.angularDrag;
        fairwayTimer = 0;
    }
    private void OnCollisionStay(Collision collision)
    {
        PhysicMaterial mat = collision.collider.sharedMaterial;
        grounded = true;
        Debug.Log(mat);

        if (mat == fairway)
        {
            rb.drag = 0.35f;
            rb.angularDrag = 0.35f;
            fairwayTimer += Time.deltaTime;
            if (fairwayTimer > 4)
            {
                rb.velocity *= 0.98f;
            }
        } else if (mat == rough)
        {
            rb.drag = 5f;
            rb.angularDrag = 5f;
        }
    }

    private void FixedUpdate()
    {
        if (!grounded)
        {
            rb.drag = drag;
            rb.angularDrag = angularDrag;
            fairwayTimer = 0;
        }

        grounded = false;
    }
}
