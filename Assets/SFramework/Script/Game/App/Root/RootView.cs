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

        protected GameObject CreateGameObjectUsingPool(string abName, string resName, float lifeTime = -1)
        {
            UnityEngine.GameObject prefab = assetManager.LoadFromBundle<UnityEngine.GameObject>(abName, resName);
            string poolName = assetManager.FullPath(abName, resName);
            return poolManager.Request<ListGameObjectPool>(poolName, prefab, lifeTime);

        }
    }
}