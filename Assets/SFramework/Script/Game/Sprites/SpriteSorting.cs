using System;
using UnityEngine;

namespace SFramework.Sprites
{
    // [ExecuteAlways]
    public class SpriteSorting : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private int _offset;

        private void OnEnable()
        {
            if(_cameraTransform == null)
                _cameraTransform = Camera.main.transform;
        }
        private void LateUpdate()
        {
            if (TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                float sort = Mathf.Abs(_cameraTransform.position.z - transform.position.z) * 16;
                spriteRenderer.sortingOrder = Mathf.CeilToInt(-sort + _offset);
            }
        }
    }
}
