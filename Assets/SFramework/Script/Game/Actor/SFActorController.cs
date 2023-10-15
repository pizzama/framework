using SFramework.Tools.Attributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace SFramework.Actor
{
    public class SFActorController : MonoBehaviour
    {
        [SFReadOnly][SerializeField] protected bool isGrounded;
        [SerializeField] protected Vector3 speed;
        [Header("Gravity")]
        [SerializeField] protected float gravity = -30f;
        [SerializeField] protected bool isActiveGravity = true;
        [SerializeField] protected Vector3 velocity;
        [SerializeField] protected Vector3 acceleration;
        [SerializeField] protected bool freeMovement = true; /// whether or not the character is in free movement mode or not
        public Vector3 CurrentMovement {get; set;}
        public Vector3 CurrentDirection {get; set;}
        public bool Grounded { get; private set; } // whether or not the character is grounded
        public bool JustGotGrounded { get; private set; } // whether or not the character got grounded this frame
        public float Friction;
        public Vector3 AddedForce;
        protected Vector3 lastUpdateVelocity; // last frame velocity
        protected Vector3 lastUpdatePosition; // last frame position
        protected Vector3 impact;
        private bool _groundedLastFrame;
        protected virtual void Awake()
        {
            CurrentDirection = transform.forward;
        }
        protected virtual void Update()
        {
            checkIsGrounded();
            handleFriction();
            determineDirection();
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

        // Use this to apply an impact to a controller, moving it in the specified direction at the specified force
        public virtual void Impact(Vector3 direction, float force) 
		{

		}
        // Adds the specified force to the controller
        public virtual void AddForce(Vector3 movement)
		{

		}

        protected virtual void handleFriction()
        {

        }
        protected virtual void determineDirection()
        {

        }
        public virtual void SetMovement(Vector3 movement)
        {

        }
        protected virtual void computeSpeed()
        {
            if (Time.deltaTime != 0f)
            {
                speed = (this.transform.position - lastUpdatePosition) / Time.deltaTime;
            }
            // we round the speed to 2 decimals
            speed.x = Mathf.Round(speed.x * 100f) / 100f;
            speed.y = Mathf.Round(speed.y * 100f) / 100f;
            speed.z = Mathf.Round(speed.z * 100f) / 100f;
            lastUpdatePosition = this.transform.position;
        }

        public virtual void Reset()
        {
			impact = Vector3.zero;
			speed = Vector3.zero;
			velocity = Vector3.zero;
			lastUpdateVelocity = Vector3.zero;
			acceleration = Vector3.zero;
			Grounded = true;
			JustGotGrounded = false;
			CurrentMovement = Vector3.zero;
			CurrentDirection = Vector3.zero;
			AddedForce = Vector3.zero;
        }
    }
}
