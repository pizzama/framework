using System;
using System.Collections.Generic;
using SFramework.Extension;
using UnityEngine;
using UnityEngine.Rendering;
using static SEnum;

namespace SFramework.Sprites
{
    // [ExecuteAlways]
    public class SpriteSorting : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;

        public Transform CameraTransform
        {
            get { return _cameraTransform; }
            set { _cameraTransform = value; }
        }
        [SerializeField] private int _offset;
        [SerializeField] private Vector3 _worldPosition;

        [SerializeField] private SAxis _sortAxis = SAxis.Y;

        private const int sorting = 16;
        private List<SpriteRenderer> _sprites;
        private List<ParticleSystemRenderer> _particleSystem;
        private List<SortingGroup> _sortingGroups;

        private void OnEnable()
        {
            if (_cameraTransform == null)
                _cameraTransform = Camera.main.transform;
            _sortingGroups = this.transform.CollectAllComponent<SortingGroup>();
            if (_sortingGroups.Count == 0)
            {
                _sprites = this.transform.CollectAllComponent<SpriteRenderer>();
                _particleSystem = this.transform.CollectAllComponent<ParticleSystemRenderer>();
            }
        }
        private void LateUpdate()
        {
            Vector3 pos = transform.position;
            if (pos != _worldPosition)
            {
                if (_sprites != null && _sprites.Count > 0)
                {
                    for (int i = 0; i < _sprites.Count; i++)
                    {
                        SpriteRenderer render = _sprites[i];
                        float sort = caculateSort(_cameraTransform);
                        render.sortingOrder = Mathf.CeilToInt(sort + _offset);
                    }
                }

                if (_particleSystem != null && _particleSystem.Count > 0)
                {
                    for (int i = 0; i < _particleSystem.Count; i++)
                    {
                        ParticleSystemRenderer render = _particleSystem[i];
                        float sort = caculateSort(_cameraTransform);
                        render.sortingOrder = Mathf.CeilToInt(sort + _offset);
                    }
                }
                
                if (_sortingGroups != null && _sortingGroups.Count > 0)
                {
                    for (int i = 0; i < _sortingGroups.Count; i++)
                    {
                        SortingGroup render = _sortingGroups[i];
                        float sort = caculateSort(_cameraTransform);
                        render.sortingOrder = Mathf.CeilToInt(sort + _offset);
                    }
                }
                _worldPosition = pos;
            }
        }
        
        private float caculateSort(Transform camera)
        {
            float sort;
            if (_sortAxis == SAxis.X)
                sort = Mathf.Abs(_cameraTransform.position.x - transform.position.x) * sorting;
            else if (_sortAxis == SAxis.Y)
            {
                if (transform.position.y > 0)
                {
                    sort = -Mathf.Abs(_cameraTransform.position.y - transform.position.y) * sorting;
                }
                else
                {
                    sort = Mathf.Abs(_cameraTransform.position.y - transform.position.y) * sorting;
                }
            }
            else
                sort = Mathf.Abs(_cameraTransform.position.z - transform.position.z) * sorting;

            return sort;
        }
    }
    

}
