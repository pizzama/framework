using SFramework;
using UnityEngine;

namespace SFramework.Game.App
{
    public class SFInputManager : SSingleton<SFInputManager>
    {
        //control the system under the same rules.when using old or new input system
        protected const string axisHorizontal = "Horizontal";
        protected const string axisVertical = "Vertical";

        public Vector2 PrimaryMovement { get { return _primaryMovement; } }
        private Vector2 _primaryMovement = Vector2.zero;
        public bool SmoothMovement = true;
        [Tooltip("set this to false to prevent the InputManager from reading input")]
        public bool InputDetectionActive = true;

        public virtual void SetMovement()
        {
            if (InputDetectionActive)
            {
                if (SmoothMovement)
                {
                    _primaryMovement.x = Input.GetAxis(axisHorizontal);
                    _primaryMovement.y = Input.GetAxis(axisVertical);
                }
                else
                {
                    _primaryMovement.x = Input.GetAxisRaw(axisHorizontal);
                    _primaryMovement.y = Input.GetAxisRaw(axisVertical);
                }
            }
        }
        public virtual void SetMovement(Vector2 movement)
        {
            if (InputDetectionActive)
            {
                _primaryMovement.x = movement.x;
                _primaryMovement.y = movement.y;
            }
        }

        public bool IsMove()
        {
            return _primaryMovement != Vector2.zero;
        }    

        // Update is called once per frame
        void Update()
        {
            SetMovement();
        }
    }
}
