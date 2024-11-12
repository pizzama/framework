using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Actor
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class SAnimatorFSMActor3D : SAnimatorFSMActor
    {
        public Rigidbody ActorRigidBody { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            ActorRigidBody = GetComponent<Rigidbody>();
        }
    }
}
