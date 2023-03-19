using System;
using System.Collections.Generic;
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
        private Dictionary<String, IBundle> _bundleMap;
        private void init()
        {
            _bundleMap = new Dictionary<String, IBundle>();
        }

        public void AddBundle(IBundle bundle)
        {
            if(bundle == null)
                throw new NotFoundException("bundle name not be null");
            string name = bundle.GetBundleName();
            IBundle value;
            _bundleMap.TryGetValue(name, out value);
            if(value != null)
                throw new AlreadyHaveException("bundle name not be null:" + name);
            _bundleMap[name] = bundle;
        }

        public IBundle DeleteBundle(string name)
        {
            IBundle value;
            _bundleMap.TryGetValue(name, out value);
            if(value != null)
                _bundleMap.Remove(name);
            return value;
        }

        public IBundle DeleteBundle(IBundle bundle)
        {
            return DeleteBundle(bundle.GetBundleName());
        }

        public void InstallBundle(IBundle bundle)
        {
            AddBundle(bundle);
            bundle.Open();
        }

        public void UninstallBundle(string name)
        {
            IBundle value = DeleteBundle(name);
            if (value != null)
                value.Close();
        }

        public void UninstallBundle(IBundle bundle)
        {
            UninstallBundle(bundle.GetBundleName());
        }

        public List<IBundle> GetAllBundle()
        {
            return null;
        }
        
    }
}