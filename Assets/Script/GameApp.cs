using SFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using SFramework.Statics;
using SFramework.Game.SActor.Skill;

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
                SBundleManager.Instance.OpenControl(SFStaticsControl.App_Loading_LoadingControl);
                // SBundleManager.Instance.OpenControl(SFStaticsControl.App_Inventory_InventoryControl);
            }
            catch (System.Exception err)
            {
                Debug.LogError(err.ToString());
            }

        }
    }
}