using UnityEngine;

namespace SFramework
{
    public abstract class SUIView : RootView
    {
        //set the ui in which layer
        protected abstract UILayer GetViewLayer();
        //set ui prefab
        protected abstract void SetViewTransform(out Transform trans, out Vector3 position, out Quaternion rotation);
        protected override void init()
        {
        }
        public override void Open()
        {
            UILayer layer = GetViewLayer();
            Transform trans = null;
            Vector3 position = default;
            Quaternion rotation = default;
            SetViewTransform(out trans, out position, out rotation);
            uiRoot.OpenUI(layer, trans, position, rotation);
        }
    }
}

