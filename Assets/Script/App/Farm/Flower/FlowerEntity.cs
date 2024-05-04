using SFramework;
using SFramework.StateMachine;
using UnityEngine;

namespace App.Farm
{
    public class FlowerEntity : SEntity
    {
        private FlowerFSM _fSM;
        protected override void initEntity()
        {
            base.initEntity();
            _fSM = new FlowerFSM();
            //init data
            _fSM.ChangeState<FlowerEmpty>();
        }

        public void Empty()
        {
            
        }

    }
}
