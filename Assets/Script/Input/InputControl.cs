using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace game
{
    public class InputControl : SControl
    {
        protected override void opening()
        {
           TestControl test = GetControl<TestControl>();
           Debug.Log(test.TestFunc());
        }

        protected override async UniTaskVoid openingAsync()
        {
            Debug.Log("test control enterasync");
            await UniTask.Yield();
        }
    }
}