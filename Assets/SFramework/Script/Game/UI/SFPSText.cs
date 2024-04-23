using UnityEngine;
using UnityEngine.UI;

namespace SFramework.UI
{
    public class SFPSText : MonoBehaviour
    {
        [SerializeField] Text _framePerSecondText;
        
        private float _deltaTime;

        void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            _framePerSecondText.text = "FPS " + Mathf.Ceil(1f / _deltaTime).ToString();
        }
    }
}