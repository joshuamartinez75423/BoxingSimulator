using UnityEngine;

public class HoleWinTrigger : MonoBehaviour
{
    [Header("Setup")]
    public string ballTag = "Ball";
    public ScorePopupManager popup;

    private bool holeCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        // Prevent multiple triggers if ball jiggles in hole
        if (holeCompleted) return;

        if (!other.CompareTag(ballTag)) return;

        holeCompleted = true;

        Debug.Log("Ball entered the hole!");

        // Try get Punchable from the ball
        Punchable punchable = other.GetComponent<Punchable>();

        int punches = 0;

        if (punchable != null)
        {
            punches = punchable.PunchCount;

            // Stop the ball immediately
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Disable further punching
            punchable.enabled = false;
        }
        else
        {
            Debug.LogWarning("Ball entered hole but has NO Punchable script!");
        }

        Debug.Log("Hole finished with punches = " + punches);

        if (popup != null)
        {
            popup.ShowWin(punches);
        }
        else
        {
            Debug.LogError("ScorePopupManager is NOT assigned on HoleWinTrigger!");
        }
    }
}
