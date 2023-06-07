using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    public abstract class SUIView : RootView
    {
        //set the ui in which layer
        protected abstract UILayer GetViewLayer();
        //set ui prefab
        protected abstract void SetViewTransform(out Transform trans, out Vector3 position, out Quaternion rotation);

        protected Dictionary<string, GameObject> goDict;
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
            Transform trans = null;
            Vector3 position = default;
            Quaternion rotation = default;
            SetViewTransform(out trans, out position, out rotation);
            if(trans != null)
            {
                UIROOT.OpenUI(layer, trans, position, rotation);
                goDict = ComponentTools.collectAllGameObjects(UIROOT.gameObject);
            }
            base.Open();
        }
    }
}

