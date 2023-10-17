using SFramework.Actor;
using SFramework.Game.App;
using UnityEngine;
namespace SFramework.Actor.Ability
{
    public class SFOrientation2D : SFAbility
    {
        public enum FacingModes { None, MovementDirection, WeaponDirection, Both }
        public enum FacingBases { WeaponAngle, MousePositionX, SceneReticlePositionX }
        public FacingModes FacingMode = FacingModes.None;
        public bool ModelShouldFlip = false;
        public bool ModelShouldRotate = false;
        public bool IsFacingRight = true; // whether or not this character is facing right
        [Header("Property Value")]
        public float AbsoluteThresholdMovement = 0.5f;
        public float ModelRotationSpeed = 0f;// the speed at which to rotate the model when changing direction, 0f means instant rotation
        public Vector3 ModelRotationValueLeft = new Vector3(0f, 180f, 0f);
        public Vector3 ModelRotationValueRight = new Vector3(0f, 0f, 0f); // the threshold at which movement is considered
        public Vector3 ModelFlipValueLeft = new Vector3(-1, 1, 1); // the scale value to apply to the model when facing left
        public Vector3 ModelFlipValueRight = new Vector3(1, 1, 1); // the scale value to apply to the model when facing right
        public SFActor.SFActorFacingDirections InitialFacingDirection = SFActor.SFActorFacingDirections.Right;
        public SFActor.SFActorFacingDirections CurrentFacingDirection = SFActor.SFActorFacingDirections.Right;
        private int _direction;
        private int _directionLastFrame = 0;
        private float _lastDirectionX;
        private float _lastDirectionY;
        private float _horizontalDirection;
        private float _verticalDirection;
        private float _directionFloat;
        private Vector3 _targetModelRotation;
        private float _lastNonNullXMovement;
        private void Start()
        {
            
        }

        protected override void init()
        {
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

            determineFacingDirection();
            flipToFaceMovementDirection();
            flipToFaceWeaponDirection();
            applyModelRotation();
            flipAbilities();

            _directionLastFrame = _direction;
            _lastNonNullXMovement = (Mathf.Abs(actControl.CurrentDirection.x) > 0) ? actControl.CurrentDirection.x : _lastNonNullXMovement;
        }

        public virtual void Face(SFActor.SFActorFacingDirections direction)
        {
            CurrentFacingDirection = direction;
            applyCurrentDirection();
            if (direction == SFActor.SFActorFacingDirections.Right)
            {
                FaceDirection(-1);
            }
            if (direction == SFActor.SFActorFacingDirections.Left)
            {
                FaceDirection(1);
            }
        }

        public virtual void FaceDirection(int direction)
        {
            if (ModelShouldFlip)
            {
                FlipModel(direction);
            }

            if (ModelShouldRotate)
            {
                rotateModel(direction);
            }

            _direction = direction;
            IsFacingRight = _direction == 1;
        }

        public virtual void FlipModel(int direction)
        {
            if (act != null)
            {
                act.transform.localScale = (direction == 1) ? ModelFlipValueRight : ModelFlipValueLeft;
            }
        }

        protected virtual void determineFacingDirection()
        {
            if (actControl.CurrentDirection == Vector3.zero)
            {
                applyCurrentDirection();
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

        protected virtual void applyCurrentDirection()
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

        protected virtual void applyModelRotation()
        {
            if (!ModelShouldRotate)
            {
                return;
            }

            if (ModelRotationSpeed > 0f)
            {
                act.transform.localEulerAngles = Vector3.Lerp(act.transform.localEulerAngles, _targetModelRotation, Time.deltaTime * ModelRotationSpeed);
            }
            else
            {
                act.transform.localEulerAngles = _targetModelRotation;
            }
        }

        protected virtual void flipToFaceMovementDirection()
        {
            // if we're not supposed to face our direction, we do nothing and exit
            if ((FacingMode != FacingModes.MovementDirection) && (FacingMode != FacingModes.Both)) { return; }
            Debug.Log(actControl.CurrentDirection.normalized);
            if (actControl.CurrentDirection.normalized.magnitude >= AbsoluteThresholdMovement)
            {
                float checkedDirection = (Mathf.Abs(actControl.CurrentDirection.normalized.x) > 0) ? actControl.CurrentDirection.normalized.x : _lastNonNullXMovement;

                if (checkedDirection >= 0)
                {
                    FaceDirection(1);
                }
                else
                {
                    FaceDirection(-1);
                }
            }
        }

        protected virtual void rotateModel(int direction)
        {
            if (act != null)
            {
                _targetModelRotation = (direction == 1) ? ModelRotationValueRight : ModelRotationValueLeft;
                _targetModelRotation.x = _targetModelRotation.x % 360;
                _targetModelRotation.y = _targetModelRotation.y % 360;
                _targetModelRotation.z = _targetModelRotation.z % 360;
            }
        }

        protected virtual void flipToFaceWeaponDirection()
        {

        }

        protected virtual void flipAbilities()
        {

        }

    }
}