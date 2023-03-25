using System;
using System.Reflection;

namespace PFramework
{
    public abstract class PBundle : IBundle
    {
        public virtual void Close()
        {
            throw new NotImplementedException();
        }

        public string GetBundleName()
        {
            //return packagename and classname
            Type classtype = this.GetType();
            return classtype.FullName;
        }

        public virtual void Install()
        {
            throw new NotImplementedException();
        }

        public virtual void Open()
        {
            throw new NotImplementedException();
        }

        public virtual void Refresh()
        {
            throw new NotImplementedException();
        }

        public virtual void Uninstall()
        {
            throw new NotImplementedException();
        }
    }
}