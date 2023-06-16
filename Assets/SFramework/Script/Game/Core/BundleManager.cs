using System;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework
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

        private List<BundleParams> _params;

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
            if (_bundleMap == null)
                return;
            foreach (KeyValuePair<string, Dictionary<string, IBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, IBundle> bundle in result.Value)
                {
                    bundle.Value.Update();
                }
            }
        }

        private void FixUpdate()
        {
            if (_bundleMap == null)
                return;
            foreach (KeyValuePair<string, Dictionary<string, IBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, IBundle> bundle in result.Value)
                {
                    bundle.Value.FixUpdate();
                }
            }
        }

        private void LateUpdate()
        {
            if (_bundleMap == null)
                return;
            foreach (KeyValuePair<string, Dictionary<string, IBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, IBundle> bundle in result.Value)
                {
                    bundle.Value.LateUpdate();
                }
            }

            executeBundleParams();
        }

        private SMemory<string, string, IBundle> _bundleMap;
        private void init()
        {
            _bundleMap = new SMemory<string, string, IBundle>();
            _params = new List<BundleParams>();
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

        public void InstallBundle(IBundle bundle, string alias = "", bool withOpen = false)
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

        // classPath is nameSpace + className
        public void OpenControl(string classPath, string alias = "", params object[] somParams)
        {
            //fix control path
            string nameSpace;
            string className;
            StringTools.PrefixClassName(classPath, out nameSpace, out className);
            string modelName = className;
            if (alias == "")
                alias = className;
            BundleManager manager = BundleManager.Instance;
            IBundle bd = manager.GetBundle(classPath, alias);
            if (bd == null)
            {
                SControl ctl = ObjectTools.CreateInstance<SControl>(nameSpace, modelName);
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

        public void AddBundleParams(BundleParams value)
        {
            _params.Add(value);
        }

        private void executeBundleParams()
        {
            for (int i = 0; i < _params.Count; i++)
            {
                BundleParams pa = _params[i];
                _params.Remove(pa);
                i--;

                IBundle control = BundleManager.Instance.GetBundle(pa.BundleFullName, pa.Alias);
                if (control != null)
                    control.HandleMessage(pa.MessageId, pa.MessageData, pa.MessageSender);
                else
                    Debug.LogWarning($"not found broadcast target{pa.NameSpace}.{pa.ClassName}");
            }



        }
    }
}