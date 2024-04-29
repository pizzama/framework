using System;
using System.Collections.Generic;
using SFramework.Tools;

namespace SFramework
{
    public class SControl : SBundle
    {
        private SModel _model;
        private SView _view;

        public SView View { get { return _view; } }
        public SModel Model { get { return _model; } }

        public T GetModel<T>() where T : SModel
        {
            return (T)_model;
        }

        public T GetView<T>() where T : SView
        {
            return (T)_view;
        }

        //when register control it will find model and view
        public override void Install()
        {
            base.Install();
            initing();
            Type classType = this.GetType();
            _model = createBundle<SModel>(classType, "Model");
            _model.Control = this;
            _model.ModelCallback = HandleModelCallback;
            _model.Install();
            _view = createBundle<SView>(classType, "View");
            _view.Control = this;
            _view.ViewCallback = HandleViewCallback;
            _view.Install();
        }

        public override void Open(SBundleParams value)
        {
            _model.Open(value);
        }

        public override void Close()
        {
            _model.Close();
            _view.Close();
            IsOpen = false;
        }

        public override void Uninstall()
        {
            _model.ModelCallback = null;
            _view.ViewCallback = null;
        }

        public void SubscribeMessage(string messageId, ISBundle bundle)
        {
            Manager.SubscribeMessage(messageId, bundle);
        }

        public void UnSubscribeMessage(string messageId, ISBundle bundle)
        {
            Manager.UnSubscribeMessage(messageId, bundle);
        }

        public void BroadcastMessage(string messageId, string fullPath, object messageData = null, string alias = "", int sort = 0)
        {
            string nameSpace;
            string className;
            StringTools.PrefixClassName(fullPath, out nameSpace, out className);
            this.BroadcastMessage(messageId, nameSpace, className, messageData, alias, sort);
        }

        public void BroadcastMessage(string messageId, string nameSpace, string className, object messageData = null, string alias = "", int sort = 0, Action<object> callback = null)
        {
            SBundleParams bdParams = new SBundleParams()
            {
                MessageId = messageId,
                NameSpace = nameSpace,
                ClassName = className,
                MessageData = messageData,
                Alias = alias,
                MessageSender = this,
                CallBack = callback,
                Sort = sort,
            };
            SBundleManager.Instance.AddMessageParams(bdParams);
        }

        public override void HandleMessage(SBundleParams value)
        {

        }

        public void HandleModelCallback()
        {
            _view.Open();
        }

        public void HandleViewCallback()
        {
            IsOpen = true;
            this.Open();
        }

        public T GetControl<T>() where T : SControl
        {
            return SBundleManager.Instance.GetControl<T>();
        }

        private T createBundle<T>(Type classType, string name)
        {
            int result = classType.Name.IndexOf("Control");
            if (result > 0)
            {
                //find model
                string modelName = classType.Name.Substring(0, result) + name;
                T mod = ObjectTools.CreateInstance<T>(classType.Namespace, modelName, classType.Assembly.GetName().Name);
                if (mod != null)
                {
                    return mod;
                }
                else
                    throw new NotFoundException($"class {classType.Namespace}.{modelName},{classType.Assembly.GetName().Name} is miss!");
            }
            else
            {
                throw new NotFoundException("this is not SFramework naming rules");
            }
        }

        public override void Update()
        {
            base.Update();
            controlUpdate();
            if (_model != null)
                _model.Update();
            if (_view != null)
                _view.Update();
        }

        public override void FixUpdate()
        {
            base.FixUpdate();
            controlFixUpdate();
            if (_model != null)
                _model.FixUpdate();
            if (_view != null)
                _view.FixUpdate();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            controlLastUpdate();
            if (_model != null)
                _model.LateUpdate();
            if (_view != null)
                _view.LateUpdate();
        }

        public void OpenControl(string fullPath, object messageData = null, bool isSequence = false, string alias = "", int sort = 0)
        {
            Manager.OpenControl(fullPath, messageData, isSequence, alias, sort);
        }

        public void CloseAllControl(List<ISBundle> excludeBundles)
        {
            Manager.CloseAllControl(excludeBundles);
        }

        protected override void closing()
        {
            base.closing();
            if (_model.OpenParams.OpenType == OpenType.Sequence)
            {
                SBundleParams? value = SBundleManager.Instance.PopUpOpenParams();
                if (value != null)
                    SBundleManager.Instance.OpenControl((SBundleParams)value);
            }

        }

        protected virtual void controlUpdate()
        {

        }

        protected virtual void controlFixUpdate()
        {

        }

        protected virtual void controlLastUpdate()
        {

        }
    }
}