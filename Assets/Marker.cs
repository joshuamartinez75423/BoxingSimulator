using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public Transform markedObjectTransform;
    public Transform playerTransform;
    public float maxOpacity = 0.5f;

    private Image img;
    private Coroutine fadeRoutine;

    void Awake()
    {
        img = GetComponent<Image>();
        Color c = img.color;
        c.a = maxOpacity;
        img.color = c;
    }
    void LateUpdate()
    {
        transform.position = markedObjectTransform.position + Vector3.up * 4f;
        transform.LookAt(playerTransform.position);

        bool blocked = IsTargetBlocked();

        if (blocked)
        {
            FadeTo(maxOpacity);
        }
        else
        {
            FadeTo(0f);
        }
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        // World-space height of the camera view at that distance
        float frustumHeight =
            2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);

        // World units per pixel
        float unitsPerPixel = frustumHeight / Screen.height;

        // Final scale factor
        float worldSize = unitsPerPixel * 0.1f;

        // Apply uniform scale
        transform.localScale = Vector3.one * worldSize;
    }

    bool IsTargetBlocked()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = transform.position;
        Vector3 direction = (destination - origin).normalized;
        float distance = Vector3.Distance(origin, destination);

        // Raycast hits something before reaching target
        return (Physics.Raycast(origin, direction, distance));
    }

    void FadeTo(float targetAlpha)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = img.color.a;
        float t = 0;

        while (Mathf.Abs(img.color.a - targetAlpha) > 0.01f)
        {
            t += Time.deltaTime * 5f;

            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            Color c = img.color;
            c.a = newAlpha;
            img.color = c;

            yield return null;
        }

        // Snap exactly at end
        Color final = img.color;
        final.a = targetAlpha;
        img.color = final;
    }
}