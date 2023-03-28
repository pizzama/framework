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

        public virtual void BroadcastMessage(string messageId, string nameSpace, string className, object messageData, string alias, object messageSender)
        {
            throw new NotImplementedException();
        }

        public virtual void HandleMessage(string messageId, object messageData, object messageSender)
        {
            throw new NotImplementedException();
        }

        public virtual void Install()
        {
            throw new NotImplementedException();
        }

        public virtual void Open()
        {
            throw new NotImplementedException();
        }

        public virtual void Uninstall()
        {
            throw new NotImplementedException();
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }

        public virtual void FixUpdate()
        {
            throw new NotImplementedException();
        }

        public virtual void LateUpdate()
        {
            throw new NotImplementedException();
        }
    }
}