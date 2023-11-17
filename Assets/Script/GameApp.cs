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
            if (!scene.name.Equals("Persistent"))
            {
                SceneManager.LoadScene("Persistent");
            }
        }

        protected override void installBundle()
        {
            var ioc = new IOCFactory();
            ioc.Register<SkillStrike>("test");
            var ii = ioc.Resolve<SkillStrike>("test");
            ii.Create(new SEntity(), "11");
            Debug.Log(ii);
            // BundleManager.Instance.InstallBundle(new GameMainControl(), "", true);
            initAllControl();
            //BundleManager.Instance.OpenControl(SFStaticsControl.Game_GameMainControl);
            SBundleManager.Instance.OpenControl(SFStaticsControl.Game_App_Battle_BattleControl);
        }
    }
}