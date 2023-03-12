using System;
using System.Collections.Generic;
using UnityEngine;

namespace PUtils
{
    public class BezierArrows : MonoBehaviour
    {
        [Tooltip("The prefab of arrow head")]
        public GameObject ArrowHeadPrefab;
        [Tooltip("The prefab of arrow node")]
        public GameObject ArrowNodePrefab;
        [Tooltip("The number of arrow node")]
        public int ArrowNodeNum;
        [Tooltip("The scale multipler for arrow nodes")]
        public float ScaleFactor = 1f;

        //The position of P0
        private RectTransform _origin;
        private List<RectTransform> _arrowNodes = new List<RectTransform>();
        private List<Vector2> _controlPoints = new List<Vector2>();
        //The factors to determine the position of control point p1, p2
        private readonly List<Vector2> _controlPointFactors = new List<Vector2> { new Vector2(-0.3f, 0.0f), new Vector2(0.1f, 1.4f) };

        private void Awake()
        {
            _origin = GetComponent<RectTransform>();
            for (int i = 0; i < ArrowNodeNum; i++)
            {
                _arrowNodes.Add(Instantiate(ArrowNodePrefab, transform).GetComponent<RectTransform>());
            }

            _arrowNodes.Add(Instantiate(ArrowHeadPrefab, transform).GetComponent<RectTransform>());
            //Hide the arrow node
            _arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));
            //Initialize the control points list
            for (int i = 0; i < 4; i++)
            {
                _controlPoints.Add(Vector2.zero);
            }
        }

        private void Update()
        {
            //p0 is at the arrow emitter point
            _controlPoints[0] = new Vector2(_origin.position.x, _origin.position.y);
            //p3 is at the mouse position
            _controlPoints[3] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //p1, p2 determines by p0 and p3
            _controlPoints[1] = _controlPoints[0] + (_controlPoints[3] - _controlPoints[0]) * _controlPointFactors[0];
            _controlPoints[2] = _controlPoints[0] + (_controlPoints[3] - _controlPoints[0]) * _controlPointFactors[1];

            
            for (int i = 0; i < _arrowNodes.Count; i++)
            {
                //caculate t
                var t = Mathf.Log(1f * i / (this._arrowNodes.Count - 1) + 1f, 2f);
                //cubic bezier curve
                // B(t) = (1-t)^3*p0+3*(1-t)^Y2*p1 + 3*(1-t)*t^2*p2+t^3*p3
                _arrowNodes[i].position = Mathf.Pow(1 - t, 3) * _controlPoints[0] + 3 * Mathf.Pow(1 - t, 2) * t * _controlPoints[1] + 3 * (1 - t) * Mathf.Pow(t, 2) * _controlPoints[2] + Mathf.Pow(t, 3) * _controlPoints[3];
                // caculates rotations for each arrow node. let direction same
                if (i > 0)
                {
                    var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, _arrowNodes[i].position - _arrowNodes[i-1].position));
                    _arrowNodes[i].rotation = Quaternion.Euler(euler);
                }
                //Caculates scales for each arow node.
                var scale = ScaleFactor*(1f-0.03f*(_arrowNodes.Count - 1 - i));
                _arrowNodes[i].localScale = new Vector3(scale, scale, 1f);
            }

            _arrowNodes[0].transform.rotation = _arrowNodes[1].transform.rotation;
        }
    }
}