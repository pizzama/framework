using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SFramework.Actor
{
    public class SFActorController : MonoBehaviour
    {
        [SerializeField] private Vector3 _currentDirection;
        [SerializeField] private bool _isGrounded;
        protected virtual void Awake()
        {
            _currentDirection = transform.forward;
        }
        protected virtual void Update()
        {

        }

        protected virtual void LateUpdate()
        {

        }
        protected virtual void FixedUpdate()
        {

        }
        protected void checkIsGrounded()
        {

        }

        public virtual void Reset()
        {
            _currentDirection = Vector3.zero;
            _isGrounded = true;
        }
    }
}
