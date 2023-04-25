using System.Collections.Generic;
using UnityEngine;

namespace PFramework
{
    public class RootUIView : PView
    {
        private Dictionary<UILayer, Transform> _rootUIDic;

        protected override void initPView()
        {
            const string uiname = "PUIROOT";
            bool hasUI = GameObject.Find(uiname);
            if (!hasUI)
            {
                var uirootPrefab = Resources.Load<GameObject>(uiname);
                if(!uirootPrefab)
                {
                    throw new NotFoundException(uiname);
                }
                var uiroot = Object.Instantiate(uirootPrefab);
                Object.DontDestroyOnLoad(uiroot);
            }
        }
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