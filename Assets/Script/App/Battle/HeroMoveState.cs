using SFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework.Game.App;
using SFramework.StateMachine;
using Game.Character;

public class HeroMoveState : FSMState
{
    private SFInputManager _inputManager;
    private float _horizontalInput;
    private float _verticalInput;
    private float _baseSpeed = 5f;
    private float _speedModifier = 1f; // change the running speed not change the base speed  

    public override void InitState()
    {
        base.InitState();
        _inputManager = SFInputManager.Instance;
    }

    public override void EnterState()
    {
        base.EnterState();
        Hero hero = (Hero)Machine.BlackBoard;
        var animator = hero.GetComponent<Animator>();
        hero.PlayAnimation("Move", () => {
            Debug.Log("Move complete!");
        });
    }

    public override void HandleInput()
    {
        _horizontalInput = _inputManager.PrimaryMovement.x;
        _verticalInput = _inputManager.PrimaryMovement.y;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        HandleInput();
        move();
    }

    private void move()
    {
        if ((_horizontalInput == 0f && _verticalInput == 0f) || _speedModifier == 0f)
            return;

        Vector3 movementDirection = getMovementInputDirection();
        float movementSpeed = getMovementSpeed();
        Vector3 currentHorizontalVelocity = getHorizontalVelocity();
        Hero hero = (Hero)Machine.BlackBoard;
        hero.ActorRigidBody.AddForce(movementSpeed * movementDirection - currentHorizontalVelocity, ForceMode.VelocityChange);
    }

    private float rotate(Vector3 direction)
    {
        // control the degree between 0-360
        // get direction angle 
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        // keep the degree is positive, -90 equal 270, -180 equal 180
        if(directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        // add camera rotation to angle
        Hero hero = (Hero)Machine.BlackBoard;
        directionAngle += hero.MainCameraTransform.eulerAngles.y;

        if(directionAngle > 360f)
        {
            directionAngle -= 360f;
        }

        return directionAngle;
    }

    #region Resuable Methods
    protected Vector3 getMovementInputDirection()
    {
        return new Vector3(_horizontalInput, 0, _verticalInput);
    }

    protected float getMovementSpeed()
    {
        return _baseSpeed * _speedModifier;
    }

    protected Vector3 getHorizontalVelocity()
    {
        Hero hero = (Hero)Machine.BlackBoard;
        Vector3 vec = hero.ActorRigidBody.velocity;
        vec.y = 0f;
        return vec;
    }
    #endregion
}
