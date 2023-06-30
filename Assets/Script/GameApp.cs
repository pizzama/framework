using SFramework;

namespace game
{
    public class GameApp : GameLauncher
    {
        protected override void installBundle()
        {
            BundleManager.Instance.InstallBundle(new GameMainControl(), "", true);
            BundleManager.Instance.InstallBundle(new TestControl(), "", true);
            BundleManager.Instance.InstallBundle(new InputControl(), "", true);
        }
    }
}