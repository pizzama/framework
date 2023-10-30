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
        protected override UILayer GetViewLayer()
        {
            return UILayer.Hud;
        }

        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }
    }
}