using System.Threading.Tasks;
using System;

namespace SFramework
{
    public abstract class SBundle : IBundle
    {
        private IManager _manager;
        public IManager Manager { get => _manager; set => _manager = value; }

        public string _aliasName;
        public string AliasName { get => _aliasName; set => _aliasName = value; }

        public void GetBundleName(out string fullName, out string nameSpace, out string className)
        {
            //return packagename and classname
            Type classtype = this.GetType();
            fullName = classtype.FullName;
            nameSpace = classtype.Namespace;
            className = classtype.Name;
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
        }

        public virtual void Uninstall()
        {
        }

        public virtual void Open()
        {
        }

        public virtual async void OpenAsync()
        {
            await Task.Yield();
        }

        public virtual void Close()
        {
            throw new NotImplementedException();
        }

        public virtual async void CloseAsync()
        {
            await Task.Yield();
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