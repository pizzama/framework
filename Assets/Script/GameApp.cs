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
            initAllControl();
            BundleManager.Instance.OpenControl("Game.GameMainControl");
        }
    }
}