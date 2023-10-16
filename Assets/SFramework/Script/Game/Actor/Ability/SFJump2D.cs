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
		protected override void init()
		{
		}

	}
}