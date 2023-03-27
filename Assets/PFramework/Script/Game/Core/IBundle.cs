namespace PFramework
{
    public interface IBundle
    {
        void Install();

        void Uninstall();
        void Open();
        void Refresh();
        void Close();

        string GetBundleName();

        void HandleMessage(PMessage message);
        void BroadcastMessage(PMessage message);
    }
}