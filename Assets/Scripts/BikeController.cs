using UnityEngine;

public class BikeController : MonoBehaviour
{
    [Header("Bike Settings")]
    [SerializeField] private float motorTorque = 1500f;
    [SerializeField] private float tiltForce = 1000f;
    [SerializeField] private float onAirTiltForce = 1000f;

    [Header("Brake Physics")] 
    [SerializeField] private float brakeForce = 2000f;
    [SerializeField] private float wheelieForce = 800f;
    [SerializeField] private float wheelieTorque = 500f;
    [SerializeField] private float angularVelocityAmount = 20f;

    [Header("Wheel Components")] 
    [SerializeField] private WheelJoint2D backWheelJoint;
    [SerializeField] private Rigidbody2D frontWheel;
    [SerializeField] private Rigidbody2D backWheel;
    [SerializeField] private Rigidbody2D bikeBody;

    [Header("Ground Check")] public Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private bool isGrounded;
    private float brakeInput;
    private float steerInput;

    private void Update()
    {
        brakeInput = 0f;
        steerInput = 0f;

        if (Input.GetKey(KeyCode.S))
            brakeInput = 1f;

        if (Input.GetKey(KeyCode.A))
            steerInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            steerInput = 1f;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            frontWheel.AddTorque(-motorTorque);
            backWheel.AddTorque(-motorTorque);
        }
        if (brakeInput > 0) ApplyBrakes();
        if (Mathf.Abs(steerInput) > 0) ApplyTilt();
    }


    private void ApplyBrakes()
    {
        // Stop wheel rotation quickly
        if (frontWheel != null)
            frontWheel.angularVelocity =
                Mathf.Lerp(frontWheel.angularVelocity, 0, brakeInput * 10f * Time.fixedDeltaTime);

        if (backWheel != null)
            backWheel.angularVelocity =
                Mathf.Lerp(backWheel.angularVelocity, 0, brakeInput * 10f * Time.fixedDeltaTime);

        // Disable motor when braking
        if (backWheelJoint != null) backWheelJoint.useMotor = false;

        // Apply brake force to slow down the bike
        var brakeForceVector = -bikeBody.linearVelocity.normalized * brakeForce * brakeInput;
        bikeBody.AddForce(brakeForceVector);

        // Realistic brake physics - front wheel dives down, back wheel lifts up
        if (isGrounded && bikeBody.linearVelocity.magnitude > 2f)
        {
            // Apply forward rotational force (front dips, back lifts)
            bikeBody.AddTorque(-wheelieTorque * brakeInput);

            // Add upward force to the back of the bike
            Vector2 backPosition = bikeBody.transform.position + bikeBody.transform.right * -0.5f;
            bikeBody.AddForceAtPosition(Vector2.up * wheelieForce * brakeInput, backPosition);

            // Add downward force to the front
            Vector2 frontPosition = bikeBody.transform.position + bikeBody.transform.right * 0.5f;
            bikeBody.AddForceAtPosition(Vector2.down * wheelieForce * 0.5f * brakeInput, frontPosition);
        }
    }

    private void ApplyTilt()
    {
        bikeBody.AddTorque(-steerInput * tiltForce * Time.fixedDeltaTime);
        if (!isGrounded)
        {
            bikeBody.AddTorque(-steerInput * onAirTiltForce * 2f * Time.fixedDeltaTime);
            LimitBikeRotation();
        }
    }

    private void LimitBikeRotation()
    {
        if (bikeBody.angularVelocity > 10f)
            bikeBody.angularVelocity = angularVelocityAmount;
        else if (bikeBody.angularVelocity < -10f)
            bikeBody.angularVelocity = -angularVelocityAmount;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}