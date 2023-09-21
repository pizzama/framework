using SFramework.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game
{
    public abstract class RootView : SView
    {
        protected abstract ViewOpenType GetViewOpenType();
        protected AssetsManager assetManager;
        protected GameObjectPoolManager poolManager; //pool manager

        public SUIROOT UIRoot { get { return SUIROOT.Instance; } }

        public override void Install()
        {
            assetManager = AssetsManager.Instance;
            poolManager = GameObjectPoolManager.Instance;
            //init ui
            base.Install();
        }

        public GameObject CreateGameObjectUsingPool(string abName, string resName, float lifeTime = -1)
        {
            UnityEngine.GameObject prefab = assetManager.LoadFromBundle<UnityEngine.GameObject>(abName, resName);
            string poolName = assetManager.FullPath(abName, resName);
            return poolManager.Request<ListGameObjectPool>(poolName, prefab, lifeTime);
        }

        public GameObject CreateGameObjectUsingPool(string path, float lifeTime = -1)
        {
            int index = path.LastIndexOf("/");
            if(index > 0)
            {
                string abName = path.Substring(0, index);
                string resName = path.Substring(index + 1, path.Length - index - 1);
                return CreateGameObjectUsingPool(abName, resName, lifeTime);
            }

            return null;
        }
    }
}