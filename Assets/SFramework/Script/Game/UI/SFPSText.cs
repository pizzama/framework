using UnityEngine;
using UnityEngine.UI;

namespace SFramework.UI
{
    public class SFPSText : MonoBehaviour
    {
        [SerializeField] Text _framePerSecondText;
        [SerializeField] Text _versionText;
        
        private float _deltaTime;

        private float count = 0f;
        private float num = 1f;

        private void Start()
        {
            if(_versionText != null)
            {
                _versionText.text = Application.version;
            }
        }

        private void LateUpdate()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            count += Time.deltaTime;
            if(count > num)
            {
                count = 0;
                if(_framePerSecondText != null)
                    _framePerSecondText.text = "FPS " + Mathf.Ceil(1f / _deltaTime).ToString();
            }
        }
    }
}