using System;
using System.Collections.Generic;
using UnityEngine;

namespace PFramework
{
    public class BundleManager
    {
        private static BundleManager _instance;
        public static BundleManager Instance 
        {
            get 
            {
                if(_instance == null)
                {
                    _instance = new BundleManager();
                    _instance.init();
                }
                return _instance;
            }
        }
        private Memory<string, string, IBundle> _bundleMap;
        private void init()
        {
            _bundleMap = new Memory<string, string, IBundle>();
        }

        public IBundle GetBundle(string name)
        {
            IBundle value = _bundleMap.GetValue(name, name);
            return value;
        }

        public IBundle AddBundle(IBundle bundle)
        {
            if(bundle == null)
                throw new NotFoundException("bundle name not be null");
            string name = bundle.GetBundleName();
            _bundleMap.SetValue(name, name, bundle);
            return bundle;
        }

        public IBundle DeleteBundle(string name)
        {
            return _bundleMap.DeleteValue(name, name);
        }

        public IBundle DeleteBundle(IBundle bundle)
        {
            return DeleteBundle(bundle.GetBundleName());
        }

        public void InstallBundle(IBundle bundle)
        {
            IBundle value = AddBundle(bundle);
            value.Install();
        }

        public void UninstallBundle(string name)
        {
            IBundle value = DeleteBundle(name);
            if (value != null)
                value.Uninstall();
        }

        public void UninstallBundle(IBundle bundle)
        {
            UninstallBundle(bundle.GetBundleName());
        }

        public void OpenView(string name,  params object[] paramss)
        {
            string modelName = name + "Model";
            BundleManager manager =  BundleManager.Instance;
            Type tp = ObjectUtils.GetType(modelName);
            PModel pmodel = (PModel)Activator.CreateInstance(tp);
            pmodel.Open();
        }
        
    }
}