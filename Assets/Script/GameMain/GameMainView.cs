using SFramework;
using SFramework.Game;
using UnityEngine;

namespace Game
{
    public class GameMainView : SSCENEView
    {
        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override void SetViewPrefabPath(out string prefabPath, out string prefabName)
        {
            prefabPath = "Scenes/BaseScene";
            prefabName = "BaseScene";
        }

        protected override void opening()
        {
            //open loading ui
            Control.OpenControl("Game.NormalLoadingControl");
        }

        protected override void loadSceneComplete()
        {
            Debug.Log("gamemainview loading finish");
        }
    }
}