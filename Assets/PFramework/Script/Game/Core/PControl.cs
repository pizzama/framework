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
            PModel mod = createBundle<PModel>(classtype, "Model");
            mod.Install();
            PView view = createBundle<PView>(classtype, "View");
            view.Install();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        public override void Uninstall()
        {
            throw new NotImplementedException();
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