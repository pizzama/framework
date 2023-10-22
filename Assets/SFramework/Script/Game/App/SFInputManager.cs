using SFramework.StateMachine;
using System.Collections.Generic;
using UnityEngine;


namespace SFramework.Game.App
{
    /// <summary>
    /// button, short for InputManager button, a class used to handle button states, whether mobile or actual keys
    /// </summary>
    public class SFInputButton
    {
        public enum ButtonStates { Off, ButtonDown, ButtonPressed, ButtonUp }
        /// a state machine used to store button states
        public SimpleStateMachine<ButtonStates> State { get; protected set; }
        /// the unique ID of this button
        public string ButtonID;

        public delegate void ButtonDownMethodDelegate();
        public delegate void ButtonPressedMethodDelegate();
        public delegate void ButtonUpMethodDelegate();

        public ButtonDownMethodDelegate ButtonDownMethod;
        public ButtonPressedMethodDelegate ButtonPressedMethod;
        public ButtonUpMethodDelegate ButtonUpMethod;

        /// returns the time (in unscaled seconds) since the last time the button was pressed down
        public float TimeSinceLastButtonDown { get { return Time.unscaledTime - _lastButtonDownAt; } }
        /// returns the time (in unscaled seconds) since the last time the button was released
        public float TimeSinceLastButtonUp { get { return Time.unscaledTime - _lastButtonUpAt; } }
        /// returns true if this button was pressed down within the time (in unscaled seconds) passed in parameters
        public bool ButtonDownRecently(float time) { return (TimeSinceLastButtonDown <= time); }
        /// returns true if this button was released within the time (in unscaled seconds) passed in parameters
        public bool ButtonUpRecently(float time) { return (TimeSinceLastButtonUp <= time); }

        protected float _lastButtonDownAt;
        protected float _lastButtonUpAt;


        //the buttonID is same as unity settings inputManager tab
        public SFInputButton(string buttonID, string playerID = "", ButtonDownMethodDelegate btnDown = null, ButtonPressedMethodDelegate btnPressed = null, ButtonUpMethodDelegate btnUp = null)
        {
            ButtonID = buttonID + "_" + playerID;
            ButtonDownMethod = btnDown;
            ButtonUpMethod = btnUp;
            ButtonPressedMethod = btnPressed;
            State = new SimpleStateMachine<ButtonStates>(null, false);
            State.ChangeState(ButtonStates.Off);
        }

        public virtual void TriggerButtonDown()
        {
            _lastButtonDownAt = Time.unscaledTime;
            if (ButtonDownMethod == null)
            {
                State.ChangeState(ButtonStates.ButtonDown);
            }
            else
            {
                ButtonDownMethod();
            }
        }

        public virtual void TriggerButtonPressed()
        {
            if (ButtonPressedMethod == null)
            {
                State.ChangeState(ButtonStates.ButtonPressed);
            }
            else
            {
                ButtonPressedMethod();
            }
        }

        public virtual void TriggerButtonUp()
        {
            _lastButtonUpAt = Time.unscaledTime;
            if (ButtonUpMethod == null)
            {
                State.ChangeState(ButtonStates.ButtonUp);
            }
            else
            {
                ButtonUpMethod();
            }
        }
    }
    public class SFInputManager : SSingleton<SFInputManager>
    {
        //control the system under the same rules.when using old or new input system
        protected const string axisHorizontal = "Horizontal";
        protected const string axisVertical = "Vertical";
        public bool IsMobile { get; protected set; }
        public Vector2 PrimaryMovement { get { return _primaryMovement; } }
        private Vector2 _primaryMovement = Vector2.zero;
        public bool SmoothMovement = true;
        [Tooltip("set this to false to prevent the InputManager from reading input")]
        public bool InputDetectionActive = true;
        private List<SFInputButton> _buttonList;
        public SFInputButton JumpButton { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
            InitializeButtons();
        }

        private void Start()
        {
            ControlsModeDetection();
        }

        private void ControlsModeDetection()
        {
            IsMobile = false;
#if UNITY_ANDROID || UNITY_IPHONE
			IsMobile = true;
#endif
        }


        public virtual void SetMovement()
        {
            if (!IsMobile && InputDetectionActive)
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
        private void Update()
        {
            if(!IsMobile)
            {
                SetMovement();
                GetInputButtons();
            }

        }

        public virtual void JumpButtonDown() { JumpButton.State.ChangeState(SFInputButton.ButtonStates.ButtonDown); }
        public virtual void JumpButtonPressed() { JumpButton.State.ChangeState(SFInputButton.ButtonStates.ButtonPressed); }
        public virtual void JumpButtonUp() { JumpButton.State.ChangeState(SFInputButton.ButtonStates.ButtonUp); }

        protected virtual void InitializeButtons()
        {
            _buttonList = new List<SFInputButton>();
            _buttonList.Add(JumpButton = new SFInputButton("Jump", "", JumpButtonDown, JumpButtonPressed, JumpButtonUp));
        }

        protected virtual void GetInputButtons()
        {
            //foreach (SFInputButton button in _buttonList)
            //{
            //    if (Input.GetButton(button.ButtonID))
            //    {
            //        button.TriggerButtonPressed();
            //    }
            //    if (Input.GetButtonDown(button.ButtonID))
            //    {
            //        button.TriggerButtonDown();
            //    }
            //    if (Input.GetButtonUp(button.ButtonID))
            //    {
            //        button.TriggerButtonUp();
            //    }
            //}
        }
    }
}
