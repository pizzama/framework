using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;

namespace SFramework
{
    public abstract class SBundle : ISBundle
    {
        private ISManager _manager;
        public ISManager Manager { get => _manager; set => _manager = value; }

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

        public virtual void Open(SBundleParams value = default)
        {
            Open();
        }

        public virtual void Close()
        {
            closing();
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

        protected virtual void initing()
        {

        }

        protected virtual void opening()
        {

        }

        protected virtual void closing()
        {

        }

        public virtual void HandleMessage(SBundleParams value)
        {

        }
    }
}