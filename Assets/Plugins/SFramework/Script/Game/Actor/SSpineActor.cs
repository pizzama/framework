#if SPINE_ANIMATION
using SFramework.Tools.Attributes;
using SFramework.StateMachine;
using UnityEngine;
using Spine;
using Spine.Unity;

namespace SFramework.SActor
{
    public class SSpineActor : SEntity
    {
        public enum SActorFacingDirections { Default, Left, Right, Up, Down }

        [SerializeField]
        [SFInformation("Actor's direction", SFInformationAttribute.InformationType.Info, false)] 
        private SActorFacingDirections _direction;

        [SerializeField]
        private SkeletonAnimation _skeletonanimation;

        
        private SFSM _fsm;

        public SFSM GetFSM()
        {
            return _fsm;
        }

        public SkeletonAnimation GetSkeletonAnimation()
        {
            return _skeletonanimation;
        }

        // protected override void initEntity()
        // {

        //     if (_skeletonanimation == null)
        //     {
        //         _skeletonanimation = FindObjectOfType<SkeletonAnimation>();
        //     }

        //     if (_fsm == null)
        //     {
        //         _fsm = new SFSM();
        //         _fsm.BlackBoard = this;
        //     }
        // }

        // public void AddFSMState(IFSMState state)
        // {
        //     _fsm?.AddState(state);
        // }

        public void Update()
        {
            _fsm?.Update();
        }

        public override void Recycle()
        {
            throw new System.NotImplementedException();
        }

        public override void Show()
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif
