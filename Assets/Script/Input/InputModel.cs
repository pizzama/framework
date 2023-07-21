using Cysharp.Threading.Tasks;
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

        protected override async UniTaskVoid openingAsync()
        {
            await GetData("");
        }
    }
}
