using SFramework.Game;
using SFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scenes
{
    public class BattleView : SSCENEView
    {
        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override void loadSceneComplete()
        {
            Debug.Log("load complete");
        }
    }
}