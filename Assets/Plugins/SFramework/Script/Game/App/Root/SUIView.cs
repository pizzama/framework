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

        public override Transform GetViewTransform()
        {
            return mViewTransform;
        }

        protected override T getExportObject<T>(string key)
        {
            GameObject go = null;
            goDict.TryGetValue(key, out go);
            if (go != null)
            {
                T rt = go.GetComponent<T>();
                if (rt is RootEntity root)
                {
                    root.SetParentView(this);
                }
                return rt;
            }
            else
            {
                goDict.TryGetValue(key + "(Clone)", out go);
                if (go != null)
                {
                    T rt = go.GetComponent<T>();
                    if (rt is RootEntity root)
                    {
                        root.SetParentView(this);
                    }
                    return rt;
                }
            }

            return default(T);
        }

        public override void Open()
        {
            ViewOpenType tp = Control.GetViewOpenType();
            if (tp == ViewOpenType.SingleNone || tp == ViewOpenType.SingleNeverClose)
            {
                ViewCallback?.Invoke();
                return;
            }
            
            Vector2 offsetMin;
            Vector2 offsetMax;
            Quaternion rotation;
            SetViewPrefabPath(out mAbName, out mResName, out offsetMin, out offsetMax, out rotation);
            //SetViewTransform(out mViewTransform, position, rotation);
            SetViewTransformAsync(offsetMin, offsetMax, rotation).Forget();
        }

        protected virtual void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector2 offsetMin, out Vector2 offsetMax, out Quaternion rotation)
        {
            Type tp = GetType();
            string path = tp.Namespace;
            path = path.Replace('.', '_');
            prefabPath = path + "." + AssetsManager.PrefabExtendName;
            prefabName = tp.Name;
            offsetMin = new Vector2(0, 0);
            offsetMax = new Vector2(0, 0);
            rotation = Quaternion.Euler(0, 0, 0);
        }

        protected virtual void SetViewTransform(Vector2 offsetMin, Vector2 offsetMax, Quaternion rotation)
        {
            if (!string.IsNullOrEmpty(mAbName) && !string.IsNullOrEmpty(mResName))
            {
                GameObject prefab = assetManager.LoadFromBundle<GameObject>(mAbName, mResName);
                if (prefab == null)
                    throw new NotFoundException("not found uiview prefab:" + mAbName + ";" + mResName);
                string fullPath = assetManager.FullPath(mAbName, mResName);
                GameObject ob = poolManager.Request<ListGameObjectPool>(fullPath, prefab, -1, 5);
                mViewTransform = ob.transform;
            }
            openUI(mViewTransform, offsetMin, offsetMax, rotation);
        }

        protected virtual async UniTaskVoid SetViewTransformAsync(Vector2 offsetMin, Vector2 offsetMax, Quaternion rotation)
        {
            if (!string.IsNullOrEmpty(mAbName) && !string.IsNullOrEmpty(mResName))
            {
                GameObject prefab = await assetManager.LoadFromBundleAsync<GameObject>(mAbName, mResName);
                if (prefab == null)
                    throw new NotFoundException("not found uiView prefab:" + mAbName + ";" + mResName);
                string fullPath = assetManager.FullPath(mAbName, mResName);
                GameObject ob = poolManager.Request<ListGameObjectPool>(fullPath, prefab, -1, 5);
                mViewTransform = ob.transform;
            }
            openUI(mViewTransform, offsetMin, offsetMax,rotation);
        }

        protected virtual void openUI(Transform trans, Vector2 offsetMin, Vector2 offsetMax, Quaternion rotation)
        {
            if (trans != null)
            {
                if (UIRoot)
                {
                    UILayer layer = GetViewLayer();
                    UIRoot.OpenUI(layer, trans, offsetMin, offsetMax, rotation);
                    goDict = mViewTransform.gameObject.CollectAllGameObjects(findTag);
                }
                else
                {
                    throw new NotFoundException("not found SUIROOT please check the scene");
                }

            }
            base.Open();
            rootEntityShowTrigger(goDict);
            ViewCallback?.Invoke();
        }

        public override void Close()
        {
            rootEntityRecycleTrigger(goDict);
            if (mViewTransform != null && poolManager != null)
            {
                string fullPath = assetManager.FullPath(mAbName, mResName);
                poolManager.Return(fullPath, mViewTransform.gameObject);
                base.Close();
            }
        }
        
    }
}

