using SFramework;
using UnityEngine;

namespace game
{
    public class InputModel : RootModel
    {
        protected override void enter()
        {
            Debug.Log("test model enter");
        }

        protected override async void enterAsync()
        {
            Debug.Log("test model enterasync");
            await GetData("");
        }
    }
}
