using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    public float angleThreshold = 80f;

    void Update()
    {
        float zRot = transform.rotation.eulerAngles.z;
        if (zRot > angleThreshold && zRot < 360 - angleThreshold)
        {
            FindObjectOfType<LevelManager>().RestartLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 10f)
        {
            FindObjectOfType<LevelManager>().RestartLevel();
        }
    }
}