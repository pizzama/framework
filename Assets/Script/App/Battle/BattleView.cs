using SFramework.Game;
using SFramework;
using UnityEngine;
using UnityEngine.UI;
using SFramework.Actor;
using SFramework.Statics;
using Game.Character;
using SFramework.Game.App;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.App.Battle
{
    public class BattleView : SSCENEView
    {
        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override void opening()
        {
            BattleModel mode = GetControl<BattleControl>().GetModel<BattleModel>();
            var input = SFInputManager.Instance;
            Debug.Log("load complete");
            Transform parent = getExportObject<Transform>("Actors");
            if (parent != null)
            {
                Hero rt = CreateSEntity<Hero>("1", SFResAssets.Model_avatar_role_carrota_sfp_Character_Carrot_prefab, parent);
                if (rt != null)
                {
                    rt.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                }
            }

            readPro().Forget();
        }

        private async UniTaskVoid readPro()
        {
            List<GameObject> results = await assetManager.LoadFromBundleWithSubResourcesAsync<GameObject>(Game_turpworld_map_sfp.BundleName);
            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log("load complete:" + results[i].name);
            }
        }
    }
}