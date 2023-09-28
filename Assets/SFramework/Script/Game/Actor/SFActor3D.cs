using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game.Actor
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class SFActor3D : SFActor
    {
        public Rigidbody ActorRigidBody { get; private set; }
        protected override void init()
        {
            base.init();
            ActorRigidBody = GetComponent<Rigidbody>();
        }
    }
}
