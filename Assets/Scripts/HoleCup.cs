using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCup : MonoBehaviour
{
    public string ballTag = "Ball";
    public Transform cupCenter;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ballTag)) return;

        var sink = other.GetComponent<BallSinkEffect>();
        if (sink != null && cupCenter != null)
        {
            StartCoroutine(sink.SinkInto(cupCenter));
        }
        else
        {
            // fallback: just hide
            other.gameObject.SetActive(false);
        }
    }

}

