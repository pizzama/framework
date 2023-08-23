using SFramework;
using SFramework.Game;
using UnityEngine;

namespace Game
{
    public class GameMainView : SSCENEView
    {
        private GameObject _gameRoot;

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

        }


    }
}