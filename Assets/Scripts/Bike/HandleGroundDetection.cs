using UnityEngine;

public class HandleGroundDetection
{
    private readonly Transform groundCheck;
    private readonly LayerMask groundLayer;
    private readonly float checkRadius;
    
    public HandleGroundDetection(Transform groundCheck, LayerMask groundLayer, float checkRadius)
    {
        this.groundCheck = groundCheck;
        this.groundLayer = groundLayer;
        this.checkRadius = checkRadius;
    }
    
    public bool CheckIfGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }
    
    public void DrawGizmos()
    {
        if (groundCheck == null) return;
        
        Gizmos.color = CheckIfGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}