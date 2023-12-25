using Cysharp.Threading.Tasks;
using LanguageData;
using TestEvent;
using SFramework;
using SFramework.Game;
using UnityEngine;

namespace Game.App.Battle
{
    public class BattleModel : RootModel
    {
        public Language_data_datas LanguageConfig;
        public test_event_datas TestConfig;
        protected override void opening()
        {
            getConfig().Forget();
            
        }

        private async UniTaskVoid getConfig()
        {
            var (_languageConfig, _testConfig, _) = await UniTask.WhenAll(
                ConfigManager.Instance.GetConfigAsync<Language_data_datas>(),
                ConfigManager.Instance.GetConfigAsync<test_event_datas>(),
                GetData(""));
            Debug.Log(_languageConfig + ";" + _testConfig);
        }

        //protected override async UniTaskVoid openingAsync()
        //{
        //    Debug.Log("test model enterasync");
        //    Language_data_datas tt = await ConfigManager.Instance.GetConfigAsync<Language_data_datas>();
        //    Language_data data = null;
        //    if (tt != null)
        //    {
        //        tt.Datamap.TryGetValue("24", out data);
        //        Debug.Log(tt.ToString());
        //    }
        //    await GetData("");
        //}
    }
}
