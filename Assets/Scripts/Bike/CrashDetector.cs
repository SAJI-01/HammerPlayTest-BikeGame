using System;
using System.Collections;
using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // If the object is a player or bike, trigger the explosion
            StartCoroutine(transform.parent.GetComponent<BikeController>().Explode());
        }

    }
}