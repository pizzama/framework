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