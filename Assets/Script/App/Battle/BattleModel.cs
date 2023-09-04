using SFramework.Game;
using UnityEngine;

namespace Game
{
    public class BattleModel : RootModel
    {
        protected override void opening()
        {
            Debug.Log("test model enter");
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
