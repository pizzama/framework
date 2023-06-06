using SFramework;

namespace game
{
    public class GameApp : GameLaucher
    {
        protected override void installBundle()
        {
            BundleManager.Instance.InstallBundle(new GameMainControl(), "", true);
            BundleManager.Instance.InstallBundle(new TestControl(), "", true);
            // BundleManager.Instance.InstallBundle(new CenterControl(), "", true);
        }
    }
}