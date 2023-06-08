
namespace SFramework
{
    public struct BundleParams
    {
        public string MessageId;
        public string NameSpace;
        public string ClassName;
        public object MessageData;
        private string _alias;
        public string Alias
        {
            get { return _alias == "" ? ClassName : _alias; }
            set { _alias = value; }
        }
        public object MessageSender;
        public int Sort;

        public string BundleFullName
        {
            get { return NameSpace + "." + ClassName; }
        }

    }
}