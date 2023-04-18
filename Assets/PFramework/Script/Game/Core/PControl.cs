using System;
using UnityEngine;

namespace PFramework
{
    public class PControl : PBundle
    {
        private PModel _model;
        private PView _view;

        public override void Close()
        {
            throw new NotImplementedException();
        }

        //when register control it will find model and view
        public override void Install()
        {
            Type classtype = this.GetType();
            _model = createBundle<PModel>(classtype, "Model");
            _model.Control = this;
            _model.ModelCallback += HandleModelCallback;
            _model.Install();
            _view = createBundle<PView>(classtype, "View");
            _view.Control = this;
            _view.Install();
        }

        public override void Open()
        {
            OpenAsync();
            _model.Open();
            _model.OpenAsync();
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
            IBundle control =  BundleManager.Instance.GetBundle(bundleFullName, alias);
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
        }

        private T createBundle<T>(Type classtype, string name)
        {
            int result = classtype.Name.IndexOf("Control");
            if(result > 0)
            {
                //find model
                string modelName = classtype.Name.Substring(0, result) + name;
                T mod = ObjectTools.CreateInstance<T>(classtype.Namespace, modelName, classtype.Assembly.GetName().Name);
                if(mod != null)
                {
                    return mod;
                }
                else
                    throw new NotFoundException($"class {classtype.Namespace}.{modelName},{classtype.Assembly.GetName().Name} is miss!");
            }
            else
            {
                throw new NotFoundException("this is not pframework naming rules");
            }
        }
    }
}