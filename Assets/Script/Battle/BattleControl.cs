using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace game
{
    public class BattleControl : SControl
    {
        protected override void opening()
        {
            Debug.Log("test control enter");
        }

        protected override async UniTaskVoid openingAsync()
        {
            Debug.Log("test control enterasync");
            await UniTask.Yield();
        }
    }
}