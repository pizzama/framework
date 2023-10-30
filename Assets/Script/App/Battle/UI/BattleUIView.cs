using SFramework.Game;
using SFramework;
using UnityEngine;
using UnityEngine.UI;
using SFramework.Actor;
using SFramework.Statics;
using Game.Character;
using SFramework.Game.App;

namespace Game.App.Battle
{
    public class BattleUIView : SUIView
    {
        private RectTransform _cardHands;
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
            _cardHands = getUIObject<RectTransform>("CardHands");
            for (int i = 0; i < 6; i++)
            {
                CreateSEntity<CardControl>(i.ToString(), SFResAssets.Game_app_battle_sf_Card_prefab, _cardHands);
            }
        }
    }
}