using System;
using SFramework.Tools;

namespace SFramework
{
    public enum OpenType
    {
        None,
        Sequence,
    }

    public struct SBundleParams
    {
        public string MessageId;
        public string NameSpace;
        public string ClassName;
        public object MessageData;
        private string _alias;
        public Action<object> CallBack;

        public string Alias
        {
            get { return _alias == "" ? ClassName : _alias; }
            set { _alias = value; }
        }
        public object MessageSender;
        public int Sort;

        public OpenType OpenType;

        public string ClassPath
        {
            get { return NameSpace + "." + ClassName; }
            set
            {
                StringTools.PrefixClassName(value, out this.NameSpace, out this.ClassName);
            }
        }
    }
}