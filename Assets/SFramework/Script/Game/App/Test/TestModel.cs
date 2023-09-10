using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace Game
{
    public class TestModel : RootModel
    {
        protected override void opening()
        {
            GetData("").Forget();
        }
    }
}
