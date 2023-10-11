using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework.Tools;
using SFramework.Game;
using Unity.VisualScripting;


namespace SFramework.Actor
{
    //using composite pattern let factory and view manager actor or other entities
    public class SFActorFactory
    {
        private List<ISFEntity> _entities;
        private RootView _view; //base scene create actor
        public SFActorFactory(RootView view)
        {
            _view = view;
            _entities = new List<ISFEntity>();
        }

        public T Create<T>(string id, string prefabFullPath, Transform parent, Vector3 pos = default, float lifeTime = -1) where T : SFEntity
        {
            GameObject obj = _view.CreateGameObjectUsingPool(prefabFullPath, lifeTime);
            T result = obj.GetOrAddComponent<T>();
            if (result == null)
                throw new DataErrorException("Could not add component:" + typeof(T));
            if (parent != null)
            {
                result.transform.SetParent(parent, false);
                result.transform.localPosition = pos;
            }
            result.SetData(id, prefabFullPath);
            _entities.Add(result);
            return result;
        }

    }
}
