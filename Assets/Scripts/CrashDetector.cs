using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    public float angleThreshold = 80f;

    void Update()
    {
        float zRot = transform.rotation.eulerAngles.z;
        if (zRot > angleThreshold && zRot < 360 - angleThreshold)
        {
            // Trigger crash logic here
            Debug.Log("Crash detected! Bike is upside down.");
            // You can add more logic here, like resetting the bike position or playing a sound.
        }
    }
}