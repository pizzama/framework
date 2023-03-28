namespace PFramework
{
    public interface IBundle
    {
        void Install();

        void Uninstall();
        void Open();
        void Close();
        void Update();
        void FixUpdate();
        void LateUpdate();
        string GetBundleName();

        IManager Manager {get; set;}

        void HandleMessage(string messageId, object messageData, object messageSender);
        void BroadcastMessage(string messageId, string nameSpace, string className, object messageData, string alias, object messageSender);
    }
}