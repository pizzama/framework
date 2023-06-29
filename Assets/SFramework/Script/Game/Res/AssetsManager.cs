using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

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

        public T LoadResource<T>(string abName, string resName) where T : UnityEngine.Object
        {
            abName = abName.ToLower();
            string path = abName + "_" + resName;
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

        public async UniTask<T> LoadResourceAsync<T>(string abName, string resName, CancellationToken token = default) where T : UnityEngine.Object
        {
            abName = abName.ToLower();
            string path = abName + "_" + resName;
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

    }
}