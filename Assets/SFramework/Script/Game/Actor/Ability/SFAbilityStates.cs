using UnityEngine;
using System.Collections;
using System;

namespace SFramework.Actor.Ability
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

    public enum AbilityAction
    {
        Jump,
    }
}