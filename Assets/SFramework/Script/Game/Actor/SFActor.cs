using SFramework.Tools.Attributes;
using System;
using System.Collections;
using UnityEngine;
using SFramework.StateMachine;

namespace SFramework.Actor
{
    public class SFActor : SFEntity
    {
        public enum SFActorFacingDirections { Default, Left, Right, Up, Down }

        [SerializeField]
        [SFInformation("Actor's direction", SFInformationAttribute.InformationType.Info, false)] 
        private SFActorFacingDirections _direction;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _mainCameraTransform;
        public Transform MainCameraTransform {get {return _mainCameraTransform;} private set {_mainCameraTransform = value;}}

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

            if(_mainCameraTransform == null)
            {
                _mainCameraTransform = Camera.main.transform;
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

        public AnimatorStateInfo GetAnimatorStateInfo()
        {
            AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
            return info;
        }

        public int PlayAnimation(string stateName, Action callback = null)
        {
            var animator = GetComponent<Animator>();

            animator.Update(0f);
            animator.Play(stateName, -1);

            if (callback == null) return 0;

            StartCoroutine(DelayRunEffectCallback(animator, stateName, callback));

            return 0;
        }

        public IEnumerator DelayRunEffectCallback(Animator animator, string stateName, Action callback)
        {
            // 状态机的切换发生在帧的结尾
            yield return new WaitForEndOfFrame();

            var info = animator.GetCurrentAnimatorStateInfo(0);
            if (!info.IsName(stateName)) yield return null;

            yield return new WaitForSeconds(info.length);
            callback();
        }
    }
}
