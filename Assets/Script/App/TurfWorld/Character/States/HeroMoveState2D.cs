using SFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework.Game.App;
using SFramework.StateMachine;
using SFramework.Actor;

namespace Game.Character
{
    public class HeroMoveState2D : SFSMState
    {
        private float _horizontalInput;
        private float _verticalInput;
        private float _baseSpeed = 5f;
        private float _speedModifier = 1f; // change the running speed not change the base speed  

        public override void InitState()
        {
        }

        public override void EnterState()
        {
            SAnimatorFSMActor2D hero = (SAnimatorFSMActor2D)Machine.Owner;
            var animator = hero.GetComponent<Animator>();
            hero.PlayAnimation("Move", () =>
            {
                Debug.Log("Move complete!");
            });
        }

        public override void HandleInput()
        {
            if (SFInputManager.Instance != null)
            {
                _horizontalInput = SFInputManager.Instance.PrimaryMovement.x;
                _verticalInput = SFInputManager.Instance.PrimaryMovement.y;
            }
            if(_horizontalInput == 0 && _verticalInput == 0)
            {
                Machine.ChangeState<HeroIdleState2D>();
            }

        }

        public override void UpdateState()
        {
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
            SAnimatorFSMActor2D hero = (SAnimatorFSMActor2D)Machine.Owner;
            hero.ActorRigidBody.AddForce(movementSpeed * movementDirection - currentHorizontalVelocity);
        }

        private float rotate(Vector3 direction)
        {
            // control the degree between 0-360
            // get direction angle 
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            // keep the degree is positive, -90 equal 270, -180 equal 180
            if (directionAngle < 0f)
            {
                directionAngle += 360f;
            }

            // add camera rotation to angle
            SAnimatorFSMActor2D hero = (SAnimatorFSMActor2D)Machine.Owner;
            directionAngle += hero.MainCameraTransform.eulerAngles.y;

            if (directionAngle > 360f)
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
            SAnimatorFSMActor2D hero = (SAnimatorFSMActor2D)Machine.Owner;
            Vector3 vec = hero.ActorRigidBody.velocity;
            vec.y = 0f;
            return vec;
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
