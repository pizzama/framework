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

        private Dictionary<string, UnityEngine.Object> _cache;
        private AssetsManager()
        {
            _cache = new Dictionary<string, UnityEngine.Object>();
        }

        public T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            if (_cache.ContainsKey(path))
            {
                return (T)_cache[path];
            }
            else
            {
                T t = Resources.Load<T>(path);
                if (t != null)
                {
                    _cache[path] = t;
                }
                return t;
            }
        }

        public async UniTask<T> LoadResourceAsync<T>(string path) where T : UnityEngine.Object
        {
            if (_cache.ContainsKey(path))
            {
                return (T)_cache[path];
            }
            else
            {
                ResourceRequest asyncOperation = Resources.LoadAsync<T>(path);
                T t = await asyncOperation as T;
                if (t != null)
                {
                    _cache[path] = t;
                }
                return t;
            }
        }

        public T LoadResource<T>(string abName, string resName, string variantName = "sf") where T : UnityEngine.Object
        {
            abName = (abName + variantName).ToLower();
            string path = FullPath(abName, resName);
            if (_cache.ContainsKey(path))
            {
                return (T)_cache[path];
            }
            else
            {
                T t = ABManager.Instance.LoadResource<T>(abName, resName);
                if (t != null)
                {
                    _cache[path] = t;
                }
                return t;
            }
        }

        public async UniTask<T> LoadResourceAsync<T>(string abName, string resName, string variantName = "sf", CancellationToken token = default) where T : UnityEngine.Object
        {
            abName = (abName + variantName).ToLower();
            string path = FullPath(abName, resName);
            if (_cache.ContainsKey(path))
            {
                return (T)_cache[path];
            }
            else
            {
                T t = await ABManager.Instance.LoadResourceAsync<T>(abName, resName, token);
                if (t != null)
                {
                    _cache[path] = t;
                }
                return t;
            }
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