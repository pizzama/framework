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

        protected override async void openingAsync()
        {
            Debug.Log("gamemain model enterasync");
            await GetData("");
        }
    }
}
