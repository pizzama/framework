using Cysharp.Threading.Tasks;
using SFramework.Game;

namespace Game
{
    public class NormalLoadingModel : RootModel
    {
        protected override void opening()
        {
            GetData().Forget();
        }
    }
}
