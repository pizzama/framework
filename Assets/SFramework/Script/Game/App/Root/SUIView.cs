using System.Collections.Generic;
using UnityEngine;
using SFramework.Pool;
using SFramework.Tools;
using System;

namespace SFramework.Game
{
    public abstract class SUIView : RootView
    {
        //set the ui in which layer
        protected abstract UILayer GetViewLayer();
        protected Dictionary<string, GameObject> goDict;

        protected Transform mViewTransform;

        protected string mAbPath; //ui asset bundle path
        protected string mAbName; //ui asset bundle name

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
            {
                goDict.TryGetValue(key + "(Clone)", out go);
                if(go != null)
                {
                    return go.GetComponent<T>();
                }
            }
 
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
                UIRoot.OpenUI(layer, mViewTransform, position, rotation);
                goDict = ComponentTools.collectAllGameObjects(mViewTransform.gameObject);
            }
            base.Open();
        }

        protected virtual void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector3 position, out Quaternion rotation)
        {
            Type tp = GetType();
            string path = tp.FullName;
            path = path.Replace('.', '/');
            prefabPath = path;
            prefabName = tp.Name;
            position = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 0);
        }

        protected virtual void SetViewTransform(out Transform trans, out Vector3 position, out Quaternion rotation)
        {
            trans = null;
            SetViewPrefabPath(out mAbPath, out mAbName, out position, out rotation);

            if (!string.IsNullOrEmpty(mAbPath))
            {
                ListGameObjectPool pool = poolManager.CreateGameObjectPool<ListGameObjectPool>(mAbPath);
                if (pool.Prefab == null)
                {
                    pool.Prefab = assetManager.LoadResource<GameObject>(mAbPath, mAbName);
                }

                trans = pool.Request().transform;
            }
        }

        public override void Close()
        {
            if(mViewTransform != null && poolManager != null)
            {
                poolManager.ReturnGameObject(mAbPath, mViewTransform.gameObject);
                base.Close();
            }
        }
    }
}

