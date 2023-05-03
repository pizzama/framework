using UnityEngine;

namespace SFramework
{
    public abstract class SUIView : SView
    {
        protected abstract UILayer GetViewLayer();

        protected abstract void SetViewTransform(out Transform trans, out Vector3 position, out Quaternion rotation);

        public override void Open()
        {
            UILayer layer = GetViewLayer();
            Transform trans = null;
            Vector3 position = default;
            Quaternion rotation = default;
            SetViewTransform(out trans, out position, out rotation);
            SUIROOT.Instance.OpenUI(layer, trans, position, rotation);
        }
    }
}

