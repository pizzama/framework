using UnityEngine;

namespace PFramework
{
    public class RootUIView : PView
    {
        public override void Open()
        {
            Sprite sp = abManager.LoadResource<Sprite>("a_png", "a");
            Debug.Log("spppp");

        }
    }
}