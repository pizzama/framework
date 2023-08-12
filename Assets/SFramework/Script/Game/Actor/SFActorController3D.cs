using UnityEngine;
using UnityEngine.UIElements;

namespace SFramework.Actor
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(CharacterController))]
    public class SFActorController3D : SFActorController
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] protected Rigidbody _rigidBody;
        [SerializeField] protected Collider _collider;
        [SerializeField] private SFActorUpdateModes _updateMode;

        [Header("Steep Surfaces")]
        /// whether or not the character should slide while standing on steep surfaces
        public bool SlideOnSteepSurfaces = true;
        /// the speed at which the character should slide
        public float SlidingSpeed = 15f;
        /// the control the player has on the speed while sliding down
        public float SlidingSpeedControl = 0.4f;
        /// the control the player has on the direction while sliding down
        public float SlidingDirectionControl = 1f;

        private Vector3 _groundNormal = Vector3.zero;
        private Vector3 _lastGroundNormal = Vector3.zero;
        private Vector3 _inputMoveDirection;

        // char movement
        private CollisionFlags _collisionFlags;
        private Vector3 _frameVelocity = Vector3.zero;
        private Vector3 _hitPoint = Vector3.zero;
        private Vector3 _lastHitPoint = new Vector3(Mathf.Infinity, 0, 0);

        // velocity
        private Vector3 _newVelocity;
        private Vector3 _lastHorizontalVelocity;
        private Vector3 _newHorizontalVelocity;
        private Vector3 _motion;
        private Vector3 _idealVelocity;
        private Vector3 _idealDirection;
        private Vector3 _idealLocalDirection;
        private Vector3 _horizontalVelocityDelta;
        private float _stickyOffset;

        protected override void Awake()
        {
            base.Awake();
            _characterController = GetComponent<CharacterController>();
            _rigidBody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        protected override void Update()
        {
            base.Update();
            if (_updateMode == SFActorUpdateModes.Update)
            {
                processUpdate();
            }
        }

        //计算新的位移，并移动角色
        protected virtual void processUpdate()
        {
            if (transform == null)
            {
                return;
            }

            if (!freeMovement)
            {
                return;
            }

            addInput();
            moveCharacterController();
        }

        public bool TooSteep() { return (_groundNormal.y <= Mathf.Cos(_characterController.slopeLimit * Mathf.Deg2Rad)); }

        public override void SetMovement(Vector3 movement)
        {
            currentMovement = movement;

            Vector3 directionVector;
            directionVector = movement;
            if (directionVector != Vector3.zero)
            {
                float directionLength = directionVector.magnitude;
                directionVector = directionVector / directionLength;
                directionLength = Mathf.Min(1, directionLength);
                directionLength = directionLength * directionLength;
                directionVector = directionVector * directionLength;
            }
            _inputMoveDirection = transform.rotation * directionVector;
        }

        protected virtual void addInput()
        {
            if (isGrounded && TooSteep())
            {
                _idealVelocity.x = _groundNormal.x;
                _idealVelocity.y = 0;
                _idealVelocity.z = _groundNormal.z;
                _idealVelocity = _idealVelocity.normalized;
                _idealDirection = Vector3.Project(_inputMoveDirection, _idealVelocity);
                _idealVelocity = _idealVelocity + (_idealDirection * SlidingSpeedControl) + (_inputMoveDirection - _idealDirection) * SlidingDirectionControl;
                _idealVelocity = _idealVelocity * SlidingSpeed;
            }
            else
            {
                _idealVelocity = currentMovement;
            }

            if (isGrounded)
            {
                Vector3 sideways = Vector3.Cross(Vector3.up, _idealVelocity);
                _idealVelocity = Vector3.Cross(sideways, _groundNormal).normalized * _idealVelocity.magnitude;
            }

            _newVelocity = _idealVelocity;
            _newVelocity.y = isGrounded ? Mathf.Min(_newVelocity.y, 0) : _newVelocity.y;
        }

        protected virtual void ComputeVelocityDelta()
        {
            _motion = _newVelocity * Time.deltaTime;
            _horizontalVelocityDelta.x = _motion.x;
            _horizontalVelocityDelta.y = 0f;
            _horizontalVelocityDelta.z = _motion.z;
            _stickyOffset = Mathf.Max(_characterController.stepOffset, _horizontalVelocityDelta.magnitude);
            if (isGrounded)
            {
                _motion -= _stickyOffset * Vector3.up;
            }
        }

        protected virtual void addGravity()
        {

        }

        protected virtual void moveCharacterController()
        {
            _groundNormal = Vector3.zero;

            _collisionFlags = _characterController.Move(_motion); // controller move

            _lastHitPoint = _hitPoint;
            _lastGroundNormal = _groundNormal;
        }
    }
}
