using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace game
{   
    public class GameMainControl : SControl
    {
        protected override void opening()
        {
            Debug.Log("gameMain control enter");
        }

        protected override async UniTaskVoid openingAsync()
        {
            Debug.Log("gameMain control openingAsync");
            await UniTask.Yield();
        }
    }
}