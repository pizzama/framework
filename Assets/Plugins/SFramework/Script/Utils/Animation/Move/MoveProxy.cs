using UnityEngine;
public interface IMoveProxy<T>
{
    public void MoveByTarget(Vector3 mouseWorldPosition);
    public void MoveByInput(float inputX, float inputY);

    public void SetControl(T control);

    public bool IsGrounded();
}

public abstract class MoveProxy<T> : IMoveProxy<T>
{
    protected bool isJump;
    protected float jumpForce = 8f;
    protected float speed = 1f;
    protected float rotateSpeed = 180f;
    protected Vector3 movementVelocity;
    [SerializeField]
    protected T playerControl;

    public virtual void MoveByInput(float inputX, float inputY)
    {
        throw new System.NotImplementedException();
    }

    public virtual void MoveByTarget(Vector3 mouseWorldPosition)
    {
        throw new System.NotImplementedException();
    }

    public void SetControl(T control)
    {
        playerControl = control;
    }

    public float GetCurrentSpeed()
    {
        return movementVelocity.magnitude;
    }

    public void SetDefaultSpeed(float value)
    {
        speed = value;
    }

    public void SetDefaultRotateSpeed(float value)
    {
        rotateSpeed = value;
    }

    public void SetDefaultJumpForce(float value)
    {
        jumpForce = value;
    }

    public void StartJump()
    {
        isJump = true;
    }

    public void EndJump()
    {
        isJump = false;
    }

    public void ResetMovementVelocity()
    {
        movementVelocity = Vector3.zero;
    }

    public virtual bool IsGrounded()
    {
        throw new System.NotImplementedException();
    }
}