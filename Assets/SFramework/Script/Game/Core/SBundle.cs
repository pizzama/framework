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
            //return packageName and className
            Type classType = this.GetType();
            fullName = classType.FullName;
            nameSpace = classType.Namespace;
            className = classType.Name;
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

        public virtual void Open(BundleParams value)
        {
            enter();
        }

        public virtual async void OpenAsync()
        {
            enterAsync();
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

        protected virtual void init()
        {

        }

        protected virtual void enter()
        {

        }

        protected virtual async void enterAsync()
        {
            await Task.Yield();
        }

        public virtual void HandleMessage(BundleParams value)
        {

        }
    }
}