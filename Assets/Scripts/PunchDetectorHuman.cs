using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PunchDetectorHuman : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose;
    public float punchThreshold = 1.5f;

    public bool IsPunching()
    {
        return pose.GetVelocity().magnitude > punchThreshold;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPunching()) return;

        var punchable = other.GetComponent<Punchable>();
    }
}
