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

        public T GetModel<T>() where T: SModel
        {
            return (T)Control.Model;
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

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            viewFixedUpdate();

        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            viewLastUpdate();
        }

        public void SendMessage(string key, object messageData = null)
        {
            SBundleParams param = new SBundleParams();
            param.MessageId = key;
            param.MessageData = messageData;
            param.Alias = "selfview";
            Control.HandleMessage(param);
        }

        protected virtual void viewUpdate()
        {

        }

        protected virtual void viewFixedUpdate()
        {

        }

        protected virtual void viewLastUpdate()
        {

        }

    }
}