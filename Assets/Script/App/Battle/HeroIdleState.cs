using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using Game.Character;

public class HeroIdleState : FSMState
{
    public override string ToName()
    {
        return "HeroIdleState";
    }
    public override void EnterState()
    {
        base.EnterState();
        Hero hero = (Hero)Machine.BlackBoard;
        var animator = hero.GetComponent<Animator>();
        Debug.Log(animator);
    }
}
