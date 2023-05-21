using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    public class RootView : SView
    {
        public static SUIROOT UIROOT;
        private Dictionary<string, GameObject> _sceneDict;
        protected SUIROOT uiRoot;

        public override void Install()
        {
            //init ui
            initUI();
            initScene();
            base.Install();
        }

        private void initUI()
        {
            const string uiname = "SUIROOT";
            GameObject uiroot = GameObject.Find(uiname);
            if (!uiroot)
            {
                var uirootPrefab = Resources.Load<GameObject>(uiname);
                if (!uirootPrefab)
                {
                    throw new NotFoundException(uiname);
                }
                uiroot = Object.Instantiate(uirootPrefab);
                Object.DontDestroyOnLoad(uiroot);
                uiRoot = ComponentTools.GetOrAddComponent<SUIROOT>(uiroot);
                UIROOT = uiRoot;
            }
        }

        private void initScene()
        {
            //init current scene
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                //遍历场景中的GameObject 记录需要的object
                if (obj.name.IndexOf(RootModel.SCENEPREFIX) > 0)
                {
                    _sceneDict[obj.name] = obj;
                }
            }
        }

        public override void Open()
        {

        }

        // public override async void OpenAsync()
        // {
        //     Sprite sp = await abManager.LoadResourceAsync<Sprite>("a_png", "a");
        //     Debug.Log("spppp");
        // }

    }
}