using SFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework.Game.App;
using Game.Character;

public class HeroMoveState : FSMState
{
    private SFInputManager _inputManager;
    private float _horizontalInput;
    private float _verticalInput;
    private float _baseSpeed = 5f;
    private float _speedModifier = 1f; // change the running speed not change the base speed  

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
    }

    private void move()
    {
        if ((_horizontalInput == 0f && _verticalInput == 0f) || _speedModifier == 0f)
            return;

        Vector3 movementDirection = getMovementInputDirection();
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
    #endregion
}
