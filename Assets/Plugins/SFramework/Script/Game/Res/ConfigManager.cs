using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Google.Protobuf;
using UnityEngine;

namespace SFramework
{
    public class ConfigManager
    {
        private static ConfigManager _instance;
        private Dictionary<System.Type, IMessage> _configDict;
        private string _serverUrl = "";
        public string ServerUrl
        {
            get { return _serverUrl; }
            set { _serverUrl = value; }
        }
        private int _timeout = 5;
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
        private Dictionary<string, string> _globalHeaders = new Dictionary<string, string>();
        public static ConfigManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConfigManager();
                return _instance;
            }
        }

        private ConfigManager()
        {
            _configDict = new Dictionary<System.Type, IMessage>();
        }

        public string BuildApiUrl(string endpoint)
        {
            return $"{_serverUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
        }

        public void SetGlobalHeader(string key, string value)
        {
            if (_globalHeaders.ContainsKey(key))
            {
                _globalHeaders[key] = value;
            }
            else
            {
                _globalHeaders.Add(key, value);
            }
        }
        
        // 移除全局头参数的方法
        public void RemoveGlobalHeader(string key)
        {
            if (_globalHeaders.ContainsKey(key))
            {
                _globalHeaders.Remove(key);
            }
        }
        
        // 清除所有全局头参数的方法
        public void ClearGlobalHeaders()
        {
            _globalHeaders.Clear();
        }
        
        // 获取全局头参数的方法
        public Dictionary<string, string> GetGlobalHeaders()
        {
            return new Dictionary<string, string>(_globalHeaders);
        }

        public T GetConfig<T>() where T: IMessage, new()
        {
            IMessage messageInstance = null;
            _configDict.TryGetValue(typeof(T), out messageInstance);
            if(messageInstance != null)
                return (T)messageInstance;

            string name = typeof(T).Name;
            int idx = name.IndexOf("_datas");
            if (idx >= 0)
            {
                name = name.Substring(0, idx);
            }

            byte[] bytes = AssetsManager.Instance.LoadData($"bytes/{name}.bytes");
            messageInstance = GetConfig<T>(bytes);
            _configDict[typeof(T)] = messageInstance;
            return (T)messageInstance;
        }

        public T GetConfig<T>(byte[] bytes) where T: IMessage, new()
        {
            T temp = new T();
            CodedInputStream ss = new CodedInputStream(bytes);
            temp.MergeFrom(ss);
            return temp;
        }

        public async UniTask<T> GetConfigAsync<T>() where T: IMessage, new()
        {
            IMessage messageInstance = null;
            _configDict.TryGetValue(typeof(T), out messageInstance);
            if(messageInstance != null)
                return (T)messageInstance;

            string name = typeof(T).Name;
            int idx = name.IndexOf("_datas");
            if (idx >= 0)
            {
                name = name.Substring(0, idx);
            }

            byte[] bytes = await AssetsManager.Instance.LoadDataAsync($"bytes/{name}.bytes");
            if (bytes == null)
                return default;
            messageInstance = GetConfig<T>(bytes);
            _configDict[typeof(T)] = messageInstance;
            return (T)messageInstance;
        }
    }
}