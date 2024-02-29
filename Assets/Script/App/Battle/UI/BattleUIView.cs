using SFramework.Game;
using SFramework;
using UnityEngine;
using UnityEngine.UI;
using SFramework.Statics;
using Game.Character;
using SFramework.Game.App;
using System.Collections.Generic;

namespace Game.App.Battle
{
    public class BattleUIView : SUIView
    {
        private RectTransform _cardHandsTransform;
        private BezierCurve _bezierCurve;
        private List<BattleCard> _cardHands;
        private float angle = 5.5f;
        private float distance = 2000;
        private float overSidePosition = 50f;
        private float overUpPosition = 200f;
        private float overScale = 1.3f;

        public bool IsDrag = false;
        private BattleCard _selectCard;
        public BattleCard SelectCard
        {
            get { return _selectCard; }
            set { _selectCard = value; }
        }

        protected override UILayer GetViewLayer()
        {
            return UILayer.Hud;
        }

        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override void opening()
        {
            _cardHandsTransform = getExportObject<RectTransform>("CardHands");
            _bezierCurve = getExportObject<BezierCurve>("BezierCurve");
            _cardHands = new List<BattleCard>();

            for (int i = 0; i < 6; i++)
            {
                BattleCard ctl = CreateSEntity<BattleCard>(i.ToString(), SFResAssets.Game_app_battle_sfp_Card_prefab, _cardHandsTransform);
                _cardHands.Add(ctl);
            }

            Relocation();
        }

        public void Relocation()
        {
            float startTheta = GetStartTheta();
            for (int i = 0; i < _cardHands.Count; i++)
            {
                _cardHands[i].transform.SetAsFirstSibling();

                float theta = startTheta + angle * i;
                Vector3 targetPos = mViewTransform.transform.position + new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad), 0) * distance;
                Vector3 targetRot = new Vector3(0f, 0f, theta - 90);
                Vector3 targetScl = Vector3.one;

                _cardHands[i].MoveCard(targetPos, targetRot, targetScl);
                _cardHands[i].transform.SetAsFirstSibling();
            }
        }

        public void OverCard(BattleCard card)
        {
            float startTheta = GetStartTheta();

            int index = GetCardIndex(card);

            if (index == -1)
                return;

            for (int i = 0; i < _cardHands.Count; i++)
            {
                _cardHands[i].transform.SetAsFirstSibling();

                float theta = startTheta + angle * i;
                Vector3 targetPos = mViewTransform.transform.position + new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad), 0) * distance;
                Vector3 targetRot = new Vector3(0f, 0f, theta - 90);
                Vector3 targetScl = Vector3.one;

                if (i < index)
                {
                    targetPos += Vector3.right * overSidePosition;
                }
                else if (i > index)
                {
                    targetPos -= Vector3.right * overSidePosition;
                }
                else
                {
                    targetPos += Vector3.up * overUpPosition;
                    targetRot = Vector3.zero;
                    targetScl = Vector3.one * overScale;
                }

                _cardHands[i].MoveCard(targetPos, targetRot, targetScl);
                _cardHands[i].transform.SetAsFirstSibling();
            }

            _cardHands[index].transform.SetAsLastSibling();
        }

        public void MoveCenter(BattleCard card)
        {
            Vector3 targetPos = Vector3.up * (-Screen.height / 2 + overUpPosition / 2);
            Vector3 targetRot = Vector3.zero;
            Vector3 targetScl = Vector3.one * overScale;

            card.MoveCard(targetPos, targetRot, targetScl);
        }

        public void ShowBezierCurve()
        {
            _bezierCurve.gameObject.SetActive(true);
        }

        public void HideBezierCurve()
        {
            _bezierCurve.gameObject.SetActive(false);
        }

        public void SetBezierCurveTransform(Vector3 p0, Vector3 p2)
        {
            _bezierCurve.p0.position = p0;
            _bezierCurve.p2.position = p2;
        }

        private float GetStartTheta()
        {
            float startTheta = 0;
            if (_cardHands.Count % 2 == 0)
                startTheta -= _cardHands.Count / 2 * angle - (angle / 2) - 90;
            else
                startTheta -= (_cardHands.Count / 2) * angle - 90;

            return startTheta;
        }
        

        private int GetCardIndex(BattleCard card)
        {
            for (int i = 0; i < _cardHands.Count; i++)
            {
                if (card == _cardHands[i])
                {
                    return i;
                }
            }
            return -1;
        }
    }

    
}