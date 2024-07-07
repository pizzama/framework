using UnityEngine;
using SFramework.Tools.Attributes;

namespace SFramework.Game.Animation
{
    /*
    * control the transform moving as phase method
    * you could change image pivot
    */
    public class TransformAnimation: MonoBehaviour
    {
        [Header("Vertical Shrink")]
        [SFInformation("Velocity and Phase only calculate Phase as result", SFInformationAttribute.InformationType.Info, false)] 
        [SerializeField] private float _verticalShrinkVelocity = 0;
        [SerializeField] private float _verticalShrinkPhase = 0;
        [SerializeField] private float _verticalShrink = 0;
        [Header("Horizontal Shrink")]
        [SerializeField] private float _horizontalShrinkVelocity = 0;
        [SerializeField] private float _horizontalShrinkPhase = 0;
        [SerializeField] private float _horizontalShrink = 0;

        [Header("Vertical Levitation")]
        [SerializeField] private float _verticalLevitationVelocity = 0;
        [SerializeField] private float _verticalLevitationPhase = 0;
        [SerializeField] private float _verticalLevitation = 0;
        [Header("Horizontal Rotate")]
        [SerializeField] private float _horizontalRotateVelocity = 0;
        [SerializeField] private float _horizontalRotatePhase = 0;
        [SerializeField] private float _horizontalRotate = 0; 

        [Header("Normal Property")]
        [SerializeField] private float _randomPhase = 0; 

        [SerializeField] private bool _right = true;

        private Vector3 _baseLocalPosition;
        private float _baseScaleX;
        private float _baseScaleY;

        private void Awake()
        {
            if (transform.localScale.x == -1)
                _right = false;
            _baseScaleX = Mathf.Abs(transform.localScale.x);
            _baseScaleY = transform.localScale.y;
            _baseLocalPosition = transform.localPosition;
            if(_randomPhase == 0)
                _randomPhase = Random.Range(0, 360f);
        }

        public void SetVerticalShrink(float value, float velocity, float phase)
        {
            _verticalShrink = value;
            _verticalShrinkVelocity = velocity;
            _verticalShrinkPhase = phase;
        }

        public void CleanVerticalShrink()
        {
            _verticalShrink = 0;
            _verticalShrinkVelocity = 0;
            _verticalShrinkPhase = 0;
        }

        public void SetHorizontalShrink(float value, float velocity, float phase)
        {
            _horizontalShrink = value;
            _horizontalShrinkVelocity = velocity;
            _horizontalShrinkPhase = phase;
        }

        public void CleanHorizontalShrink()
        {
            _horizontalShrink = 0;
            _horizontalShrinkVelocity = 0;
            _horizontalShrinkPhase = 0;
        }

        public void SetLevitation(float value, float velocity, float phase)
        {
            _verticalLevitation = value;
            _verticalLevitationVelocity = velocity;
            _verticalLevitationPhase = phase;
        }

        public void CleanLevitation()
        {
            _verticalLevitation = 0;
            _verticalLevitationVelocity = 0;
            _verticalLevitationPhase = 0;            
        }

        public void SetRotate(float value, float velocity, float phase)
        {
            _horizontalRotate = value;
            _horizontalRotateVelocity = velocity;
            _horizontalRotatePhase = phase;
        }

        public void CleanRotate()
        {
            _horizontalRotate = 0;
            _horizontalRotateVelocity = 0;
            _horizontalRotatePhase = 0;            
        }

        private float getValue(float velocity, float phase)
        {
            float angle = velocity * Time.fixedTime;
            float v = -Mathf.Cos((angle + phase + _randomPhase) * Mathf.Deg2Rad);
            return v;
        } 

        private void FixedUpdate()
        {
            float v = 0;

            //shrink x axis
            float vvh = 1;
            float vvs = 1;
            if (_verticalShrink != 0 || _horizontalShrink != 0)
            {
                v = (getValue(_verticalShrinkVelocity, _verticalShrinkPhase) + 1) / 2; //keep v in range 0,1
                vvs = v * _verticalShrink + _baseScaleY;

                v = (getValue(_horizontalShrinkVelocity, _horizontalShrinkPhase) + 1) / 2; //keep v in range 0,1
                vvh = v * _horizontalShrink + _baseScaleX;
            }

            vvh = _right ? vvh : -vvh;
            transform.localScale = new Vector3(vvh, vvs, 1);

            //levitation y axis
            if(_verticalLevitation != 0)
            {
                v = getValue(_verticalLevitationVelocity, _verticalLevitationPhase); //keep v in range -1,-1
                float vvf = v * _verticalLevitation;
                transform.localPosition = new Vector3(_baseLocalPosition.x, _baseLocalPosition.y + vvf, _baseLocalPosition.z);
            }

            //rotate z axis
            if(_horizontalRotate != 0)
            {
                v = getValue(_horizontalRotateVelocity, _horizontalRotatePhase); //keep v in range -1,-1
                float rrz = v * _horizontalRotate;
                transform.localRotation = Quaternion.Euler(0, 0, rrz);
            }
        }
    }
}
