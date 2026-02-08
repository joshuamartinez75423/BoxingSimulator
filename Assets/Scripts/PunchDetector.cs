using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/// <summary>
/// Detects punch gestures using SteamVR controller velocity
/// and applies force, damage, and haptic feedback when a punch
/// hits a Punchable object.
/// </summary>
public class PunchDetector : MonoBehaviour
{
    // Reference to the SteamVR pose component for this hand. Provides position, rotation, velocity, and angular velocity.
    public SteamVR_Behaviour_Pose pose;

    // Minimum controller velocity magnitude required for the motion to count as a punch.
    public float punchVelocityThreshold = 1.5f;

    //multiplier for the punch force
    public float punchMult = 1f;

    /// <summary>
    /// Determines whether the controller is currently moving
    /// fast enough to be considered a punch.
    /// </summary>
    /// <returns>
    /// True if the controller velocity exceeds the punch threshold.
    /// </returns>
    public bool IsPunching()
    {
        return pose.GetVelocity().magnitude > punchVelocityThreshold;
    }



private void OnTriggerEnter(Collider other)
{
    if (!IsPunching()) return;

    // IMPORTANT: works even if the collider you hit is on a child
    var punchable = other.GetComponentInParent<Punchable>();

    Debug.Log($"Hit: {other.name} | punchable found: {(punchable != null ? punchable.name : "NO")}");

    if (punchable != null)
    {
        Vector3 velocity;
        Vector3 angularVelocity;

        pose.GetEstimatedPeakVelocities(out velocity, out angularVelocity);

        float force = velocity.magnitude;
        Vector3 punchDir = (other.transform.position - pose.transform.position).normalized;

        punchable.Punch(punchDir * force * punchMult);
        TriggerHaptics();
    }
    }

    /// <summary>
    /// Triggers haptic feedback on the controller
    /// to give physical punch confirmation.
    /// </summary>
    void TriggerHaptics()
    {
        SteamVR_Actions.default_Haptic.Execute(
            0,              // delay before vibration starts
            0.1f,           // duration of vibration
            150,            // frequency
            0.75f,          // amplitude (strength)
            pose.inputSource // which controller (left/right)
        );
    }
}