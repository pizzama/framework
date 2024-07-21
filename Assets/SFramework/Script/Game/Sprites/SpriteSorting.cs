using System;
using System.Collections.Generic;
using SFramework.Extension;
using UnityEngine;
using static SEnum;

namespace SFramework.Sprites
{
    // [ExecuteAlways]
    public class SpriteSorting : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private int _offset;
        [SerializeField] private Vector3 _worldPosition;

        [SerializeField] private SAxis _sortAxis = SAxis.Y;

        private List<SpriteRenderer> _sprites;

        private void OnEnable()
        {
            if (_cameraTransform == null)
                _cameraTransform = Camera.main.transform;

            _sprites = this.transform.CollectAllComponent<SpriteRenderer>();
        }
        private void LateUpdate()
        {
            Vector3 pos = transform.position;
            if (pos != _worldPosition)
            {
                if (_sprites.Count > 0)
                {
                    for (int i = 0; i < _sprites.Count; i++)
                    {
                        SpriteRenderer spriteRenderer = _sprites[i];
                        float sort;
                        if (_sortAxis == SAxis.X)
                            sort = Mathf.Abs(_cameraTransform.position.x - transform.position.x) * 16;
                        else if (_sortAxis == SAxis.Y)
                            sort = Mathf.Abs(_cameraTransform.position.y - transform.position.y) * 16;
                        else
                            sort = Mathf.Abs(_cameraTransform.position.z - transform.position.z) * 16;

                        spriteRenderer.sortingOrder = Mathf.CeilToInt(-sort + _offset);
                        //Debug.Log(spriteRenderer.name + ";" + spriteRenderer.sortingOrder + ";" + spriteRenderer.transform.position);
                    }
                }
                _worldPosition = pos;
                // if (TryGetComponent(out SpriteRenderer spriteRenderer))
                // {
                //     float sort;
                //     if(_sortAxis == SAxis.X)
                //         sort = Mathf.Abs(_cameraTransform.position.x - transform.position.x) * 16;
                //     else if(_sortAxis == SAxis.Y)
                //         sort = Mathf.Abs(_cameraTransform.position.y - transform.position.y) * 16;
                //     else
                //         sort = Mathf.Abs(_cameraTransform.position.z - transform.position.z) * 16;

                //     spriteRenderer.sortingOrder = Mathf.CeilToInt(-sort + _offset);
                //     _worldPosition = pos;
                // }
            }

        }
    }
}
