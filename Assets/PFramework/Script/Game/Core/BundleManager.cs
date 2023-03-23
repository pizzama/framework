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

        public void OpenControl(string classpath,  string alias="", params object[] paramss)
        {
            //fix control path
            string nameSpace;
            string className;
            StringTools.PrefixClassName(classpath, out nameSpace, out className);
            string modelName = className;
            if (alias == "")
                alias = className;
            BundleManager manager =  BundleManager.Instance;
            IBundle bd = manager.GetBundle(classpath, alias);
            if (bd == null)
            {
                PControl ctl = ObjectTools.CreateInstance<PControl>(nameSpace, modelName);
                if (ctl == null)
                    throw new NotFoundException($"class {nameSpace}.{modelName} is miss!");
                InstallBundle(ctl, alias);
            }
            else
            {
                bd.Open();
            }

        }
        
    }
}