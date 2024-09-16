namespace SFramework
{
    public interface ISBundle
    {
        //加载模块
        void Install();
        //卸载模块
        void Uninstall();
        void Open();
        void Open(SBundleParams value);
        void Close();
        void Update();
        void FixedUpdate();
        void LateUpdate();
        void GetBundleName(out string fullName, out string nameSpace, out string className);

        ISManager Manager {get; set;}

        string AliasName {get; set;}
        string FullName {get; set;}
        string NameSpace {get; set;}
        string ClassName {get; set;}
        void HandleMessage(SBundleParams value);
        bool IsOpen {get; set;}
    }
}