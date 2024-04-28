using SFramework;
using SFramework.StateMachine;
using UnityEngine;

namespace App.Farm
{
    public class FlowerEntity : SEntity
    {
        private SFSM _fSM;
        protected override void initEntity()
        {
            base.initEntity();
            _fSM = new SFSM();
            //init data
        }
    }
}
