using SFramework.Tools.Attributes;
using System;
using System.Collections;
using UnityEngine;
using SFramework.StateMachine;
using SFramework;

namespace SFramework.Actor
{
    public class SFSMActor : SEntity
    {
        public enum SFActorFacingDirections {Left, Right, Up, Down }

        [SerializeField]
        [SFInformation("Actor's direction", SFInformationAttribute.InformationType.Info, false)] 
        private SFActorFacingDirections _direction;

        [SerializeField]
        private Transform _mainCameraTransform;
        public Transform MainCameraTransform {get {return _mainCameraTransform;} private set {_mainCameraTransform = value;}}

        protected FSM mFsm;

        public void CreateFSM()
        {
            if (mFsm == null)
            {
                mFsm = new FSM();
                mFsm.Owner = this;
            }
        }

        public FSM GetFSM()
        {
            return mFsm;
        }

        public void AddFSMState(IFSMState state)
        {
            mFsm?.AddState(state);
        }

        public void Update()
        {
            mFsm?.Update();
        }
    }

}
