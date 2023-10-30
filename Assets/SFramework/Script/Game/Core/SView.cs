using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace SFramework
{
    public enum ViewOpenType
    {
        Single,
        Multi
    }

    public abstract class SView : SBundle
    {
        private SControl _control;
        public SControl Control
        {
            set { _control = value; }
            get { return _control; }
        }

        public T GetControl<T>() where T : SControl
        {
            return (T)_control;
        }

        public delegate void DelegateViewCallback();
        public DelegateViewCallback ViewCallback;

        public override void Install()
        {
            initing();
        }

        public override void Update()
        {
            base.Update();
            viewUpdate();

        }

        public override void FixUpdate()
        {
            base.FixUpdate();
            viewFixUpdate();

        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            viewLastUpdate();

        }

        protected virtual void viewUpdate()
        {

        }

        protected virtual void viewFixUpdate()
        {

        }

        protected virtual void viewLastUpdate()
        {

        }

    }
}