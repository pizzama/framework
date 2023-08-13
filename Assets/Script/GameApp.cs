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
            List<Type> controls = ReflectionTools.GetTypesFormTypeWithAllAssembly(typeof(SControl));
            for (int i = 0; i < controls.Count; i++)
            {
                Type cType = controls[i];
                IBundle bd = (IBundle)Activator.CreateInstance(cType, true);
                BundleManager.Instance.InstallBundle(bd, "");
            }

            //BundleManager.Instance.OpenControl("game", "GameMainControl");
            BundleManager.Instance.OpenControl("Game", "TestControl");
            BundleManager.Instance.OpenControl("Game", "InputControl");
        }
    }
}