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

        private Vector3 _inputMoveDirection;
        private Vector3 _newVelocity;
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
        }

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

        }

        protected virtual void addGravity()
        {

        }
    }
}
