using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SFramework.Game
{
    public abstract class RootView : SView
    {
        protected abstract ViewOpenType GetViewOpenType();
        protected AssetsManager assetManager;

        public SUIROOT UIRoot { get { return SUIROOT.Instance; } }

        public override void Install()
        {
            assetManager = AssetsManager.Instance;
            //init ui
            base.Install();
        }
    }
}