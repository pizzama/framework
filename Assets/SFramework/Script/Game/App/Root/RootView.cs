using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SFramework
{
    public abstract class RootView : SView
    {
        protected abstract ViewOpenType GetViewOpenType();
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