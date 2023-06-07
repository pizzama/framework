using SFramework;
using UnityEngine;
using LanguageData;

namespace game
{
    public class TestModel : RootModel
    {
        protected override void enter()
        {
            Debug.Log("test model enter");
        }

        protected override async void enterAsync()
        {
            Debug.Log("test model enterasync");
            Language_data_datas tt = await ConfigManager.Instance.GetConfigAsync<Language_data_datas>();
            Language_data data = null;
            tt.Datamap.TryGetValue("24", out data);
            Debug.Log(tt.ToString());
            await GetData("");
        }
    }
}
