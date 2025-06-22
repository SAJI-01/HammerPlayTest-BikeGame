using UnityEngine;

public class BikePhysicsController
{
    private readonly Rigidbody2D frontWheel;
    private readonly Rigidbody2D backWheel;
    private readonly Rigidbody2D bikeBody;
    private readonly WheelJoint2D backWheelJoint;
    private readonly BikePhysicsSettings settings;
    
    public BikePhysicsController(Rigidbody2D frontWheel, Rigidbody2D backWheel, 
        Rigidbody2D bikeBody, WheelJoint2D backWheelJoint, BikePhysicsSettings settings)
    {
        this.frontWheel = frontWheel;
        this.backWheel = backWheel;
        this.bikeBody = bikeBody;
        this.backWheelJoint = backWheelJoint;
        this.settings = settings;
    }
    
    public void HandleAcceleration(float motorInput, bool isGrounded)
    {
        if (motorInput > 0f && isGrounded)
        {
            float torque = -settings.motorTorque * motorInput;
            frontWheel?.AddTorque(torque);
            backWheel?.AddTorque(torque);
        }
    }
    
    public void HandleBrakes(float brakeInput, bool isGrounded)
    {
        if (brakeInput <= 0f) return;
        
        HandleWheelBraking(brakeInput);
        HandleBodyBraking(brakeInput);
        
        if (isGrounded && bikeBody.linearVelocity.magnitude > 2f)
        {
            HandleBrakePhysics(brakeInput);
        }
    }
    
    private void HandleWheelBraking(float brakeInput)
    {
        float dampingFactor = brakeInput * 10f * Time.fixedDeltaTime;
        
        if (frontWheel != null)
            frontWheel.angularVelocity = Mathf.Lerp(frontWheel.angularVelocity, 0f, dampingFactor);
        
        if (backWheel != null)
            backWheel.angularVelocity = Mathf.Lerp(backWheel.angularVelocity, 0f, dampingFactor);
        
        if (backWheelJoint != null)
            backWheelJoint.useMotor = false;
    }
    
    private void HandleBodyBraking(float brakeInput)
    {
        Vector2 brakeForceVector = -bikeBody.linearVelocity.normalized * settings.brakeForce * brakeInput;
        bikeBody.AddForce(brakeForceVector);
    }
    
    private void HandleBrakePhysics(float brakeInput)
    {
        bikeBody.AddTorque(-settings.wheelieTorque * brakeInput);
        
        Vector2 backPosition = bikeBody.transform.position + bikeBody.transform.right * -0.5f;
        Vector2 frontPosition = bikeBody.transform.position + bikeBody.transform.right * 0.5f;
        
        bikeBody.AddForceAtPosition(Vector2.up * settings.wheelieForce * brakeInput, backPosition);
        bikeBody.AddForceAtPosition(Vector2.down * settings.wheelieForce * 0.5f * brakeInput, frontPosition);
    }
    
    public void HandleTilt(float steerInput, bool isGrounded)
    {
        if (Mathf.Abs(steerInput) <= 0f) return;
        
        float tiltForce = isGrounded ? settings.tiltForce : settings.airTiltForce;
        float multiplier = isGrounded ? 1f : 2f;
        
        bikeBody.AddTorque(-steerInput * tiltForce * multiplier * Time.fixedDeltaTime);
        
        if (!isGrounded)
        {
            LimitAngularVelocity();
        }
    }
    
    private void LimitAngularVelocity()
    {
        float maxVelocity = settings.maxAngularVelocity;
        bikeBody.angularVelocity = Mathf.Clamp(bikeBody.angularVelocity, -maxVelocity, maxVelocity);
    }
}