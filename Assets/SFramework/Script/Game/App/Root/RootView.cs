using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SFramework
{
    public class RootView : SView
    {
        protected static Dictionary<string, GameObject> sceneDict;

        protected AssetsManager assetManager;
        protected RootManager rootManager;

        protected SUIROOT uiRoot;

        public override void Install()
        {
            assetManager = AssetsManager.Instance;
            //init ui
            rootManager = RootManager.Instance;
            uiRoot = rootManager.GetUIRoot();
            base.Install();
        }

        public GameObject GetDefineSceneObject(string name)
        {
            return rootManager.GetDefineSceneObject(name);
        }
    }
}