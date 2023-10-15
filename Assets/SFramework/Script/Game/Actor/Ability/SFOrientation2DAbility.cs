using SFramework.Actor;
using SFramework.Game.App;
using UnityEngine;
namespace SFramework.Actor.Ability
{
    public class SFOrientation2DAbility : SFAbility
    {
        public enum FacingModes { None, MovementDirection, WeaponDirection, Both }
        public enum FacingBases { WeaponAngle, MousePositionX, SceneReticlePositionX }
        public float AbsoluteThresholdMovement = 0.5f;/// the threshold at which movement is considered
        public SFActor.SFActorFacingDirections InitialFacingDirection = SFActor.SFActorFacingDirections.Right;
        public SFActor.SFActorFacingDirections CurrentFacingDirection = SFActor.SFActorFacingDirections.Right;
        public bool IsFacingRight = true; /// whether or not this character is facing right
        private int _direction;
        private int _directionLastFrame = 0;
        private float _lastDirectionX;
        private float _lastDirectionY;
        private float _horizontalDirection;
        private float _verticalDirection;
        private float _directionFloat;
        public override void InitAbility()
        {
            base.InitAbility();
            actControl.CurrentDirection = Vector3.zero;
            if (InitialFacingDirection == SFActor.SFActorFacingDirections.Left)
            {
                IsFacingRight = false;
                _direction = -1;
            }
            else
            {
                IsFacingRight = true;
                _direction = 1;
            }
            Face(InitialFacingDirection);
            _directionLastFrame = 0;
            CurrentFacingDirection = InitialFacingDirection;
            switch (InitialFacingDirection)
            {
                case SFActor.SFActorFacingDirections.Right:
                    _lastDirectionX = 1f;
                    _lastDirectionY = 0f;
                    break;
                case SFActor.SFActorFacingDirections.Left:
                    _lastDirectionX = -1f;
                    _lastDirectionY = 0f;
                    break;
                case SFActor.SFActorFacingDirections.Up:
                    _lastDirectionX = 0f;
                    _lastDirectionY = 1f;
                    break;
                case SFActor.SFActorFacingDirections.Down:
                    _lastDirectionX = 0f;
                    _lastDirectionY = -1f;
                    break;
            }
        }

        public override void UpdateAbility()
        {
            base.UpdateAbility();
            if (conditionMachine.CurrentState != SFAbilityStates.AbilityConditions.Normal)
            {
                return;
            }

            if (!AbilityAuthorized)
            {
                return;
            }
        }

        protected virtual void determineFacingDirection()
        {
            if (actControl.CurrentDirection == Vector3.zero)
            {
                ApplyCurrentDirection();
            }

            if (actControl.CurrentDirection.normalized.magnitude >= AbsoluteThresholdMovement)
            {
                if (Mathf.Abs(actControl.CurrentDirection.y) > Mathf.Abs(actControl.CurrentDirection.x))
                {
                    CurrentFacingDirection = (actControl.CurrentDirection.y > 0) ? SFActor.SFActorFacingDirections.Up : SFActor.SFActorFacingDirections.Down;
                }
                else
                {
                    CurrentFacingDirection = (actControl.CurrentDirection.x > 0) ? SFActor.SFActorFacingDirections.Right : SFActor.SFActorFacingDirections.Left;
                }
                _horizontalDirection = Mathf.Abs(actControl.CurrentDirection.x) >= AbsoluteThresholdMovement ? actControl.CurrentDirection.x : 0f;
                _verticalDirection = Mathf.Abs(actControl.CurrentDirection.y) >= AbsoluteThresholdMovement ? actControl.CurrentDirection.y : 0f;
            }
            else
            {
                _horizontalDirection = _lastDirectionX;
                _verticalDirection = _lastDirectionY;
            }

            switch (CurrentFacingDirection)
            {
                case SFActor.SFActorFacingDirections.Left:
                    _directionFloat = 0f;
                    break;
                case SFActor.SFActorFacingDirections.Up:
                    _directionFloat = 1f;
                    break;
                case SFActor.SFActorFacingDirections.Right:
                    _directionFloat = 2f;
                    break;
                case SFActor.SFActorFacingDirections.Down:
                    _directionFloat = 3f;
                    break;
            }

            _lastDirectionX = _horizontalDirection;
            _lastDirectionY = _verticalDirection;
        }

        protected virtual void ApplyCurrentDirection()
        {
            switch (CurrentFacingDirection)
            {
                case SFActor.SFActorFacingDirections.Right:
                    actControl.CurrentDirection = Vector3.right;
                    break;
                case SFActor.SFActorFacingDirections.Left:
                    actControl.CurrentDirection = Vector3.left;
                    break;
                case SFActor.SFActorFacingDirections.Up:
                    actControl.CurrentDirection = Vector3.up;
                    break;
                case SFActor.SFActorFacingDirections.Down:
                    actControl.CurrentDirection = Vector3.down;
                    break;
            }
        }

        public virtual void Face(SFActor.SFActorFacingDirections direction)
        {
            CurrentFacingDirection = direction;
            ApplyCurrentDirection();
            if (direction == SFActor.SFActorFacingDirections.Right)
            {

            }
            if (direction == SFActor.SFActorFacingDirections.Left)
            {

            }
        }

    }
}