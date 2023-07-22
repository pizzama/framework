using SFramework;
using UnityEngine;
using LanguageData;
using Cysharp.Threading.Tasks;

namespace game
{
    public class LoadingModel : RootModel
    {
        protected override void opening()
        {
        }

        protected override async UniTaskVoid openingAsync()
        {
            await GetData("");
        }
    }
}
