using UnityEngine;
using System.Collections;
using System;

namespace SFramework.Actor.Ability
{	
	/// <summary>
	/// The various states you can use to check if your character is doing something at the current frame
	/// </summary>    
	public class SFAbilityStates 
	{
		// The possible character conditions
		
		public enum AbilityConditions
		{
			Normal,
			ControlledMovement,
			Frozen,
			Paused,
			Dead,
			Stunned
		}

		// The possible Movement States the character can be in. These usually correspond to their own class, 
		// but it's not mandatory
		public enum AbilityStates 
		{
			Null,
			Idle,
			Falling,
			Walking,
			Running,
			Crouching,
			Crawling, 
			Dashing,
			Jetpacking,
			Jumping,
			Pushing,
			DoubleJumping,
			Attacking,
			FallingDownHole
		}
	}

	public struct AbilityEvent
	{
		public string Name;
		public Action Callback;
	}
}