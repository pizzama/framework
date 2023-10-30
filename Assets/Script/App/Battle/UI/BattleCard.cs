using SFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SFramework.StateMachine;

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
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnDrag(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
