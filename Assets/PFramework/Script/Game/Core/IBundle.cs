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
        void GetBundleName(out string fullName, out string nameSpace, out string className);

        IManager Manager {get; set;}

        string AliasName {get; set;}

        void HandleMessage(string messageId, object messageData, object messageSender);
        void BroadcastMessage(string messageId, string nameSpace, string className, object messageData, string alias, object messageSender);
    }
}