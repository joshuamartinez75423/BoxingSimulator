using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

[RequireComponent(typeof(SteamVR_Behaviour_Pose))]
public class VRLaserClick : MonoBehaviour
{
    [Header("Laser Visual")]
    public float maxDistance = 10f;
    public float thickness = 0.01f;

    [Header("Click Input")]
    public SteamVR_Action_Boolean clickAction; // assign in inspector

    [Header("Enable")]
    public bool enabledForMenu = true;

    private SteamVR_Behaviour_Pose pose;
    private GameObject laserObj;
    private Transform laserTf;

    void Awake()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();

        // Create laser beam
        laserObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        laserObj.name = "VR_Laser";
        Destroy(laserObj.GetComponent<Collider>()); // visual only

        laserTf = laserObj.transform;
        laserTf.SetParent(transform, false);
        laserTf.localRotation = Quaternion.identity;
    }

    void Update()
    {
        laserObj.SetActive(enabledForMenu);
        if (!enabledForMenu) return;

        if (clickAction == null) return;

        Ray ray = new Ray(transform.position, transform.forward);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, maxDistance);

        float dist = hitSomething ? hit.distance : maxDistance;

        // Laser transform
        laserTf.localScale = new Vector3(thickness, thickness, dist);
        laserTf.localPosition = new Vector3(0f, 0f, dist / 2f);

        // Click
        if (clickAction.GetStateDown(pose.inputSource) && hitSomething)
        {
            // Find a Unity UI Button on the hit object or its parents
            Button btn = hit.transform.GetComponentInParent<Button>();
            if (btn != null && btn.interactable)
            {
                btn.onClick.Invoke();
            }
        }
    }
}
