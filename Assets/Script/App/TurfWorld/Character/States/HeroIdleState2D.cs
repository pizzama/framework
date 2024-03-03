using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework.StateMachine;
using SFramework.Game.App;
namespace Game.Character
{
    public class HeroIdleState2D : FSMState
    {
        private float _horizontalInput;
        private float _verticalInput;


        public virtual void InitState()
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Koala hero = (Koala)Machine.Owner;
            var animator = hero.GetComponent<Animator>();
            hero.PlayAnimation("Idle", () =>
            {
                Debug.Log("idle complete!");
                // Machine.ChangeState<HeroMoveState2D>();
            });
        }

        public override void HandleInput()
        {
            if (SFInputManager.Instance != null)
            {
                _horizontalInput = SFInputManager.Instance.PrimaryMovement.x;
                _verticalInput = SFInputManager.Instance.PrimaryMovement.y;
            }

        }

        public override void UpdateState()
        {
            HandleInput();
            if (_horizontalInput != 0 || _verticalInput != 0)
            {
                Machine.ChangeState<HeroMoveState2D>();
            }
        }


        public virtual void ResetInput()
        {
            _horizontalInput = 0f;
            _verticalInput = 0f;
        }
    }
}