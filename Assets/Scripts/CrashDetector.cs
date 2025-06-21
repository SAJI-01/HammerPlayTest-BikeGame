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
}