using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SFramework.Actor
{
    public enum SFActorUpdateModes { Update, FixedUpdate }
    public class SFActorController : MonoBehaviour
    {
        [SerializeField] protected Vector3 currentDirection;
        [SerializeField] protected bool isGrounded;
        [SerializeField] protected Vector3 speed;
        [Header("Gravity")]
        [SerializeField] protected float gravity = -30f;
        [SerializeField] protected bool isActiveGravity = true;
        [SerializeField] protected Vector3 currentMovement;
        /// whether or not the character is in free movement mode or not
        [SerializeField] protected bool freeMovement = true;

        protected Vector3 _lastUpdatePosition;
        protected virtual void Awake()
        {
            currentDirection = transform.forward;
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
            computeSpeed();
        }
        protected void checkIsGrounded()
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
            speed = (this.transform.position - _lastUpdatePosition) / Time.deltaTime;
            // we round the speed to 2 decimals
            speed.x = Mathf.Round(speed.x * 100f) / 100f;
            speed.y = Mathf.Round(speed.y * 100f) / 100f;
            speed.z = Mathf.Round(speed.z * 100f) / 100f;
            _lastUpdatePosition = this.transform.position;
        }

        public virtual void Reset()
        {
            currentDirection = Vector3.zero;
            isGrounded = true;
            speed = Vector3.zero;
            isActiveGravity = true;
            currentMovement = Vector3.zero;
        }
    }
}
