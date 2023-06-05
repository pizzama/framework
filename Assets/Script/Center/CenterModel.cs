using AccountHeadIconEvent;
using SFramework;
using UnityEngine;

namespace game
{
    public class CenterModel : RootModel
    {
        protected override void enter()
        {
            Debug.Log("test model enter");
            GetConfig("AccountHeadIcon_event");
        }

        protected override async void enterAsync()
        {
            Debug.Log("test model enterasync");
            for (int i = 0; i < 100; i++)
            {
                AccountHeadIcon_event_datas tt = await ConfigManager.Instance.GetConfigAsync<AccountHeadIcon_event_datas>();
                Debug.Log(tt.ToString());
            }
            await GetData("");
        }
    }
}
