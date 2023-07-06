using System.Collections.Generic;
using System.Reflection;
using SFramework;

namespace game
{
    public class GameApp : GameLauncher
    {
        protected override void installBundle()
        {
            var ss = ReflectionTools.GetTypesFormBaseTypeWithAllAssembly(typeof(SControl));
            BundleManager.Instance.InstallBundle(new GameMainControl(), "", true);
            BundleManager.Instance.InstallBundle(new TestControl(), "", true);
            BundleManager.Instance.InstallBundle(new InputControl(), "", true);
        }
    }
}