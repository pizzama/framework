using SFramework.Tools.Attributes;
using UnityEngine;
using SFramework.StateMachine;
using System.Collections.Generic;
using SFramework.Game;

namespace SFramework.Actor
{
    public class SFSMActor : RootEntity
    {
        [SerializeField]
        [SFInformation("Actor's direction", SFInformationAttribute.InformationType.Info, false)]
        private SFActorFacingDirections _direction;

        [SerializeField] protected Transform mActorModel; //the parent of the actor model

        [SerializeField] protected Transform mMainCameraTransform;
        public Transform MainCameraTransform { get { return mMainCameraTransform; } private set { mMainCameraTransform = value; } }

        [SerializeField] private string _currentStateName;

        [SerializeField] private List<string> _stateNames;

        [SFReadOnly][SerializeField] protected bool mIsGrounded;
        [SerializeField] protected Vector3 mSpeed;
        [Header("Gravity")]
        [SerializeField] protected float mGravity = -30f;
        [SerializeField] protected bool mIsActiveGravity = true;
        [SerializeField] protected Vector3 mVelocity;
        [SerializeField] protected Vector3 mAcceleration;

        [SerializeField] protected bool mFreeMovement = true; /// whether or not the character is in free movement mode or not
        public Vector3 CurrentMovement { get; set; }
        public Vector3 CurrentDirection { get; set; }
        public bool Grounded { get; private set; } // whether or not the character is grounded
        public bool JustGotGrounded { get; private set; } // whether or not the character got grounded this frame
        public float Friction;
        public Vector3 AddedForce;
        protected Vector3 mLastUpdateVelocity; // last frame velocity
        protected Vector3 mLastUpdatePosition; // last frame position
        protected Vector3 mImpact;
        private bool _groundedLastFrame;


        protected SFSM mFsm;

        public void CreateFSM()
        {
            if (mFsm == null)
            {
                mFsm = new SFSM(this);
            }
        }

        public SFSM GetFSM()
        {
            return mFsm;
        }

        public void AddFSMState(ISFSMState state)
        {
            mFsm?.AddState(state);
            _stateNames.Clear();
            List<ISFSMState> states = mFsm.GetFSMStates();
            foreach (var tempState in states)
            {
                _stateNames.Add(tempState.ToName());
            }
        }

        protected virtual void Update()
        {
            if(!Checked())
            {
                return;
            }
            mFsm?.Update();
            _currentStateName = mFsm?.CurrentState?.ToName();
            checkIsGrounded();
            handleFriction();
            determineDirection(); //collect direction info;
        }

        protected virtual void LateUpdate()
        {
        }
        protected virtual void FixedUpdate()
        {
        }

        protected void checkIsGrounded()
        {
            JustGotGrounded = (!_groundedLastFrame && Grounded);
            _groundedLastFrame = Grounded;
        }

        protected virtual void handleFriction()
        {

        }
        protected virtual void determineDirection()
        {

        }
        
        public virtual void Impact(Vector3 direction, float force) // Use this to apply an impact to a controller, moving it in the specified direction at the specified force
        {

        }
        public virtual void AddForce(Vector3 movement) // Adds the specified force to the controller
        {

        }


        public virtual void SetMovement(Vector3 movement) // Sets the current movement of the controller to the specified Vector3
        {

        }

        public virtual void MovePosition(Vector3 newPosition) // Moves the controller to the specified position (in world space)
        {

        }

        protected virtual void computeSpeed()
        {
            if (Time.deltaTime != 0f)
            {
                mSpeed = (this.transform.position - mLastUpdatePosition) / Time.deltaTime;
            }
            // we round the speed to 2 decimals
            mSpeed.x = Mathf.Round(mSpeed.x * 100f) / 100f;
            mSpeed.y = Mathf.Round(mSpeed.y * 100f) / 100f;
            mSpeed.z = Mathf.Round(mSpeed.z * 100f) / 100f;
            mLastUpdatePosition = this.transform.position;
        }

        public virtual void Reset()
        {
            mImpact = Vector3.zero;
            mSpeed = Vector3.zero;
            mVelocity = Vector3.zero;
            mLastUpdateVelocity = Vector3.zero;
            mAcceleration = Vector3.zero;
            Grounded = true;
            JustGotGrounded = false;
            CurrentMovement = Vector3.zero;
            CurrentDirection = Vector3.zero;
            AddedForce = Vector3.zero;
        }
    }

}
