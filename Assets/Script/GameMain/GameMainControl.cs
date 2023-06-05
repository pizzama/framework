using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace game
{   
    public class GameMainControl : SControl
    {
        protected override void enter()
        {
            Debug.Log("gamemain control enter");
        }

        protected override async void enterAsync()
        {
            Debug.Log("gamemain control enterasync");
            await UniTask.Yield();
        }
    }
}