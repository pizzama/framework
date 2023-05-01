using UnityEngine;

namespace SFramework
{
    public abstract class SUIView : SView
    {
        protected abstract UILayer GetViewLayer();

        protected abstract Transform GetViewTransform();

        public override void Open()
        {
            UILayer layer = GetViewLayer();
            Transform trans = GetViewTransform();
            SUIROOT.Instance.OpenUI(layer, trans);
        }
    }
}

