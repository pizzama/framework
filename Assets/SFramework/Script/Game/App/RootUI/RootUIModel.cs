using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SFramework
{
    public class RootUIModel : SModel
    {
        public override void Open()
        {
            ModelCallback?.Invoke();
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