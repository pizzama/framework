using System.Collections.Generic;
using UnityEngine;
using SFramework.Pool;

namespace SFramework
{
    public abstract class SUIView : RootView
    {
        //set the ui in which layer
        protected abstract UILayer GetViewLayer();
        //set ui prefab
        protected abstract void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector3 position, out Quaternion rotation);

        protected Dictionary<string, GameObject> goDict;

        protected Transform mViewTransform;
        protected override void init()
        {
            goDict = new Dictionary<string, GameObject>();
        }

        protected T getAssetFromGoDict<T>(string key)
        {
            GameObject go = null;
            goDict.TryGetValue(key, out go);
            if (go != null)
                return go.GetComponent<T>();
            else
                return default(T);
        }

        public override void Open()
        {
            UILayer layer = GetViewLayer();
            Vector3 position = default;
            Quaternion rotation = default;
            SetViewTransform(out mViewTransform, out position, out rotation);
            if (mViewTransform != null)
            {
                uiRoot.OpenUI(layer, mViewTransform, position, rotation);
                goDict = ComponentTools.collectAllGameObjects(uiRoot.gameObject);
            }
            base.Open();
        }

        protected virtual void SetViewTransform(out Transform trans, out Vector3 position, out Quaternion rotation)
        {
            string abPath;
            string abName;
            SetViewPrefabPath(out abPath, out abName, out position, out rotation);

            BaseGameObjectPool pool = rootManager.GetUIPool(abPath);
            if (pool.Prefab == null)
            {
                pool.Prefab = assetManager.LoadResource<GameObject>(abPath, abName);
            }

            trans = pool.Request().transform;
            // trans = assetManager.LoadResource<Transform>("Test");
        }

        public override void Close()
        {

            base.Close();
        }
    }
}

