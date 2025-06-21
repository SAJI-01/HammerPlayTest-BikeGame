using System;
using UnityEngine;

public class NormalHitBox : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision with the spike trap
        if (other.CompareTag("Player"))
        {
            // Apply damage or effects to the player
            Debug.Log("Player hit by spike trap!");
            restartLevel();
        }
    }

    private void restartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}