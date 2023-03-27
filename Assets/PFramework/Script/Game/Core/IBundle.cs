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

        void HandleMessage(string messageId, object messageData, object messageSender);
        void BroadcastMessage(string messageId, object messageData, object messageSender);
    }
}