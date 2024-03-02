using System.Collections;
using System.Collections.Generic;
using SFramework.Actor;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Character
{
    public class Koala : SAnimatorFSMActor2D
    {
        // Start is called before the first frame update
        protected override void InitEntity()
        {
            base.InitEntity();

            AddFSMState(new HeroIdleState2D());
            GetFSM().ChangeState<HeroIdleState2D>();
        }
    }
}
