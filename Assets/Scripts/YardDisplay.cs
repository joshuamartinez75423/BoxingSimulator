using UnityEngine;
using TMPro;

public class YardDisplay : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI text;  // drag the TMP text here

    private void OnEnable()
    {
        YardTrackerUntilStop.AnyStoppedYards += HandleAnyStopped;
    }

    private void OnDisable()
    {
        YardTrackerUntilStop.AnyStoppedYards -= HandleAnyStopped;
    }

    private void Start()
    {
        if (text != null)
            text.text = "Punch to start!";
    }

    private void HandleAnyStopped(string objectName, float yards)
    {
        Debug.Log($"YardDisplay got yards from {objectName}: {yards}");

        if (text != null)
            text.text = $"{objectName}: {yards:F1} yd";
    }
}
