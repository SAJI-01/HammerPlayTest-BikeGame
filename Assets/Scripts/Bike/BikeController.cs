using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public class BikeController : MonoBehaviour, IBikeController
{
    [Header("Bike Explosion")]
    public ParticleSystem explosionEffect;
    [Header("Bike Configuration")] 
    [SerializeField] private BikeData bikeData;
    [SerializeField] private BikePhysicsSettings physicsSettings = BikePhysicsSettings.DefaultSettings;

    [Header("Components")] 
    [SerializeField] private WheelJoint2D backWheelJoint;
    [SerializeField] private Rigidbody2D frontWheel;
    [SerializeField] private Rigidbody2D backWheel;
    [SerializeField] private Rigidbody2D bikeBody;
    [SerializeField] private Transform groundCheck;
    
    public BikeData BikeData => bikeData;
    public bool IsGrounded { get; private set; }
    public BikeState CurrentState { get; private set; }

    private HandleBikeInput bikeInput;
    private BikePhysicsController bikePhysicsController;
    private HandleGroundDetection groundDetector;
    private bool canHandleInput;

    private void Awake()
    {
        InitializeComponents();
        explosionEffect?.gameObject.SetActive( false);
        canHandleInput = true;
    }

    private void InitializeComponents()
    {
        bikeInput = new HandleBikeInput();
        bikePhysicsController = 
            new BikePhysicsController(frontWheel, backWheel, bikeBody, backWheelJoint, physicsSettings);
        groundDetector =
            new HandleGroundDetection(groundCheck, physicsSettings.groundLayer, physicsSettings.groundCheckRadius);
    }

    private void Update()
    {
        bikeInput.HandleInput();
    }

    private void FixedUpdate()
    {
        HandleGroundDetection();
        HandleBikeState();
        HandlePhysics();
    }

    private void HandleGroundDetection()
    {
        IsGrounded = groundDetector.CheckIfGrounded();
    }
    private void HandleBikeState()
    {
        if (!IsGrounded)
        {
            CurrentState = BikeState.InAir;
        }
        else if (bikeInput.BrakeInput > 0f)
        {
            CurrentState = BikeState.Braking;
        }
        else if (bikeInput.AccelerationInput > 0f)
        {
            CurrentState = BikeState.Accelerating;
        }
        else
        {
            CurrentState = BikeState.Idle;
        }
    }
    private void HandlePhysics()
    {
        bikePhysicsController.HandleAcceleration(bikeInput.AccelerationInput, IsGrounded);
        bikePhysicsController.HandleBrakes(bikeInput.BrakeInput, IsGrounded);
        bikePhysicsController.HandleTilt(bikeInput.SteerInput, IsGrounded);
    }
    public void SetInputs(float acceleration, float steering, float braking)
    {
        if(canHandleInput)
        bikeInput.SetInputs(acceleration, steering, braking);
    }
    
    //Explosion - bike Wheels goes flying off and reloads the scene
    public IEnumerator Explode()
    {
        canHandleInput = false;
        explosionEffect?.gameObject.SetActive(true);
        explosionEffect?.Play();
        var wheelJoints = GetComponentsInChildren<WheelJoint2D>();
        foreach (var wheelJoint2D in wheelJoints)
        {
            wheelJoint2D.enabled = false;
        }
        backWheel.transform.SetParent(null);
        frontWheel.transform.SetParent(null);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmosSelected()
    {
        groundDetector?.DrawGizmos();
    }
}

