using SFramework;
namespace game
{
    public class TestModel : SModel
    {
        public override async void OpenAsync()
        {
            await GetData("");
        }
    }
}
