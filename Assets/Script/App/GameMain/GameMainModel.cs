using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;
namespace Game
{
    public class GameMainModel : SModel
    {
        protected override void opening()
        {
            GetData().Forget();   
        }
    }
}
