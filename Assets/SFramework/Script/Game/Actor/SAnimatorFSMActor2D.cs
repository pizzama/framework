using SFramework.Game.App;
using UnityEngine;

namespace SFramework.Actor
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class SAnimatorFSMActor2D : SAnimatorFSMActor
    {
        [SerializeField] private Rigidbody2D _rigidBody;

        public Rigidbody2D ActorRigidBody { get { return _rigidBody; } }
        [SerializeField] private Collider2D _collider;
        [SerializeField] private SFActorFacingDirections _initialFacingDirection = SFActorFacingDirections.Right; //init facing direction
        [SerializeField] private SFActorFacingDirections _currentFacingDirection = SFActorFacingDirections.Right; //current facing direction
        [SerializeField] bool _modelShouldFlip = false;
        private int _direction;
        public LayerMask GroundLayerMask = LayerManager.GroundLayerMask;
        private Vector3 _orientedMovement;

        protected override void Awake()
        {
            base.Awake();
            _rigidBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        protected override void Update()
        {
            base.Update();
            mVelocity = (_rigidBody.transform.position - mLastUpdatePosition) / Time.deltaTime;
            mAcceleration = (mVelocity - mLastUpdateVelocity) / Time.deltaTime;
        }


        protected override void LateUpdate()
        {
            base.LateUpdate();
            mLastUpdateVelocity = mVelocity;
            computeSpeed();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            applyImpact();
            if (!mFreeMovement)
            {
                return;
            }

            moveUpdate();
        }

        public override void Impact(Vector3 direction, float force)
        {
            direction = direction.normalized;
            mImpact += direction.normalized * force;
        }

        public override void AddForce(Vector3 movement)
        {
            Impact(movement.normalized, movement.magnitude);
        }

        public override void SetMovement(Vector3 movement)
        {
            _orientedMovement = movement;
            _orientedMovement.y = _orientedMovement.z;
            _orientedMovement.z = 0f;
            CurrentMovement = _orientedMovement;
        }

        protected virtual void applyImpact()
        {
            if (mImpact.magnitude > 0.2f)
            {
                _rigidBody.AddForce(mImpact);
            }
            mImpact = Vector3.Lerp(mImpact, Vector3.zero, 5f * Time.deltaTime);
        }

        protected override void determineDirection()
        {
            if (CurrentMovement != Vector3.zero)
            {
                CurrentDirection = CurrentMovement.normalized;
            }
        }

        private void moveUpdate()
        {
            if (Friction > 1)
            {
                CurrentMovement = CurrentMovement / Friction;
            }

            // if we have a low friction (ice, marbles...) we lerp the speed accordingly
            if (Friction > 0 && Friction < 1)
            {
                CurrentMovement = Vector3.Lerp(mSpeed, CurrentMovement, Time.deltaTime * Friction);
            }

            Vector2 newMovement = _rigidBody.position + (Vector2)(CurrentMovement + AddedForce) * Time.fixedDeltaTime;
            _rigidBody.MovePosition(newMovement);
        }

    }
}
