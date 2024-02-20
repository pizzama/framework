using System.Collections.Generic;
using UnityEngine;
using SFramework.Pool;
using System;
using Cysharp.Threading.Tasks;
using SFramework.Extension;

namespace SFramework.Game
{
    public abstract class SUIView : RootView
    {
        private const string defaultVariantName = "sf";
        //set the ui in which layer
        protected abstract UILayer GetViewLayer();
        protected Dictionary<string, GameObject> goDict;

        protected Transform mViewTransform;

        protected string mAbName; //ui asset bundle path
        protected string mResName; //ui asset bundle name

        protected override void initing()
        {
            goDict = new Dictionary<string, GameObject>();
        }

        protected T getUIObject<T>(string key)
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
            Vector3 position;
            Quaternion rotation;
            SetViewPrefabPath(out mAbName, out mResName, out position, out rotation);
            //SetViewTransform(out mViewTransform, position, rotation);
            SetViewTransformAsync(position, rotation).Forget();
        }

        protected virtual void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector3 position, out Quaternion rotation)
        {
            Type tp = GetType();
            string path = tp.Namespace;
            path = path.Replace('.', '_');
            prefabPath = path + "." + defaultVariantName;
            prefabName = tp.Name;
            position = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 0);
        }

        protected virtual void SetViewTransform(Vector3 position, Quaternion rotation)
        {
            if (!string.IsNullOrEmpty(mAbName) && !string.IsNullOrEmpty(mResName))
            {
                GameObject prefab = assetManager.LoadFromBundle<GameObject>(mAbName, mResName);
                if(prefab == null)
                    throw new NotFoundException("not found uiview prefab:" + mAbName + ";" + mResName);
                string fullPath = assetManager.FullPath(mAbName, mResName);
                GameObject ob = poolManager.Request<ListGameObjectPool>(fullPath, prefab, -1);
                mViewTransform = ob.transform;
            }
            openUI(mViewTransform, position, rotation);
        }

        protected async UniTaskVoid SetViewTransformAsync(Vector3 position, Quaternion rotation)
        {
            if (!string.IsNullOrEmpty(mAbName) && !string.IsNullOrEmpty(mResName))
            {
                GameObject prefab = await assetManager.LoadFromBundleAsync<GameObject>(mAbName, mResName);
                if (prefab == null)
                    throw new NotFoundException("not found uiView prefab:" + mAbName + ";" + mResName);
                string fullPath = assetManager.FullPath(mAbName, mResName);
                GameObject ob = poolManager.Request<ListGameObjectPool>(fullPath, prefab, -1);
                mViewTransform = ob.transform;
            }
            openUI(mViewTransform, position, rotation);
        }

        protected virtual void openUI(Transform trans, Vector3 position, Quaternion rotation)
        {
            if (trans != null)
            {
                if (UIRoot)
                {
                    UILayer layer = GetViewLayer();
                    UIRoot.OpenUI(layer, trans, position, rotation);
                    goDict = mViewTransform.gameObject.CollectAllGameObjects(findTag);
                }
                else
                {
                    throw new NotFoundException("not found SUIROOT please check the scene");
                }

            }
            base.Open();
            ViewCallback?.Invoke();
        }

        public override void Close()
        {
            if(mViewTransform != null && poolManager != null)
            {
                string fullPath = assetManager.FullPath(mAbName, mResName);
                poolManager.Return(fullPath, mViewTransform.gameObject);
                base.Close();
            }
        }
    }
}

