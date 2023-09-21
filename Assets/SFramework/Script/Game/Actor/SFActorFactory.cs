using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework.Tools;


namespace SFramework.Game.Actor
{
    public class SFActorFactory
    {
        private List<ISFEntity> _entitys;
        private RootView _view; //base scene create actor
        public SFActorFactory(RootView view)
        {
            _view = view;
            _entitys = new List<ISFEntity>();
        }

        public SFEntity Create<T>(string id, string prefabFullPath, float lifeTime = -1) where T : SFEntity
        {
            GameObject obj = _view.CreateGameObjectUsingPool(prefabFullPath, lifeTime);
            obj.GetOrAddComponent<T>();
            return null;
        }

    }
}
