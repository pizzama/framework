using SFramework.Tools.Attributes;
using UnityEngine;

namespace SFramework.Game.Actor
{
    public class SFActor : SFEntity
    {
        public enum SFActorFacingDirections { Default, Left, Right, Up, Down }

        [SerializeField]
        [SFInformation("Actor's direction", SFInformationAttribute.InformationType.Info, false)] 
        private SFActorFacingDirections _direction;
        [SerializeField]
        private Animator _animator;

        private FSM _fsm;

        public FSM GetFSM()
        {
            return _fsm;
        }

        protected virtual void Awake()
        {
            init();
        }

        protected virtual void init()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
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

        private void ForceCrossFade(Animator animator, string name, float transitionDuration, int layer = 0, float normalizedTime = float.NegativeInfinity, bool changeImmediately = false)
        {
            if (changeImmediately)
            {
                animator.Play(animator.GetNextAnimatorStateInfo(layer).fullPathHash, layer);
                animator.Update(0);
                animator.CrossFade(name, transitionDuration, layer, normalizedTime);
                return;
            }
            animator.Update(0);
            if (animator.GetNextAnimatorStateInfo(layer).fullPathHash == 0)
            {
                animator.CrossFade(name, transitionDuration, layer, normalizedTime);
            }
            else
            {
                animator.Play(animator.GetNextAnimatorStateInfo(layer).fullPathHash, layer);
                animator.Update(0);
                animator.CrossFade(name, transitionDuration, layer, normalizedTime);
            }
        }

        public AnimatorStateInfo GetAnimatorStateInfo
        {
            get
            {
                AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
                return info;
            }
        }
    }
}
