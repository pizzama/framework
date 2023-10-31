using SFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SFramework.Extension;

namespace Game.App.Battle
{
    //deal with card operation
    public class BattleCard : SEntity, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private float _moveTime = 0.2f;

        private Vector3 _targetPos;
        private Vector3 _targetRot;
        private Vector3 _targetScl;

        private Coroutine _coMove;

        public bool IsBezierCurve { get; set; }

        public void MoveCard(Vector3 targetPos, Vector3 targetRot, Vector3 targetScl)
        {
            _targetPos = targetPos;
            _targetRot = targetRot;
            _targetScl = targetScl;

            if (_coMove != null)
            {
                StopCoroutine(_coMove);
            }
            _coMove = StartCoroutine(CoMove());
        }

        private IEnumerator CoMove()
        {
            float currentTime = 0f;

            Vector3 originPos = transform.localPosition;
            Vector3 originRot = transform.localEulerAngles;
            Vector3 originScl = transform.localScale;

            while (true)
            {
                currentTime += Time.deltaTime;

                transform.localPosition = Vector3.Lerp(originPos, _targetPos, currentTime / _moveTime);
                transform.localEulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(originRot.z, _targetRot.z, currentTime / _moveTime));
                transform.localScale = Vector3.Lerp(originScl, _targetScl, currentTime / _moveTime);

                if (currentTime >= _moveTime)
                    break;

                yield return null;
            }

        }

        public void SetActiveRaycast(bool flag)
        {
            Image[] children = GetComponentsInChildren<Image>();
            foreach (Image child in children)
            {
                child.raycastTarget = flag;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("click");
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            BattleUIView uiView = (BattleUIView)ParentView;
            if (uiView.IsDrag)
                return;
            uiView.OverCard(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            BattleUIView uiView = (BattleUIView)ParentView;
            if (uiView.IsDrag)
                return;
            uiView.Relocation();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // current card will be selected and uiview will know what happened
            BattleUIView uiView = (BattleUIView)ParentView;
            uiView.IsDrag = true;
            uiView.SelectCard = this;
            StopAllCoroutines();//stop all card coroutines

            transform.SetAsLastSibling();
            IsBezierCurve = true;
            if (IsBezierCurve) //if the card is directivity
            {
                uiView.ShowBezierCurve();
                uiView.MoveCenter(this);
            }

        }

        public void OnDrag(PointerEventData eventData)
        {
            // covert screen position to world position 
            BattleUIView uiView = (BattleUIView)ParentView;
            var pos = uiView.UIRoot.UICamera.ScreenToWorldPoint(eventData.position);
            pos.z = 0; // keep z axis is zero
            if (IsBezierCurve)
            {
                uiView.SetBezierCurveTransform(transform.position, pos);
            }
            else
            {
                transform.position = pos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            BattleUIView uiView = (BattleUIView)ParentView;
            uiView.IsDrag = false;

            if (IsBezierCurve)
            {
                SetActiveRaycast(false);
                uiView.HideBezierCurve();
                UseCard();
            }
            else
            {
                if (eventData.position.y > 300f)
                {
                    SetActiveRaycast(false);
                    UseCard();
                }
            }

            uiView.SelectCard = null;
            uiView.Relocation(); //all card reset position
        }

        private void UseCard()
        {
        }
    }
}
