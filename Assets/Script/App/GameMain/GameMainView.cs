using Cysharp.Threading.Tasks;
using SFramework;
using SFramework.Game;
using SFramework.Tools;
using System.Collections.Generic;
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
            Control.OpenControl("Game.NormalLoadingControl", "Start");
        }

        //if you need control the loading progress you will override the method
        //protected override async UniTask<bool> loadingScene(float progress)
        //{
        //    if(progress >= 0.9f)
        //    {
        //        Control.BroadcastMessage("LoadingEnd", "Game.NormalLoadingControl");
        //        await UniTask.Delay(300);
        //        return true;

        //    }
        //    return false;
        //}

        protected override void loadSceneComplete()
        {
            //Debug.Log("gamemainview loading finish");
            Control.OpenControl("Game.NormalLoadingControl", "End");

            Texture tx = assetManager.LoadResource<Texture>("pic/cha", "haipa");
            Debug.Log(tx);

            Transform ga = getSceneObject<Transform>("Env");

            GameObject ab = assetManager.LoadResource<GameObject>("model/avatar", "Role_CarrotA_Skin");
            bool rt = ab.IsPrefab();
            Debug.Log(ab);

            GameObject aa = CreateGameObjectUsingPool("model/avatar", "Role_CarrotA_Skin");
            rt = aa.IsPrefab();
            Debug.Log(aa);
        }
    }
}