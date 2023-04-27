using UnityEngine;

namespace SFramework
{
    public abstract class SUIView : SView
    {
        public abstract UILayer GetViewLayer();

        public abstract Transform GetViewTransform();
    }
}

