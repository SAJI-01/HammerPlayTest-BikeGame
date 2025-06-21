using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private float angleThreshold = 80f;
    [SerializeField] private ParticleSystem crashEffect;
    [SerializeField] private GameObject frontWheel,backWheel;
    
 
    void Update()
    {
        float zRot = transform.rotation.eulerAngles.z;
        if (zRot > angleThreshold && zRot < 360 - angleThreshold)
        {
            // Trigger crash logic here
            Debug.Log("Crash detected! Bike is upside down.");
            
        }
    }
}