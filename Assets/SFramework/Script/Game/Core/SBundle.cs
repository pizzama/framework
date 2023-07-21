using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;

namespace SFramework
{
    public abstract class SBundle : IBundle
    {
        private IManager _manager;
        public IManager Manager { get => _manager; set => _manager = value; }

        public string _aliasName;
        public string AliasName { get => _aliasName; set => _aliasName = value; }

        private bool _isOpen;

        public bool IsOpen { get => _isOpen; set => _isOpen = value; }

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

        public virtual async UniTaskVoid OpenAsync()
        {
            openingAsync().Forget();
            await Task.Yield();
        }

        public virtual void Close()
        {
            closing();
            CloseAsync().Forget();
        }

        public virtual async UniTaskVoid CloseAsync()
        {
            closingAsync().Forget();
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

        protected virtual async UniTaskVoid openingAsync()
        {
            await Task.Yield();
        }

        protected virtual void closing()
        {

        }

        protected virtual async UniTaskVoid closingAsync()
        {
            await Task.Yield();
        }

        public virtual void HandleMessage(BundleParams value)
        {

        }
    }
}