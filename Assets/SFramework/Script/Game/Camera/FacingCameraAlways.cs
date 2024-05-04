using System.Collections.Generic;
using SFramework.Tools.Attributes;
using UnityEngine;

namespace SFramework.CameraUtils
{
    public class FacingCameraAlways : MonoBehaviour
    {
        private List<Transform> _childs;

        [SFReadOnly]
        [SerializeField]
        private int _totalChildren;

        [SerializeField]
        private bool _useExportFlag;

        [SFReadOnly]
        [SerializeField]
        private Quaternion _rotation;

        [SerializeField]
        private Camera _camera;

        private void Start()
        {
            Collect();
        }

        private void Update()
        {
            if (_camera != null)
            {
                for (int i = 0; i < _childs.Count; i++)
                {
                    _rotation = _camera.transform.rotation;
                    _childs[i].rotation = _rotation;
                }
            }
        }

        public void Collect()
        {
            _childs = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (_useExportFlag)
                {
                    if (child.tag == SEnum.ExportTag)
                    {
                        _childs.Add(child);
                    }
                }
                else
                {
                    _childs.Add(child);
                }

            }

            _totalChildren = _childs.Count;
            if (_camera == null)
                _camera = Camera.main;
        }
    }
}
