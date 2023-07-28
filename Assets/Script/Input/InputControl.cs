using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace game
{
    public class InputControl : SControl
    {
        protected override void opening()
        {
            BundleManager.Instance.GetControl<InputControl>();
        }

        protected override async UniTaskVoid openingAsync()
        {
            Debug.Log("test control enterasync");
            await UniTask.Yield();
        }
    }
}