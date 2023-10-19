using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework.Event;

namespace SFramework.Actor.Ability
{
	/// <summary>
	/// Add this ability to a character and it'll be able to jump in 2D. This means no actual movement, only the collider turned off and on. Movement will be handled by the animation itself.
	/// </summary>
	public class SFJump2D : SFAbility
	{
		public bool InputAuthorized = false; // whether or not need inputmanager input;
		public bool JumpProportionalToPress = true; // whether or not the jump should be proportional to press (if yes, releasing the button will stop the jump)
		public float MinimumPressTime = 0.4f;// the minimum amount of time after the jump's start before releasing the jump button has any effect
		public float JumpForce = 800f; // the force to apply to the jump, the higher the jump, the faster the jump
		public float JumpHeight = 4f; // the height the jump should have
		public float JumpDuration = 1f; // the duration of the jump
		public bool CanJumpOnTooSteepSlopes = true;
		public bool ResetJumpsOnTooSteepSlopes = false;

		private bool _jumpStopped = false;
		private float _jumpStartedAt = 0f;

		protected override void init()
		{
			AddListener("start", () =>
			{

			});
		}

        public override void DestroyAbility()
        {
            base.DestroyAbility();
			RemoveListener("start");
        }

        public override void UpdateAbility()
		{
			if (InputAuthorized)
			{
				handleInput();
			}
			// On process ability, we stop the jump if needed
			if (movementMachine.CurrentState == SFAbilityStates.AbilityStates.Jumping)
			{
				if (!_jumpStopped)
				{
					if (Time.time - _jumpStartedAt >= JumpDuration)
					{
						JumpStop();
					}
					else
					{
						movementMachine.ChangeState(SFAbilityStates.AbilityStates.Jumping);
					}
				}
				if (!_jumpStopped && JumpProportionalToPress && (Time.time - _jumpStartedAt > MinimumPressTime))
				{
					JumpStop();
				}
			}
		}

		public virtual void JumpStart()
		{
			if (!AbilityAuthorized)
			{
				return;
			}
			movementMachine.ChangeState(SFAbilityStates.AbilityStates.Jumping);
			_jumpStopped = false;
			_jumpStartedAt = Time.time;
		}

		public virtual void JumpStop()
		{
			_jumpStopped = true;
		}

		protected void handleInput()
		{
			// if movement is prevented, or if the character is dead/frozen/can't move, we exit and do nothing
			if (!AbilityAuthorized
				|| (conditionMachine.CurrentState != SFAbilityStates.AbilityConditions.Normal))
			{
				return;
			}
		}
    }
}