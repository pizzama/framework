using UnityEngine;

namespace PFramework
{
    public class RootUIView : PView
    {
        private Canvas _tags;
        private Canvas _pend;
        private Canvas _hud;
        private Canvas _popUp;
        private Canvas _toast;
        private Canvas _blocker;
        public override void Open()
        {
            
            
        }
        public override async void OpenAsync()
        {
            Sprite sp = await abManager.LoadResourceAsync<Sprite>("a_png", "a");
            Debug.Log("spppp");
        }
    }
}