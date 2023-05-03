using SFramework;
using UnityEngine;

namespace game
{
    public class TestView : SUIView
    {
        protected override UILayer GetViewLayer()
        {
            return UILayer.Popup;
        }

        protected override void SetViewTransform(out Transform trans, out Vector3 position, out Quaternion rotation)
        {
            trans = abManager.LoadResource<RectTransform>("ss/test", "Test");
            position = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 180);
        }
    }
}