using System;
using System.Collections.Generic;
using UnityEngine;

namespace PFramework
{
    public class BundleManager : MonoBehaviour, IManager
    {
        private static BundleManager _instance;
        public static BundleManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<BundleManager>();
                if (_instance != null) return _instance;
                var obj = new GameObject();
                obj.name = "BundleManager";
                _instance = obj.AddComponent<BundleManager>();
                _instance.init();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (_instance == null)
            {
                _instance = this as BundleManager;
                DontDestroyOnLoad(transform.gameObject);
            }
            else
            {
                if (this != _instance)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        private void Update()
        {
            foreach(KeyValuePair<string, Dictionary<string, IBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, IBundle> bundle in result.Value)
                {
                    bundle.Value.Update();
                }
            }
        }

        private void FixUpdate()
        {
            foreach(KeyValuePair<string, Dictionary<string, IBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, IBundle> bundle in result.Value)
                {
                    bundle.Value.FixUpdate();
                }
            }
        }

        private void LateUpdate()
        {
            foreach(KeyValuePair<string, Dictionary<string, IBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, IBundle> bundle in result.Value)
                {
                    bundle.Value.LateUpdate();
                }
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
            if (bundle == null)
                throw new NotFoundException("bundle name not be null");
            string fullName;
            string className;
            string nameSpace;
            bundle.GetBundleName(out fullName, out nameSpace, out className);
            if (alias == "")
                alias = className;
            bundle.AliasName = alias;
            _bundleMap.SetValue(name, alias, bundle);
            bundle.Manager = this;
            return bundle;
        }

        public IBundle DeleteBundle(string name, string alias)
        {
            return _bundleMap.DeleteValue(name, alias);
        }

        public IBundle DeleteBundle(IBundle bundle)
        {
            string fullName;
            string className;
            string nameSpace;
            bundle.GetBundleName(out fullName, out nameSpace, out className);

            return DeleteBundle(fullName, bundle.AliasName);
        }

        public void InstallBundle(IBundle bundle, string alias="", bool withOpen = false)
        {
            IBundle value = AddBundle(bundle, alias);
            value.Install();
            if (withOpen)
                value.Open();
        }

        public void UninstallBundle(string name, string alias)
        {
            IBundle value = DeleteBundle(name, alias);
            if (value != null)
                value.Uninstall();
        }

        public void UninstallBundle(IBundle bundle)
        {
            string fullName;
            string className;
            string nameSpace;
            bundle.GetBundleName(out fullName, out nameSpace, out className);
            UninstallBundle(fullName, bundle.AliasName);
        }

        public void OpenControl(string classpath, string alias = "", params object[] paramss)
        {
            //fix control path
            string nameSpace;
            string className;
            StringTools.PrefixClassName(classpath, out nameSpace, out className);
            string modelName = className;
            if (alias == "")
                alias = className;
            BundleManager manager = BundleManager.Instance;
            IBundle bd = manager.GetBundle(classpath, alias);
            if (bd == null)
            {
                PControl ctl = ObjectTools.CreateInstance<PControl>(nameSpace, modelName);
                if (ctl == null)
                    throw new NotFoundException($"class {nameSpace}.{modelName} is miss!");
                InstallBundle(ctl, alias);
                ctl.Open();
            }
            else
            {
                bd.Open();
            }
        }
    }
}