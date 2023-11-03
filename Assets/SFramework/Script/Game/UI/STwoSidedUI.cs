using UnityEngine;
using UnityEngine.Events;

namespace SFramework.UI
{
    [ExecuteAlways]
    public class STwoSidedUI : MonoBehaviour
    {
        public enum Axis { x, y, z }

        [Header("Bindings")]
        public GameObject Front;// the object to consider as the "front" of the two sided element. Will be visible if the scale is above the threshold
        public GameObject Back;// the object to consider as the "back" of the two sided element. Will be visible if the scale is below the threshold
        [Header("Axis")]
        public Axis FlipAxis;
        public float ScaleThreshold = 0f;// the scale threshold at which the flip should occur
        [Header("Events")]
        public UnityEvent OnFlip;
        [Header("Debug")]
        public bool DebugMode;// whether or not we're in debug mode
        [Range(-1f, 1f)]
        public float ScaleValue; // the value to apply to the scale when in debug mode
        public bool BackVisible = false;
        protected RectTransform _rectTransform;
        protected bool _initialized = false;

        void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            _rectTransform = this.gameObject.GetComponent<RectTransform>();
            _initialized = true;

            float axis = GetScaleValue();
            BackVisible = (axis < ScaleThreshold);

            Front.SetActive(!BackVisible);
            Back.SetActive(BackVisible);
        }

        // Update is called once per frame
        protected virtual void Update()
        {
#if UNITY_EDITOR
            IfEditor();
#endif

            float axis = GetScaleValue();

            if ((axis < ScaleThreshold) != BackVisible)
            {
                Front.SetActive(BackVisible);
                Back.SetActive(!BackVisible);
                OnFlip?.Invoke();
            }
            BackVisible = (axis < ScaleThreshold);
        }

        protected virtual void IfEditor()
        {
            if (!_initialized)
            {
                Initialization();
            }

            if (DebugMode)
            {
                switch (FlipAxis)
                {
                    case Axis.x:
                        _rectTransform.localScale = new Vector3(ScaleValue, _rectTransform.localScale.y, _rectTransform.localScale.z);
                        break;
                    case Axis.y:
                        _rectTransform.localScale = new Vector3(_rectTransform.localScale.x, ScaleValue, _rectTransform.localScale.z);
                        break;
                    case Axis.z:
                        _rectTransform.localScale = new Vector3(_rectTransform.localScale.x, _rectTransform.localScale.y, ScaleValue);
                        break;
                }
            }
        }
        protected virtual float GetScaleValue()
        {
            switch (FlipAxis)
            {
                case Axis.x:
                    return _rectTransform.localScale.x;
                case Axis.y:
                    return _rectTransform.localScale.y;
                case Axis.z:
                    return _rectTransform.localScale.z;
            }

            return 0f;
        }
    }
}
