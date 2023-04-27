using UnityEngine;

namespace SFramework
{
    public abstract class SUIView : SView
    {
        public abstract UILayer GetViewLayer();

        public abstract Transform GetViewTransform();

        public override void Open()
        {
            UILayer layer = GetViewLayer();
            Transform trans = GetViewTransform();
        }
    }
}

