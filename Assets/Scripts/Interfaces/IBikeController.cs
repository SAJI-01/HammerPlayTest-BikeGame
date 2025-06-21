public interface IBikeController
{
    BikeData BikeData { get; }
    bool IsGrounded { get; }
    void SetInputs(float acceleration, float steering, float braking);
}