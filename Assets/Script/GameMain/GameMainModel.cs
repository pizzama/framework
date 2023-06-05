using SFramework;
using UnityEngine;
namespace game
{
    public class GameMainModel : SModel
    {
        protected override void enter()
        {
            Debug.Log("gamemain model enter");
        }

        protected override async void enterAsync()
        {
            Debug.Log("gamemain model enterasync");
            await GetData("");
        }
    }
}
