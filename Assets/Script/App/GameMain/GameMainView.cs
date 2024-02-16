using SFramework;
using SFramework.Game;
using UnityEngine;
using SFramework.Statics;

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
            prefabPath = Game_scenes_sf.BundleName;
            prefabName = Game_scenes_sf.RES_BaseScene_unity;
        }

        public override void Open()
        {
            base.Open();
            //open loading ui
            Control.OpenControl(SFStaticsControl.Game_NormalLoadingControl, "Start");
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

        protected override void opening()
        {
            //Debug.Log("gamemainview loading finish");
            Control.OpenControl("Game.NormalLoadingControl", "End");

            Texture tx = assetManager.LoadFromBundle<Texture>(Pic_sf.BundleName, Pic_sf.RES_haipa_png);
            Debug.Log(tx);

            Transform ga = getSceneObject<Transform>("Env");

            GameObject aa = CreateGameObjectUsingPool(Model_avatar_role_carrota_sf.BundleName, Model_avatar_role_carrota_sf.RES_Character_Carrot_prefab);
            Debug.Log(aa);
        }
    }
}