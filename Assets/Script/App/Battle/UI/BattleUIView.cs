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
        private List<CardControl> _cardHands;
        private float angle = 5.5f;
        private float distance = 2000;
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
            _cardHandsTransform = getUIObject<RectTransform>("CardHands");
            _cardHands = new List<CardControl>();

            for (int i = 0; i < 6; i++)
            {
                CardControl ctl = CreateSEntity<CardControl>(i.ToString(), SFResAssets.Game_app_battle_sf_Card_prefab, _cardHandsTransform);
                _cardHands.Add(ctl);
            }


            relocation();
        }

        private void relocation()
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

        private float GetStartTheta()
        {
            float startTheta = 0;
            if (_cardHands.Count % 2 == 0)
                startTheta -= _cardHands.Count / 2 * angle - (angle / 2) - 90;
            else
                startTheta -= (_cardHands.Count / 2) * angle - 90;

            return startTheta;
        }
    }
}