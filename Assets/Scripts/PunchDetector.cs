using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PunchDetector : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose;
    public float punchVelocityThreshold = 1.5f;

    public bool IsPunching()
    {
        return pose.GetVelocity().magnitude > punchVelocityThreshold;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPunching()) return;

        var punchable = other.GetComponent<Punchable>();

        if (punchable != null)
        {
            float force = pose.GetVelocity().magnitude;
            DealDamage(punchable, force);
            TriggerHaptics();
        }
    }

    void DealDamage(Punchable punchable, float force)
    {
        //enemy.GetComponent<EnemyHealth>()?.TakeDamage(force * 10f);
        Vector3 punchDir = pose.GetVelocity().normalized;
        punchable.Punch(punchDir * force);
    }

    void TriggerHaptics()
    {
        SteamVR_Actions.default_Haptic.Execute(
            0,
            0.1f,
            150,
            0.75f,
            pose.inputSource
        );
    }
}