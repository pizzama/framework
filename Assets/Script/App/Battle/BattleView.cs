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
    public class BattleView : SSCENEView
    {
        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override void opening()
        {
            var input = SFInputManager.Instance;
            Debug.Log("load complete");
            Transform parent = getSceneObject<Transform>("Actors");
            if (parent != null)
            {
                Hero rt = CreateSEntity<Hero>("1", SFResAssets.Model_avatar_role_carrota_sf_Character_Carrot_prefab, parent);
                if (rt != null)
                {
                    rt.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                }
            }

        }
    }
}