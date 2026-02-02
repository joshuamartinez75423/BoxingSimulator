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
        float force = pose.GetVelocity().magnitude;
        DealDamage(punchable, force);
        TriggerHaptics();
    }
}

    /// <summary>
    /// Applies punch force and damage logic to a Punchable object.
    /// </summary>
    /// <param name="punchable">
    /// The object being punched.
    /// </param>
    /// <param name="force">
    /// The magnitude of the punch based on controller velocity.
    /// </param>
    void DealDamage(Punchable punchable, float force)
    {
        // Direction of the punch based on controller movement
        Vector3 punchDir = pose.GetVelocity().normalized;

        // Apply force to the Punchable object
        punchable.Punch(punchDir * force);
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