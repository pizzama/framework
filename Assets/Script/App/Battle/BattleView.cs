using SFramework.Game;
using SFramework;
using UnityEngine;
using UnityEngine.UI;
using SFramework.Actor;
using SFramework.Statics;
using Game.Character;
using SFramework.Game.App;

namespace Game.Scenes
{
    public class BattleView : SSCENEView
    {
        private SFActorFactory _factory;
        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override void loadSceneComplete()
        {
            var input = SFInputManager.Instance;
            Debug.Log("load complete");
            Transform parent = getSceneObject<Transform>("Actors");
            _factory = new SFActorFactory(this);
            Hero rt = _factory.Create<Hero>("1", SFResAssets.Model_avatar_role_carrota_sf_Character_Carrot_prefab, parent);
            if(rt != null)
            {
                rt.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            }

        }
    }
}