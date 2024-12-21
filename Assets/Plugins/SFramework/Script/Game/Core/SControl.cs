using System;
using System.Collections.Generic;
using SFramework.Tools;

namespace SFramework
{
    public enum ViewOpenType
    {
        Single,
        Additive,
        SingleNone,
        SingleNeverClose,
    }
    public abstract class SControl : SBundle
    {
        private SModel _model;
        private SView _view;

        public SView View { get { return _view; } }
        public SModel Model { get { return _model; } }

        private Dictionary<string, SBundleParams> _paramsCache;

        public T GetModel<T>() where T : SModel
        {
            return (T)_model;
        }

        public T GetView<T>() where T : SView
        {
            return (T)_view;
        }
        public abstract ViewOpenType GetViewOpenType();

        //when register control it will find model and view
        public override void Install()
        {
            base.Install();
            _paramsCache = new Dictionary<string, SBundleParams>();
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
            IsOpen = true;
            Open();
            _model.Open(value);
        }

        public override void Close()
        {
            if (GetViewOpenType() != ViewOpenType.SingleNeverClose)
            {
                _model.Close();
                _view.Close();
                IsOpen = false;
            }

            base.Close();
        }

        public void CloseControl()
        {
            Manager.CloseControl(this);
        }

        public override void Uninstall()
        {
            _model.ModelCallback = null;
            _view.ViewCallback = null;
            _model.Uninstall();
            _view.Uninstall();
        }

        public void SubscribeBundleMessage(string messageId)
        {
            SubscribeBundleMessage(messageId, this);
        }

        public void SubscribeBundleMessage(string messageId, ISBundle bundle)
        {
            Manager.SubscribeBundleMessage(messageId, bundle);
        }

        public void UnSubscribeBundleMessage(string messageId)
        {
            Manager.UnSubscribeBundleMessage(messageId, this);
        }

        public void UnSubscribeBundleMessage(string messageId, ISBundle bundle)
        {
            Manager.UnSubscribeBundleMessage(messageId, bundle);
        }

        public void BroadcastControlWithOutCache(string messageId, object messageData = null, string fullPath = "",
            Action<object> callback = null, string alias = "", int sort = 0)
        {
            string nameSpace;
            string className;
            StringTools.PrefixClassName(fullPath, out nameSpace, out className);
            this.BroadcastControl(messageId, messageData, nameSpace, className, callback, alias, sort, false);
        }

        public void BroadcastControl(string messageId, object messageData = null, string fullPath = "", Action<object> callback = null, string alias = "", int sort = 0)
        {
            string nameSpace;
            string className;
            StringTools.PrefixClassName(fullPath, out nameSpace, out className);
            this.BroadcastControl(messageId, messageData, nameSpace, className, callback, alias, sort, true);
        }

        public void BroadcastControl(string messageId, object messageData, string nameSpace, string className, Action<object> callback, string alias, int sort, bool useCache)
        {
            string primaryKey = $"{nameSpace}.{className}.{alias}.{messageId}";
            if (useCache)
            {
                if(_paramsCache.ContainsKey(primaryKey))
                {
                    var bdParams = _paramsCache[primaryKey];
                    bdParams.MessageData = messageData;
                    bdParams.CallBack = callback;
                    bdParams.Sort = sort;
                    bdParams.UseCache = true;
                    SBundleManager.Instance.AddMessageParams(bdParams);
                }
                else
                {
                    var bdParams = new SBundleParams()
                    {
                        MessageId = messageId,
                        NameSpace = nameSpace,
                        ClassName = className,
                        MessageData = messageData,
                        Alias = alias,
                        MessageSender = this,
                        CallBack = callback,
                        Sort = sort,
                        UseCache = true,
                    };

                    _paramsCache[primaryKey] = bdParams;

                    SBundleManager.Instance.AddMessageParams(bdParams);
                }
            }
            else
            {
                var bdParams = new SBundleParams()
                {
                    MessageId = messageId,
                    NameSpace = nameSpace,
                    ClassName = className,
                    MessageData = messageData,
                    Alias = alias,
                    MessageSender = this,
                    CallBack = callback,
                    Sort = sort,
                    UseCache = false,
                };
                SBundleManager.Instance.AddMessageParams(bdParams);
            }
        }

        public override void HandleMessage(SBundleParams value)
        {

        }

        public void HandleModelCallback(int code)
        {
            if (code != 0)
            {
                IsOpen = false;
            }
            else
            {
                _view.Open();
            }

        }

        public void HandleViewCallback()
        {
            alreadyOpened();
            _model.OpenParams.CallBack?.Invoke(this);
        }

        public T GetControl<T>() where T : SControl
        {
            T result = SBundleManager.Instance.GetControl<T>();
            if (result.IsOpen)
            {
                return result;
            }

            return null;
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

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            controlFixedUpdate();
            if (_model != null)
                _model.FixedUpdate();
            if (_view != null)
                _view.FixedUpdate();
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

        public void OpenControl(string fullPath, object messageData = null, bool isSequence = false, string alias = "", int sort = 0, Action<Object> callback = null)
        {
            Manager.OpenControl(fullPath, messageData, isSequence, alias, sort, callback);
        }

        public void CloseAllControl(List<ISBundle> excludeBundles)
        {
            Manager.CloseAllControl(excludeBundles);
        }

        public override string ToLog()
        {
            return string.Format(@"{0};{1}", IsOpen.ToString(), GetViewOpenType().ToString());
        }

        protected override void closing()
        {
            base.closing();
            _paramsCache.Clear();
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

        protected virtual void controlFixedUpdate()
        {

        }

        protected virtual void controlLastUpdate()
        {

        }

        protected virtual void alreadyOpened()
        {

        }
    }
}