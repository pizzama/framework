using UnityEngine;
using System.Collections;
using System;

namespace SFramework.Actor
{
    public enum ActorConditions
    {
        Normal,
        ControlledMovement,
        Frozen,
        Paused,
        Dead,
        Stunned
    }

    public enum ActorStates
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

    public enum ActorAction
    {
        Move,
        Jump,
    }
}