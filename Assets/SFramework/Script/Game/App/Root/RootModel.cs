using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Google.Protobuf;

namespace SFramework.Game
{
    public class RootModel : SModel
    {
        protected ConfigManager configManager;

        public override void Install()
        {
            configManager = ConfigManager.Instance;
            base.Install();
        }
    }
}