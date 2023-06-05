using System;
using Cysharp.Threading.Tasks;
using AccountHeadIconEvent;
using UnityEngine;
using Google.Protobuf;

namespace SFramework
{
    public class RootModel : SModel
    {
        public static string SCENEPREFIX = "$s$";
        public static string UIPREFIX = "$u$";

        protected AssetsManager assetManager;

        public override void Install()
        {
            assetManager = AssetsManager.Instance;
            base.Install();
        }

        public void GetConfig(string name)
        {
            // byte[] bytes = assetManager.LoadData($"bytes/{name}.bytes");

            // for (int i = 0; i < 100; i++)
            // {
            //     AccountHeadIcon_event_datas tt = ConfigManager.Instance.GetConfig<AccountHeadIcon_event_datas>();
            //     Debug.Log(tt.ToString());
            // }

            // AccountHeadIcon_event_datas tt = new AccountHeadIcon_event_datas();
            // CodedInputStream ss = new CodedInputStream(bytes);
            // tt.MergeFrom(ss);
            // for (int i = 0; i < tt.Datas.Count; i++)
            // {
            //     AccountHeadIcon_event dt = tt.Datas[i];
            //     Debug.Log("aaaaaaaa:" + dt.ToString());
            // }

        }

        // public override async void OpenAsync()
        // {
        // Debug.Log("hahah1" + Time.time);
        // await UniTask.Delay(TimeSpan.FromSeconds(5), ignoreTimeScale: false);
        // Debug.Log("aaaa" + Time.time);
        // Debug.Log("aaaa1111" + Time.time);
        // await UniTask.Yield();
        // }
    }
}