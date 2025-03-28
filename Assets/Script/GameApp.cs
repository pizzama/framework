using System;
using SFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using SFramework.Statics;
 

namespace Game
{
    public class GameApp : GameLauncher
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)] //not load unity logo
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (!scene.name.Equals("Loading"))
            {
                SceneManager.LoadScene("Loading");
            }
            
            Debug.unityLogger.logEnabled = true; // whether close log
            Application.logMessageReceivedThreaded += logMessageCallback;
        }

        protected override void installBundle()
        {
            try
            {
                //var ioc = new IOCFactory();
                //ioc.Register<SkillStrike>("test");
                //var ii = ioc.Resolve<ISSkillScript>("test");
                //ii.Create(null, "11");
                //Debug.Log(ii);
                // BundleManager.Instance.InstallBundle(new GameMainControl(), "", true);
                initAllControl();
                //BundleManager.Instance.OpenControl(SFStaticsControl.Game_GameMainControl);
                // SBundleManager.Instance.OpenControl(SFStaticsControl.App_Farm_FarmControl);
                // SBundleManager.Instance.OpenControl(SFStaticsControl.App_Inventory_InventoryControl);
            }
            catch (System.Exception err)
            {
                Debug.LogError(err.ToString());
            }

        }

        private static void logMessageCallback(string condition, string stacktrace, LogType type)
        {
            if (type == LogType.Exception || type == LogType.Assert || type == LogType.Error)
            {
                //统计收集所有的错误信息。
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= logMessageCallback;
        }
    }
}