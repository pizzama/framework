using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Google.Protobuf;

namespace SFramework.Game
{
    public class RootModel : SModel
    {
        protected AssetsManager assetManager;
        protected ConfigManager configManager;

        public override void Install()
        {
            assetManager = AssetsManager.Instance;
            configManager = ConfigManager.Instance;
            base.Install();
        }
    }
}