using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace game
{
    public class LoadingControl : SControl
    {
        protected override void opening()
        {
        }

        protected override async UniTaskVoid openingAsync()
        {
            await UniTask.Yield();
        }

        public override void HandleMessage(BundleParams value)
        {
            switch(value.MessageId)
            {
                case "LoadingUpdate":
                    Debug.Log("loading value:" + value.MessageData);
                break;
                case "LoadingEnd":
                    Debug.Log("loading end:" + value.MessageData);
                break;
            }

        }
    }
}