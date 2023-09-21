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

        public T Create<T>(string id, string prefabFullPath, Transform parent, Vector3 pos = default, float lifeTime = -1) where T : SFEntity
        {
            if(parent == null)
                Debug.LogWarning("parent is null");
            GameObject obj = _view.CreateGameObjectUsingPool(prefabFullPath, lifeTime);
            T result = obj.GetOrAddComponent<T>();
            result.transform.SetParent(parent, false);
            result.transform.localPosition = pos;
            result.SetData(id, prefabFullPath);
            _entitys.Add(result);
            return result;
        }

    }
}
