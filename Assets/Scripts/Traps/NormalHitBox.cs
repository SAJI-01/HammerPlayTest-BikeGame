using System;
using UnityEngine;

public class NormalHitBox : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by spike trap!");
            StartCoroutine(other.GetComponent<BikeController>().Explode());
        }
    }
}