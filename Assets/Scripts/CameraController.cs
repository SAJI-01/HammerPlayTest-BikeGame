using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;

    [Header("Zoom Settings")]
    [SerializeField] private float minOrthographicSize = 5f;
    [SerializeField] private float maxOrthographicSize = 10f;
    [SerializeField] private float zoomLerpSpeed = 2f;
    [SerializeField] private float maxGroundDistance = 10f;
    [SerializeField] private GameObject skyBackground;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 50f;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (!target) return;

        // Follow player smoothly
        Vector3 desiredPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // Raycast down to check distance from ground
        RaycastHit2D hit = Physics2D.Raycast(target.position, Vector2.down, raycastDistance, groundLayer);
        float distanceToGround = hit.collider ? hit.distance : maxGroundDistance;

        // Map distance to zoom range
        float t = Mathf.InverseLerp(0, maxGroundDistance, distanceToGround);
        float targetZoom = Mathf.Lerp(minOrthographicSize, maxOrthographicSize, t);

        // Smooth zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomLerpSpeed * Time.deltaTime);
        
        //increase and decrease background based on orthographic size
        if (skyBackground != null )
        {
            skyBackground.transform.localScale = Vector3.one * (1 + (cam.orthographicSize - minOrthographicSize) / (maxOrthographicSize - minOrthographicSize));
        }
        
    }
}