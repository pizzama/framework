using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PFramework
{
    public class RootUIModel : PModel
    {
        public override void Open()
        { 
        }

        public override async void OpenAsync()
        {
            await GetData("");
            Debug.Log("hahah1" + Time.time);
            await UniTask.Delay(TimeSpan.FromSeconds(5), ignoreTimeScale: false);
            Debug.Log("aaaa" + Time.time);
            await UniTask.Yield();
        }
    }
}