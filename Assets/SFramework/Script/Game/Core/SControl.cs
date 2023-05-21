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
            Type classtype = this.GetType();
            _model = createBundle<SModel>(classtype, "Model");
            _model.Control = this;
            _model.ModelCallback += HandleModelCallback;
            _model.Install();
            _view = createBundle<SView>(classtype, "View");
            _view.Control = this;
            _view.Install();
        }

        public override void Open()
        {
            base.Open();
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

        public override void BroadcastMessage(string messageId, string nameSpace, string className, object messageData, string alias, object messageSender)
        {
            string bundleFullName = nameSpace + "." + className;
            if (alias == "")
                alias = className;
            IBundle control = BundleManager.Instance.GetBundle(bundleFullName, alias);
            if (control != null)
                control.HandleMessage(messageId, messageData, messageSender);
            else
                Debug.LogWarning($"not found broadcast target{nameSpace}.{className}");
        }

        public override void HandleMessage(string messageId, object messageData, object messageSender)
        {

        }

        public void HandleModelCallback()
        {
            _view.Open();
            _view.OpenAsync();
        }

        private T createBundle<T>(Type classtype, string name)
        {
            int result = classtype.Name.IndexOf("Control");
            if (result > 0)
            {
                //find model
                string modelName = classtype.Name.Substring(0, result) + name;
                T mod = ObjectTools.CreateInstance<T>(classtype.Namespace, modelName, classtype.Assembly.GetName().Name);
                if (mod != null)
                {
                    return mod;
                }
                else
                    throw new NotFoundException($"class {classtype.Namespace}.{modelName},{classtype.Assembly.GetName().Name} is miss!");
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