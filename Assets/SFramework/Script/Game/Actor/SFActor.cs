using SFramework.Tools.Attributes;
using UnityEngine;

namespace SFramework.Actor
{
    public class SFActor : MonoBehaviour
    {
        public enum SFActorFacingDirections { Default, Left, Right, Up, Down }

        [SerializeField]
        [SFInformation("Actor's direction", SFInformationAttribute.InformationType.Info, false)] 
        private SFActorFacingDirections _direction;
        [SerializeField]
        private Animator _animator;
        protected virtual void Awake()
        {
            init();
        }

        protected virtual void init()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
        }
    }
}
