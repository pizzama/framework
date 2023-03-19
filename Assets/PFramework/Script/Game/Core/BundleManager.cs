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
        private PMemory<string, string, IBundle> _bundleMap;
        private void init()
        {
            _bundleMap = new PMemory<string, string, IBundle>();
        }

        public IBundle GetBundle(string name, string alias)
        {
            IBundle value = _bundleMap.GetValue(name, name);
            return value;
        }

        public IBundle AddBundle(IBundle bundle, string alias)
        {
            if(bundle == null)
                throw new NotFoundException("bundle name not be null");
            string name = bundle.GetBundleName();
            _bundleMap.SetValue(name, alias, bundle);
            return bundle;
        }

        public IBundle DeleteBundle(string name, string alias)
        {
            return _bundleMap.DeleteValue(name, alias);
        }

        public IBundle DeleteBundle(IBundle bundle, string alias)
        {
            return DeleteBundle(bundle.GetBundleName(), alias);
        }

        public void InstallBundle(IBundle bundle, string alias)
        {
            IBundle value = AddBundle(bundle, alias);
            value.Install();
        }

        public void UninstallBundle(string name, string alias)
        {
            IBundle value = DeleteBundle(name, alias);
            if (value != null)
                value.Uninstall();
        }

        public void UninstallBundle(IBundle bundle, string alias)
        {
            UninstallBundle(bundle.GetBundleName(), alias);
        }

        public void OpenView(string name,  string alias="", params object[] paramss)
        {
            string modelName = name + "Model";
            if (alias == "")
                alias = name;
            BundleManager manager =  BundleManager.Instance;
            IBundle bd = manager.GetBundle(name, alias);
            if (bd == null)
            {
                Type tp = ObjectTools.GetType(modelName);
                PModel pmodel = (PModel)Activator.CreateInstance(tp);
                InstallBundle(pmodel, alias);
            }
            

        }
        
    }
}