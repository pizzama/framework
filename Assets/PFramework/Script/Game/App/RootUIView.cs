using UnityEngine;

namespace PFramework
{
    public class RootUIView : PView
    {
        public override void Open()
        {
            Sprite sp = abManager.LoadResource<Sprite>("activity_btn_arrow", "activity_btn_arrow");
            Debug.Log("spppp");

        }
    }
}