using SFramework.Tools.Attributes;
using UnityEngine;

namespace SFramework.CameraUtils
{
    public class FacingCameraAlways : MonoBehaviour
    {
        private Transform[] _childs;
        [SFReadOnly][SerializeField] private int _totalChildren;
        [SFReadOnly][SerializeField] private Quaternion _rotation;
        private void Start()
        {
            Collect();
        }

        private void Update()
        {
            for (int i = 0; i < _childs.Length; i++)
            {
                _rotation = Camera.main.transform.rotation;
                _childs[i].rotation = _rotation;
                _totalChildren = _childs.Length;
            }
        }

        public void Collect()
        {
            _childs = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                _childs[i] = transform.GetChild(i);
            }

            _totalChildren = _childs.Length;
        }
    }
}
