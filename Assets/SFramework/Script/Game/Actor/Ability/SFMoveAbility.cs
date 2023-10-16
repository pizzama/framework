using SFramework.Actor;
using SFramework.Game.App;
using UnityEngine;
namespace SFramework.Actor.Ability
{
    public class SFMoveAbility : SFAbility
    {
        public enum MovementsType { Free, Strict2DirectionsHorizontal, Strict2DirectionsVertical, Strict4Directions, Strict8Directions }
        public bool AnalogInput = false; // whether or not input should be analog
        public bool InterpolateMovementSpeed = false; // whether or not to interpolate movement speed
        public bool InputAuthorized = false; // whether or not need inputmanager input;
        public MovementsType Movements = MovementsType.Free;
        [Header("Property Value")]
        public float IdleThreshold = 0.05f; // the speed threshold after which the character is not considered idle anymore
        public float Acceleration = 10f; //the acceleration to apply to the current speed / 0f : no acceleration, instant full speed
        public float Deceleration = 10f; //the deceleration to apply to the current speed / 0f : no deceleration, instant stop
        public float MovementSpeed { get; set; }
        public float WalkSpeed = 6f;
        public float HorizontalMovement;
        public float VerticalMovement;
        private Vector2 _lerpedInput = Vector2.zero;
        private float _acceleration = 0f;
        protected override void init()
        {
            MovementSpeed = WalkSpeed;
        }

        public override void UpdateAbility()
        {
            if (AbilityAuthorized && conditionMachine != null && movementMachine != null)
            {
                if (handleFrozen())
                    return;
                if (InputAuthorized)
                {
                    handleInput();
                }
                handleDirection();
                handleMovement();
            }
        }

        protected virtual void handleDirection()
        {
            switch (Movements)
            {
                case MovementsType.Free:
                    // do nothing
                    break;
                case MovementsType.Strict2DirectionsHorizontal:
                    VerticalMovement = 0f;
                    break;
                case MovementsType.Strict2DirectionsVertical:
                    HorizontalMovement = 0f;
                    break;
                case MovementsType.Strict4Directions:
                    if (Mathf.Abs(HorizontalMovement) > Mathf.Abs(VerticalMovement))
                    {
                        VerticalMovement = 0f;
                    }
                    else
                    {
                        HorizontalMovement = 0f;
                    }
                    break;
                case MovementsType.Strict8Directions:
                    VerticalMovement = Mathf.Round(VerticalMovement);
                    HorizontalMovement = Mathf.Round(HorizontalMovement);
                    break;
            }
        }

        protected virtual void handleMovement()
        {
            // if movement is prevented, or if the character is dead/frozen/can't move, we exit and do nothing
            if (!AbilityInitialized
                 || (conditionMachine.CurrentState != SFAbilityStates.AbilityConditions.Normal))
            {
                return;
            }

            CheckJustGotGrounded();

            if (!actControl.Grounded
                && (conditionMachine.CurrentState == SFAbilityStates.AbilityConditions.Normal)
                && (
                    (movementMachine.CurrentState == SFAbilityStates.AbilityStates.Walking)
                    || (movementMachine.CurrentState == SFAbilityStates.AbilityStates.Idle)
                ))
            {
                movementMachine.ChangeState(SFAbilityStates.AbilityStates.Falling);
            }

            if (actControl.Grounded && (movementMachine.CurrentState == SFAbilityStates.AbilityStates.Falling))
            {
                movementMachine.ChangeState(SFAbilityStates.AbilityStates.Idle);
            }

            if (actControl.Grounded
                 && (actControl.CurrentMovement.magnitude > IdleThreshold)
                 && (movementMachine.CurrentState == SFAbilityStates.AbilityStates.Idle))
            {
                movementMachine.ChangeState(SFAbilityStates.AbilityStates.Walking);
            }

            if ((movementMachine.CurrentState == SFAbilityStates.AbilityStates.Walking)
                && (actControl.CurrentMovement.magnitude <= IdleThreshold))
            {
                movementMachine.ChangeState(SFAbilityStates.AbilityStates.Idle);
            }

            SetMovement();
        }

        protected virtual bool handleFrozen()
        {
            if (conditionMachine.CurrentState == SFAbilityStates.AbilityConditions.Frozen)
            {
                HorizontalMovement = 0f;
                VerticalMovement = 0f;
                SetMovement();

                return true;
            }

            return false;
        }

        protected void handleInput()
        {
            HorizontalMovement = SFInputManager.Instance.PrimaryMovement.x;
            VerticalMovement = SFInputManager.Instance.PrimaryMovement.y;
        }

        protected virtual void CheckJustGotGrounded()
        {
            // if the character just got grounded
            if (actControl.JustGotGrounded)
            {
                movementMachine.ChangeState(SFAbilityStates.AbilityStates.Idle);
            }
        }

        protected virtual void SetMovement()
        {
            Vector3 movementVector = Vector3.zero;
            Vector2 currentInput = Vector2.zero;

            currentInput.x = HorizontalMovement;
            currentInput.y = VerticalMovement;

            Vector2 normalizedInput = currentInput.normalized;

            float interpolationSpeed = 1f;

            if ((Acceleration == 0) || (Deceleration == 0))
            {
                _lerpedInput = AnalogInput ? currentInput : normalizedInput;
            }
            else
            {
                if (normalizedInput.magnitude == 0)
                {
                    _acceleration = Mathf.Lerp(_acceleration, 0f, Deceleration * Time.deltaTime);
                    _lerpedInput = Vector2.Lerp(_lerpedInput, _lerpedInput * _acceleration, Time.deltaTime * Deceleration);
                    interpolationSpeed = Deceleration;
                }
                else
                {
                    _acceleration = Mathf.Lerp(_acceleration, 1f, Acceleration * Time.deltaTime);
                    _lerpedInput = AnalogInput ? Vector2.ClampMagnitude(currentInput, _acceleration) : Vector2.ClampMagnitude(normalizedInput, _acceleration);
                    interpolationSpeed = Acceleration;
                }
            }

            movementVector.x = _lerpedInput.x;
            movementVector.y = 0f;
            movementVector.z = _lerpedInput.y;
            // float movementSpeed = 0f;
            // if (InterpolateMovementSpeed)
            // {
            //     movementSpeed = Mathf.Lerp(movementSpeed, MovementSpeed * ContextSpeedMultiplier * MovementSpeedMultiplier, interpolationSpeed * Time.deltaTime);
            // }
            // else
            // {
            //     movementSpeed = MovementSpeed * MovementSpeedMultiplier * ContextSpeedMultiplier;
            // }

            // movementVector *= movementSpeed;

            // if (movementVector.magnitude > MovementSpeed * ContextSpeedMultiplier * MovementSpeedMultiplier)
            // {
            //     movementVector = Vector3.ClampMagnitude(movementVector, MovementSpeed);
            // }

            if ((currentInput.magnitude <= IdleThreshold) && (actControl.CurrentMovement.magnitude < IdleThreshold))
            {
                movementVector = Vector3.zero;
            }

            actControl.SetMovement(movementVector);
        }
    }
}