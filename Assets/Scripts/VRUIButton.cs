using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;

public class VRUIButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
            Debug.LogError($"VRUIButton must be on the same GameObject as a Unity UI Button. ({name})");
    }

    private void OnEnable()
    {
        foreach (var pointer in FindObjectsOfType<SteamVR_LaserPointer>())
            pointer.PointerClick += HandlePointerClick;
    }

    private void OnDisable()
    {
        foreach (var pointer in FindObjectsOfType<SteamVR_LaserPointer>())
            pointer.PointerClick -= HandlePointerClick;
    }

    private void HandlePointerClick(object sender, PointerEventArgs e)
    {
        if (e.target != null && e.target.GetComponentInParent<Button>() == button)
            return;

        // Raycast often hits a child. If that child is under THIS button, click it.
        var hitButton = e.target.GetComponentInParent<Button>();
        if (hitButton == button && button.interactable)
        {
            Debug.Log($"VRUIButton click: {button.name}");
            button.onClick.Invoke();
        }
    }
}
