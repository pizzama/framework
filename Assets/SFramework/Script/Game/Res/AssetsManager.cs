using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using SFramework.Tools;

namespace SFramework
{
    public class AssetsManager
    {
        private static AssetsManager _instance;
        public static AssetsManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AssetsManager();
                return _instance;
            }
        }

        private Dictionary<string, UnityEngine.Object> _resCache;
        private SMemory<string, string, UnityEngine.Object> _bundleCache;
        private AssetsManager()
        {
            _resCache = new Dictionary<string, UnityEngine.Object>();
            _bundleCache = new SMemory<string, string, UnityEngine.Object>();
        }

        public T LoadFromResources<T>(string path) where T : UnityEngine.Object
        {
            if (_resCache.ContainsKey(path))
            {
                return (T)_resCache[path];
            }
            else
            {
                T t = Resources.Load<T>(path);
                if (t != null)
                {
                    _resCache[path] = t;
                }
                return t;
            }
        }

        public async UniTask<T> LoadFromResourcesAsync<T>(string path) where T : UnityEngine.Object
        {
            if (_resCache.ContainsKey(path))
            {
                return (T)_resCache[path];
            }
            else
            {
                ResourceRequest asyncOperation = Resources.LoadAsync<T>(path);
                T t = await asyncOperation as T;
                if (t != null)
                {
                    _resCache[path] = t;
                }
                return t;
            }
        }

        public T LoadFromBundle<T>(string path) where T : UnityEngine.Object
        {
            int index = path.LastIndexOf("/");
            if(index > 0)
            {
                string abName = path.Substring(0, index);
                string resName = path.Substring(index + 1, path.Length - index - 1);
                return LoadFromBundle<T>(abName, resName);
            }

            return default;
            
        }

        public T LoadFromBundle<T>(string abName, string resName) where T : UnityEngine.Object
        {
            abName = abName.ToLower();
            if (_bundleCache.GetValue(abName, resName))
            {
                return (T)_bundleCache.GetValue(abName, resName);
            }
            else
            {
                T t = ABManager.Instance.LoadResource<T>(abName, resName);
                if (t != null)
                {
                    _bundleCache.SetValue(abName, resName, t);
                }
                return t;
            }
        }

        public T LoadFromBundle<T>(string abName, string resName, string variantName) where T : UnityEngine.Object
        {
            abName = (abName + variantName).ToLower();
            string path = FullPath(abName, resName);

            return LoadFromBundle<T>(path, resName);
        }

        public List<T> LoadFromBundleWithSubResources<T>(string abName) where T : UnityEngine.Object
        {
            abName = abName.ToLower();
            List<T> result = ABManager.Instance.LoadResourceWithSubResource<T>(abName);
            return result;
        }

        public async UniTask<T> LoadFromBundleAsync<T>(string path) where T : UnityEngine.Object
        {
            int index = path.LastIndexOf("/");
            if(index > 0)
            {
                string abName = path.Substring(0, index);
                string resName = path.Substring(index + 1, path.Length - index - 1);
                return await LoadFromBundleAsync<T>(abName, resName);
            }

            return default;
        }


        public async UniTask<T> LoadFromBundleAsync<T>(string abName, string resName, CancellationToken token = default) where T : UnityEngine.Object
        {
            abName = abName.ToLower();
            if (_bundleCache.GetValue(abName, resName))
            {
                return (T)_bundleCache.GetValue(abName, resName);
            }
            else
            {
                T t = await ABManager.Instance.LoadResourceAsync<T>(abName, resName, token);
                if (t != null)
                {
                    _bundleCache.SetValue(abName, resName, t);
                }
                return t;
            }
        }

        public async UniTask<T> LoadFromBundleAsync<T>(string abName, string resName, string variantName, CancellationToken token = default) where T : UnityEngine.Object
        {
            abName = (abName + variantName).ToLower();
            string path = FullPath(abName, resName);

            return await LoadFromBundleAsync<T>(path, resName, token);
        }

        public byte[] LoadData(string path)
        {
            try
            {
                path = ABPathHelper.GetResPathInPersistentOrStream(path);
                return FileTools.ReadFile(path);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
                return null;
            }

        }

        public async UniTask<byte[]> LoadDataAsync(string path)
        {
            try
            {
                path = ABPathHelper.GetResPathInPersistentOrStream(path);
                byte[] result = await FileTools.ReadFileAsync(path);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                return null;
            }
        }

        public string FullPath(string abName, string resName)
        {
            return abName + "$$$" + resName;
        }

        public void SplitPath(string fullPath, out string abName, out string resName)
        {
            string[] arr = fullPath.Split("$$$");
            if (arr.Length != 2)
                throw new DataErrorException("fullPath is error:" + fullPath);
            abName = arr[0];
            resName = arr[1];
        }

    }
}