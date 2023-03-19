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

        public void AddBundle(string name, IBundle bundle)
        {
            if(bundle == null)
                throw new NotFoundException("bundle name not be null:" + name);
            IBundle value;
            _bundleMap.TryGetValue(name, out value);
            if(value != null)
                throw new AlreadyHaveException("bundle name not be null:" + name);
            _bundleMap[name] = bundle;
        }

        public void DeleteBundle(string name)
        {
            IBundle value;
            _bundleMap.TryGetValue(name, out value);
            if(value != null)
                _bundleMap.Remove(name);
        }

        public void InstallBundle(string name, IBundle bundle)
        {
            AddBundle(name, bundle);
            bundle.Open();
        }


        
    }
}