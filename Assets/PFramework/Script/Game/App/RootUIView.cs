using UnityEngine;

namespace PFramework
{
    public class RootUIView : PView
    {
        public override async void OpenAsync()
        {
            Sprite sp = await abManager.LoadResourceAsync<Sprite>("a_png", "a");
            Debug.Log("spppp");

        }
    }
}