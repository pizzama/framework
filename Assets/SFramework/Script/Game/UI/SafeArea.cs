using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.UI
{
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private bool _left;
        [SerializeField] private bool _right;
        [SerializeField] private bool _top;
        [SerializeField] private bool _bottom;

        private void Start()
        {
            var panel = GetComponent<RectTransform>();
            var area = Screen.safeArea;

            var anchorMin = area.position;
            var anchorMax = area.position + area.size;

            if (_left) anchorMin.x /= Screen.width;
            else anchorMin.x = 0;

            if (_right) anchorMax.x /= Screen.width;
            else anchorMax.x = 1;

            if (_bottom) anchorMin.y /= Screen.height;
            else anchorMin.y = 0;

            if (_top) anchorMax.y /= Screen.height;
            else anchorMax.y = 1;

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }
    }
}
