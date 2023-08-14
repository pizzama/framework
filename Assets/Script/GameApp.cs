using System;
using System.Collections.Generic;
using SFramework;
using SFramework.Tools;

namespace Game
{
    public class GameApp : GameLauncher
    {
        protected override void installBundle()
        {
            // BundleManager.Instance.InstallBundle(new GameMainControl(), "", true);
            // BundleManager.Instance.InstallBundle(new TestControl(), "", true);
            // BundleManager.Instance.InstallBundle(new InputControl(), "", true);
            initAllControl();
            //BundleManager.Instance.OpenControl("game", "GameMainControl");
            BundleManager.Instance.OpenControl("Game", "TestControl");
            BundleManager.Instance.OpenControl("Game", "InputControl");
        }
    }
}