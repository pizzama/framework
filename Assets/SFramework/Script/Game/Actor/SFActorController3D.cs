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
        protected override void Awake()
        {
            base.Awake();
            _characterController = GetComponent<CharacterController>();
            _rigidBody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }
    }
}
