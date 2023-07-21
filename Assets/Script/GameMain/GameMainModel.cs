using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;
namespace game
{
    public class GameMainModel : SModel
    {
        protected override void opening()
        {
            Debug.Log("gamemain model enter");
        }

        protected override async UniTaskVoid openingAsync()
        {
            await GetData("");
        }
    }
}
