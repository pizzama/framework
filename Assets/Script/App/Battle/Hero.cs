using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using SFramework.Actor;

namespace Game.Character
{
    public class Hero : SFActor3D
    {
        private void Start()
        {
            AddFSMState(new HeroIdleState());
            AddFSMState(new HeroMoveState());
            GetFSM().ChangeState<HeroIdleState>();
        }
    }
}
