using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using NativeHelper;
using UnityEngine;
using UnityEngine.Networking;

namespace SFramework.Game
{
    public class RootModel : SModel
    {
        protected ConfigManager configManager;
        public override void Install()
        {
            configManager = ConfigManager.Instance;
            base.Install();
        }

        public void SaveData<T>(T data, string fileName)
            where T : Google.Protobuf.IMessage
        {
            try
            {
                byte[] bytes = GetBytesFromProtoObject(data);
                Debug.Log("will save name:" + fileName);
                NativeHelperFactory.Instance.Save(bytes, fileName);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        public void SaveData<T>(T data)
            where T : Google.Protobuf.IMessage, new()
        {
            Type tp = typeof(T);
            SaveData(data, tp.FullName + ".bytes");
        }

        public T ReadData<T>(string fileName)
            where T : Google.Protobuf.IMessage, new()
        {
            try
            {
                byte[] bytes = NativeHelperFactory.Instance.Read(fileName);
                if (bytes != null)
                {
                    T dt = GetProtobufObjectFromBytes<T>(bytes);
                    return dt;
                }

                return new T();
            }
            catch (System.Exception)
            {
                return new T();
            }
        }

        public T ReadData<T>()
            where T : Google.Protobuf.IMessage, new()
        {
            Type tp = typeof(T);
            return ReadData<T>(tp.FullName + ".bytes");
        }

        public async UniTask<T> ReadDataAsync<T>()
            where T : Google.Protobuf.IMessage, new()
        {
            Type tp = typeof(T);
            return await ReadDataAsync<T>(tp.FullName + ".bytes");
        }

        public async UniTask<T> ReadDataAsync<T>(string fileName)
            where T : Google.Protobuf.IMessage, new()
        {
            try
            {
                string fileFullPath = Application.persistentDataPath + "/" + fileName;
                UnityWebRequest request = UnityWebRequest.Get(fileFullPath);
                await request.SendWebRequest();
                if (request.isDone)
                {
                    T dt = GetProtobufObjectFromBytes<T>(request.downloadHandler.data);
                    return dt;
                }
            }
            catch (System.Exception)
            {
                return new T();
            }

            return new T();
        }

        public void Delete<T>() where T : Google.Protobuf.IMessage, new()
        {
            Type tp = typeof(T);
            Delete(tp.FullName + ".bytes");
        }

        public void Delete(string fileName)
        {
           NativeHelperFactory.Instance.Delete(fileName); 
        }

        private byte[] GetBytesFromProtoObject(Google.Protobuf.IMessage msg)
        {
            using (MemoryStream sndms = new MemoryStream())
            {
                Google.Protobuf.CodedOutputStream cos = new Google.Protobuf.CodedOutputStream(
                    sndms
                );
                cos.WriteMessage(msg);
                cos.Flush();
                return sndms.ToArray();
            }
        }

        private T GetProtobufObjectFromBytes<T>(byte[] bytes)
            where T : Google.Protobuf.IMessage, new()
        {
            Google.Protobuf.CodedInputStream cis = new Google.Protobuf.CodedInputStream(bytes);
            T msg = new T();
            cis.ReadMessage(msg);
            return msg;
        }

        public async UniTask<T> GetData<T>(Dictionary<string, string> getParams, IProgress<float> progress, string endpoint = "") where T: new()
        {
            string fullUrl = configManager.ServerUrl;
            if (string.IsNullOrWhiteSpace(endpoint) == false)
            {
                fullUrl = configManager.BuildApiUrl(endpoint);
            }
            Dictionary<string, string> headParams = configManager.GetGlobalHeaders();
            int timeout = configManager.Timeout;
            byte[] bytes = await GetRemoteData(fullUrl, getParams, headParams, progress, timeout, true);
            return BytesToObject<T>(bytes);
        }

        public async UniTask<byte[]> GetData(Dictionary<string, string> getParams, Dictionary<string, string> headParams, IProgress<float> progress, int timeout, string endpoint = "")
        {
            string fullUrl = configManager.ServerUrl;
            if (string.IsNullOrWhiteSpace(endpoint) == false)
            {
                fullUrl = configManager.BuildApiUrl(endpoint);
            }
            return await GetRemoteData(fullUrl, getParams, headParams, progress, timeout, true);
        }

        public async UniTask<T> GetNetData<T>(Dictionary<string, string> getParams, IProgress<float> progress, string endpoint = "") where T: new()
        {
            string fullUrl = configManager.ServerUrl;
            if (string.IsNullOrWhiteSpace(endpoint) == false)
            {
                fullUrl = configManager.BuildApiUrl(endpoint);
            }
            Dictionary<string, string> headParams = configManager.GetGlobalHeaders();
            int timeout = configManager.Timeout;
            byte[] bytes = await GetRemoteData(fullUrl, getParams, headParams, progress, timeout, false);
            return BytesToObject<T>(bytes);
        }

        public async UniTask<byte[]> GetNetData(Dictionary<string, string> getParams, Dictionary<string, string> headParams, IProgress<float> progress, int timeout, string endpoint = "")
        {
            string fullUrl = configManager.ServerUrl;
            if (string.IsNullOrWhiteSpace(endpoint) == false)
            {
                fullUrl = configManager.BuildApiUrl(endpoint);
            }
            return await GetRemoteData(fullUrl, getParams, headParams, progress, timeout, false);
        }

        public async UniTask<T> PostData<T>(object postParams, IProgress<float> progress, string endpoint = "") where T: new()
        {
            return await PostData<T>(postParams, null, progress, endpoint); //TODO: add defaul
        }

        public async UniTask<T> PostData<T>(object postParams, Dictionary<string, string> getParams, IProgress<float> progress, string endpoint = "") where T: new()
        {
            string fullUrl = configManager.ServerUrl;
            if (string.IsNullOrWhiteSpace(endpoint) == false)
            {
                fullUrl = configManager.BuildApiUrl(endpoint);
            }
            Dictionary<string, string> headParams = configManager.GetGlobalHeaders();
            int timeout = configManager.Timeout;
            byte[] bytes = await PostRemoteData(fullUrl, postParams, getParams, headParams, progress, timeout, true);
            return BytesToObject<T>(bytes);
        }

        public async UniTask<byte[]> PostData(object postParams, Dictionary<string, string> getParams, Dictionary<string, string> headParams, IProgress<float> progress, int timeout, string endpoint = "")
        {
            string fullUrl = configManager.ServerUrl;
            if (string.IsNullOrWhiteSpace(endpoint) == false)
            {
                fullUrl = configManager.BuildApiUrl(endpoint);
            }
            return await PostRemoteData(fullUrl, postParams, getParams, headParams, progress, timeout, true);
        }

        public async UniTask<T> PostNetData<T>(object postParams, IProgress<float> progress, string endpoint = "") where T: new()
        {
            return await PostNetData<T>(postParams, null, progress, endpoint); //TODO: add defaul
        }

        public async UniTask<T> PostNetData<T>(object postParams, Dictionary<string, string> getParams, IProgress<float> progress, string endpoint = "") where T: new()
        {
            string fullUrl = configManager.ServerUrl;
            if (string.IsNullOrWhiteSpace(endpoint) == false)
            {
                fullUrl = configManager.BuildApiUrl(endpoint);
            }
            Dictionary<string, string> headParams = configManager.GetGlobalHeaders();
            int timeout = configManager.Timeout;
            byte[] bytes = await PostRemoteData(fullUrl, postParams, getParams, headParams, progress, timeout, false);
            return BytesToObject<T>(bytes);
        }

        public async UniTask<byte[]> PostNetData(object postParams, Dictionary<string, string> getParams, Dictionary<string, string> headParams, IProgress<float> progress, int timeout, string endpoint = "")
        {
            string fullUrl = configManager.ServerUrl;
            if (string.IsNullOrWhiteSpace(endpoint) == false)
            {
                fullUrl = configManager.BuildApiUrl(endpoint);
            }
            return await PostRemoteData(fullUrl, postParams, getParams, headParams, progress, timeout, false);
        }
    }
}
