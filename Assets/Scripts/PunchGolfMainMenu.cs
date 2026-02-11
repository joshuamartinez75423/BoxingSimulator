using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchGolfMainMenu : MonoBehaviour
{
    [Header("Menu UI")]
    public GameObject menuRoot;   // assign MenuPanel (or Canvas)

    [Header("Optional spawn teleport")]
    public Transform spawnPoint;  // empty object in scene
    public Transform rigRoot;     // SteamVR rig root
    public Transform head;        // HMD camera transform

    public void Play()
    {
        if (menuRoot != null)
            menuRoot.SetActive(false);

        // Optional teleport on Play
        if (spawnPoint != null && rigRoot != null && head != null)
        {
            Vector3 headOffset = head.position - rigRoot.position;
            Vector3 newRigPos = spawnPoint.position - headOffset;
            newRigPos.y = rigRoot.position.y; // keep height stable
            rigRoot.position = newRigPos;

            rigRoot.rotation = Quaternion.Euler(0f, spawnPoint.eulerAngles.y, 0f);
        }

        Debug.Log("PunchGolf: Play pressed");
    }
}

