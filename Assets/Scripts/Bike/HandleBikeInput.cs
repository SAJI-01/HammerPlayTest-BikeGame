using UnityEngine;

public class HandleBikeInput
{
    public float AccelerationInput { get; private set; }
    public float SteerInput { get; private set; }
    public float BrakeInput { get; private set; }
    
    public void HandleInput()
    {
        AccelerationInput = Input.GetKey(KeyCode.W) ? 1f : 0f;
        BrakeInput = Input.GetKey(KeyCode.S) ? 1f : 0f;
        
        SteerInput = 0f;
        if (Input.GetKey(KeyCode.A))
            SteerInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            SteerInput = 1f;
    }
    
    public void SetInputs(float acceleration, float steering, float braking)
    {
        AccelerationInput = Mathf.Clamp01(acceleration);
        SteerInput = Mathf.Clamp(steering, -1f, 1f);
        BrakeInput = Mathf.Clamp01(braking);
    }
}