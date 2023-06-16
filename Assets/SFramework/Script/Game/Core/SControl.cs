using System.Threading.Tasks;
using System;
using UnityEngine;

namespace SFramework
{
    public class SControl : SBundle
    {
        private SModel _model;
        private SView _view;

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

        public override void Open(BundleParams value)
        {
            base.Open(value);
            OpenAsync();
            _model.Open();
            _model.OpenAsync();
        }

        public override void Close()
        {
            CloseAsync();
            _model.CloseAsync();
            _model.Close();
            _view.CloseAsync();
            _view.Close();
        }

        public override void Uninstall()
        {
            _model.ModelCallback -= HandleModelCallback;
        }

        public virtual void BroadcastMessage(string messageId, string nameSpace, string className, object messageData, string alias = "", int sort = 0)
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

        public virtual void OpenControl(string nameSpace, string className, object messageData, bool isSequence, string alias = "", int sort = 0)
        {
            BundleParams bdParams = new BundleParams()
            {
                MessageId = "",
                NameSpace = nameSpace,
                ClassName = className,
                MessageData = messageData,
                Alias = alias,
                MessageSender = this,
                Sort = 0,
            };
            if (isSequence)
            {
                BundleManager.Instance.AddOpenParams(bdParams);
            }
            else
            {
                BundleManager.Instance.OpenControl(bdParams);
            }
        }

        public override void HandleMessage(BundleParams value)
        {

        }

        public void HandleModelCallback()
        {
            _view.Open();
            _view.OpenAsync();
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
            _model.Update();
            _view.Update();
        }

        public override void FixUpdate()
        {
            base.FixUpdate();
            controlFixUpdate();
            _model.FixUpdate();
            _view.FixUpdate();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            controlLastUpdate();
            _model.LateUpdate();
            _view.LateUpdate();
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