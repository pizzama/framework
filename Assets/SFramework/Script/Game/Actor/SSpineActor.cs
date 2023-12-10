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

        
        private FSM _fsm;

        public FSM GetFSM()
        {
            return _fsm;
        }

        public SkeletonAnimation GetSkeletonAnimation()
        {
            return _skeletonanimation;
        }

        protected override void InitEntity()
        {

            if (_skeletonanimation == null)
            {
                _skeletonanimation = FindObjectOfType<SkeletonAnimation>();
            }

            if (_fsm == null)
            {
                _fsm = new FSM();
                _fsm.BlackBoard = this;
            }
        }

        public void AddFSMState(IFSMState state)
        {
            _fsm?.AddState(state);
        }

        public void Update()
        {
            _fsm?.Update();
        }
    }
}
#endif
