using System;
using SFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameApp : GameLauncher
    {
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
            // BundleManager.Instance.InstallBundle(new GameMainControl(), "", true);
            initAllControl();
            BundleManager.Instance.OpenControl("Game.GameMainControl");
        }
    }
}