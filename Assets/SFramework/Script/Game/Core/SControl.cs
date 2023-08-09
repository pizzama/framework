using System;
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
            init();
            Type classType = this.GetType();
            _model = createBundle<SModel>(classType, "Model");
            _model.Control = this;
            _model.ModelCallback += HandleModelCallback;
            _model.Install();
            _view = createBundle<SView>(classType, "View");
            _view.Control = this;
            _view.Install();
        }

        public override void Open(BundleParams value = default)
        {
            base.Open(value);
            _model.Open(value);
            IsOpen = true;
        }

        public override void Open()
        {
            base.Open();
            OpenAsync().Forget();
            _model.Open();
            _model.OpenAsync().Forget();
        }

        public override void Close()
        {
            CloseAsync().Forget();
            _model.CloseAsync().Forget();
            _model.Close();
            _view.CloseAsync().Forget();
            _view.Close();
            IsOpen = false;
        }

        public override void Uninstall()
        {
            _model.ModelCallback -= HandleModelCallback;
        }

        public void BroadcastMessage(string messageId, string nameSpace, string className, object messageData = null, string alias = "", int sort = 0)
        {
            BundleParams bdParams = new BundleParams()
            {
                MessageId = messageId,
                NameSpace = nameSpace,
                ClassName = className,
                MessageData = messageData,
                Alias = alias,
                MessageSender = this,
                Sort = sort,
            };
            BundleManager.Instance.AddMessageParams(bdParams);
        }

        public override void HandleMessage(BundleParams value)
        {

        }

        public void HandleModelCallback()
        {
            _view.Open();
            _view.OpenAsync().Forget();
        }

        public T GetControl<T>() where T : SControl
        {
            return BundleManager.Instance.GetControl<T>();
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

        public void OpenControl(string nameSpace, string className, object messageData = null, bool isSequence = false, string alias = "", int sort = 0)
        {
            Manager.OpenControl(nameSpace, className, messageData, isSequence, alias, sort);
        }

        protected override void closing()
        {
            base.closing();
            if (_model.OpenParams.OpenType == OpenType.Sequence)
            {
                BundleParams? value = BundleManager.Instance.PopUpOpenParams();
                if (value != null)
                    BundleManager.Instance.OpenControl((BundleParams)value);
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