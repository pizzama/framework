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
        public bool UseCache; 
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
            set { StringTools.PrefixClassName(value, out this.NameSpace, out this.ClassName); }
        }

        public string PrimaryKey()
        {
            if(MessageData == null)
                return $"{NameSpace}.{ClassName}.{Alias}.{MessageId}";
            else
                return $"{NameSpace}.{ClassName}.{Alias}.{MessageId}.{MessageSender.ToString()}";
        }

        public bool Equals(SBundleParams other)
        {
            string aKey = PrimaryKey();
            string bKey = other.PrimaryKey();
            if(aKey == bKey)
            {
                return true;
            }

            return false;
        }
    }
}
