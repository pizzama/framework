using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace game
{
    public class InputControl: SControl
    {
        protected override void enter()
        {
            Debug.Log("test control enter");
        }

        protected override async void enterAsync()
        {
            Debug.Log("test control enterasync");
            await UniTask.Yield();
        }
    }
}