namespace SFramework
{
    public interface IBundle
    {
        //加载模块
        void Install();
        //卸载模块
        void Uninstall();
        void Open();
        void Open(BundleParams value);
        void Close();
        void Update();
        void FixUpdate();
        void LateUpdate();
        void GetBundleName(out string fullName, out string nameSpace, out string className);

        IManager Manager {get; set;}

        string AliasName {get; set;}
        void HandleMessage(BundleParams value);
    }
}