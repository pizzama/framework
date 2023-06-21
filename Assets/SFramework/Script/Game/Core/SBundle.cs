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
            opening();
        }

        public virtual void Open(BundleParams value = default)
        {
            Open();
        }

        public virtual async void OpenAsync()
        {
            openingAsync();
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

        protected virtual void opening()
        {

        }

        protected virtual async void openingAsync()
        {
            await Task.Yield();
        }

        protected virtual void closing()
        {

        }

        protected virtual async void closingAsync()
        {
            await Task.Yield();
        }

        public virtual void HandleMessage(BundleParams value)
        {

        }
    }
}