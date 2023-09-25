using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using SFramework.Game.Actor;

namespace Game.Character
{
    public class Hero : SFActor
    {
        private void Start()
        {
            AddFSMState(new HeroIdleState());
        }
    }
}
