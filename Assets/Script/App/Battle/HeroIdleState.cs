using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using SFramework.Game.App;
using Game.Character;

public class HeroIdleState : FSMState
{
    private SFInputManager _inputManager;
    private float _horizontalInput;
    private float _verticalInput;

    public override void EnterState()
    {
        base.EnterState();
        Hero hero = (Hero)Machine.BlackBoard;
        var animator = hero.GetComponent<Animator>();
        hero.PlayAnimation("Idle", ()=> {
            Debug.Log("idle complete!");
            Machine.ChangeState<HeroMoveState>();
        });
    }

    public override void HandleInput()
    {
        _horizontalInput = _inputManager.PrimaryMovement.x;
        _verticalInput = _inputManager.PrimaryMovement.y;
    }
}
