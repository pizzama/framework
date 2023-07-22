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
            

        }
    }
}