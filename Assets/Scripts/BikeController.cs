using UnityEngine;
public enum BikeState
{
    Idle,
    Accelerating,
    Braking,
    InAir
}

[System.Serializable]
public struct BikeData
{
    public string name;
    public bool isUnlocked;
    public bool isSelected;
    public int price;
}

[System.Serializable]
public struct BikePhysicsSettings
{
    [Header("Motor Settings")]
    public float motorTorque;
    
    [Header("Steering Settings")]
    public float tiltForce;
    public float airTiltForce;
    
    [Header("Brake Settings")]
    public float brakeForce;
    public float wheelieForce;
    public float wheelieTorque;
    public float maxAngularVelocity;
    
    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public float groundCheckRadius;
    
    public static BikePhysicsSettings DefaultSettings => new BikePhysicsSettings
    {
        motorTorque = 1500f,
        tiltForce = 1000f,
        airTiltForce = 1000f,
        brakeForce = 2000f,
        wheelieForce = 800f,
        wheelieTorque = 500f,
        maxAngularVelocity = 20f,
        groundCheckRadius = 0.2f
    };
}
public class BikeInputHandler
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

public class BikeController : MonoBehaviour, IBikeController
{
    [Header("Bike Configuration")] [SerializeField]
    private BikeData bikeData;

    [SerializeField] private BikePhysicsSettings physicsSettings = BikePhysicsSettings.DefaultSettings;

    [Header("Components")] [SerializeField]
    private WheelJoint2D backWheelJoint;

    [SerializeField] private Rigidbody2D frontWheel;
    [SerializeField] private Rigidbody2D backWheel;
    [SerializeField] private Rigidbody2D bikeBody;
    [SerializeField] private Transform groundCheck;

    // Public Properties
    public BikeData BikeData => bikeData;
    public bool IsGrounded { get; private set; }
    public BikeState CurrentState { get; private set; }

    // Private Fields
    private BikeInputHandler inputHandler;
    private BikePhysicsController physicsController;
    private GroundDetector groundDetector;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        inputHandler = new BikeInputHandler();
        physicsController = new BikePhysicsController(
            frontWheel, backWheel, bikeBody, backWheelJoint, physicsSettings);
        groundDetector =
            new GroundDetector(groundCheck, physicsSettings.groundLayer, physicsSettings.groundCheckRadius);
    }

    private void Update()
    {
        inputHandler.HandleInput();
    }

    private void FixedUpdate()
    {
        UpdateGroundStatus();
        UpdateBikeState();
        ApplyPhysics();
    }

    private void UpdateGroundStatus()
    {
        IsGrounded = groundDetector.CheckGrounded();
    }

    private void UpdateBikeState()
    {
        if (!IsGrounded)
        {
            CurrentState = BikeState.InAir;
        }
        else if (inputHandler.BrakeInput > 0f)
        {
            CurrentState = BikeState.Braking;
        }
        else if (inputHandler.AccelerationInput > 0f)
        {
            CurrentState = BikeState.Accelerating;
        }
        else
        {
            CurrentState = BikeState.Idle;
        }
    }

    private void ApplyPhysics()
    {
        physicsController.ApplyMotor(inputHandler.AccelerationInput, IsGrounded);
        physicsController.ApplyBrakes(inputHandler.BrakeInput, IsGrounded);
        physicsController.ApplyTilt(inputHandler.SteerInput, IsGrounded);
    }

    public void SetInputs(float acceleration, float steering, float braking)
    {
        inputHandler.SetInputs(acceleration, steering, braking);
    }

    private void OnDrawGizmosSelected()
    {
        groundDetector?.DrawGizmos();
    }
}

public class GroundDetector
{
    private readonly Transform groundCheck;
    private readonly LayerMask groundLayer;
    private readonly float checkRadius;
    
    public GroundDetector(Transform groundCheck, LayerMask groundLayer, float checkRadius)
    {
        this.groundCheck = groundCheck;
        this.groundLayer = groundLayer;
        this.checkRadius = checkRadius;
    }
    
    public bool CheckGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }
    
    public void DrawGizmos()
    {
        if (groundCheck == null) return;
        
        Gizmos.color = CheckGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}