using System.Collections;
using System.Collections.Generic;
using SFramework.Actor;
using UnityEngine;

namespace Game.Character
{
    public class Koala : SAnimatorFSMActor2D
    {
        // Start is called before the first frame update
        protected override void initEntity()
        {
            base.initEntity();

            AddFSMState(new HeroIdleState2D());
            AddFSMState(new HeroMoveState2D());
            GetFSM().ChangeState<HeroIdleState2D>();
        }
    }
}
