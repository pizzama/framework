using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace game
{   
    public class GameMainControl : SControl
    {
        protected override void opening()
        {
            Debug.Log("gamemain control enter");
        }

        protected override async void openingAsync()
        {
            Debug.Log("gamemain control enterasync");
            await UniTask.Yield();
        }
    }
}