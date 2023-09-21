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
            _fsm.AddState(state);
        }

        public void Update()
        {
            _fsm.Update();
        }
    }
}
