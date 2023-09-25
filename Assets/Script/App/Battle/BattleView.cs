using SFramework.Game;
using SFramework;
using UnityEngine;
using UnityEngine.UI;
using SFramework.Game.Actor;
using SFramework.Statics;
using Game.Character;

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
            Debug.Log("load complete");
            Transform parent = getSceneObject<Transform>("Actors");
            _factory = new SFActorFactory(this);
            Hero rt = _factory.Create<Hero>("1", SFResAssets.Model_avatar_sf_Role_CarrotA_Skin_prefab, parent);
        }
    }
}