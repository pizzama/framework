using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Google.Protobuf;

namespace SFramework
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

        // public override async void OpenAsync()
        // {
        // Debug.Log("hahah1" + Time.time);
        // await UniTask.Delay(TimeSpan.FromSeconds(5), ignoreTimeScale: false);
        // Debug.Log("aaaa" + Time.time);
        // Debug.Log("aaaa1111" + Time.time);
        // await UniTask.Yield();
        // }
    }
}