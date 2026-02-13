using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 endPosition;
    MeshRenderer meshRender;

    void Awake()
    {
        initialPosition = transform.position;
        endPosition = transform.position + Vector3.up * 3;
        meshRender = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
        StartCoroutine(Disappear(1f));
    }

    public IEnumerator Disappear(float timeToDisappear)
    {
        float t = 0f;
        while (t < timeToDisappear)
        {
            t += Time.deltaTime / timeToDisappear;

            transform.position = Vector3.Lerp(initialPosition, endPosition, t);
            meshRender.material.color = new Color(1f, 1f, 1f, 1 - t);


            yield return null;
        }
    }
}
