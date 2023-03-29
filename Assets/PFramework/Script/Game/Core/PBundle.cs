using System;
using System.Reflection;

namespace PFramework
{
    public abstract class PBundle : IBundle
    {
        private IManager _manager;
        public IManager Manager { get => _manager; set => _manager = value; }

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
           
        }

        public virtual void FixUpdate()
        {
            
        }

        public virtual void LateUpdate()
        {
            
        }
    }
}