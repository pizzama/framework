using SFramework;
using UnityEngine;

namespace game
{
    public class InputModel : RootModel
    {
        protected override void opening()
        {
            Debug.Log("test model enter");
        }

        protected override async void openingAsync()
        {
            Debug.Log("test model enterasync");
            await GetData("");
        }
    }
}
