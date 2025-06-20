using UnityEngine;

public class BikeController : MonoBehaviour
{
    public float acceleration = 10f;
    public float torque = 7f;
    public Rigidbody2D frontWheel, backWheel;
    public Rigidbody2D bikeBody;

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float tiltInput = Input.GetAxis("Horizontal"); 

        backWheel.AddTorque(-moveInput * acceleration);
        frontWheel.AddTorque(-moveInput * acceleration);
        bikeBody.AddTorque(-tiltInput * torque);
    }
}