using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Actor.Ability
{
	/// <summary>
	/// Add this ability to a character and it'll be able to jump in 2D. This means no actual movement, only the collider turned off and on. Movement will be handled by the animation itself.
	/// </summary>
	public class SFJump2D : SFAbility
	{
        public bool JumpProportionalToPress = true; // whether or not the jump should be proportional to press (if yes, releasing the button will stop the jump)
        public float MinimumPressTime = 0.4f;// the minimum amount of time after the jump's start before releasing the jump button has any effect
        public float JumpForce = 800f; // the force to apply to the jump, the higher the jump, the faster the jump
        public float JumpHeight = 4f; // the height the jump should have
        public bool CanJumpOnTooSteepSlopes = true;
        public bool ResetJumpsOnTooSteepSlopes = false;
		protected override void init()
		{
		}

	}
}