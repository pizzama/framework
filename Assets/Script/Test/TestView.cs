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

        protected override Transform GetViewTransform()
        {
            RectTransform trans =  abManager.LoadResource<RectTransform>("ss/test", "Test");
            return trans;  
        }
    }
}