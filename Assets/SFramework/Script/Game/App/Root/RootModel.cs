using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Google.Protobuf;
using NativeHelper;
using UnityEngine;
using UnityEngine.Networking;

namespace SFramework.Game
{
    public class RootModel : SModel
    {
        protected ConfigManager configManager;
        private INativeHelper _nativeHelper = NativeHelperFactory.Create();
        public override void Install()
        {
            configManager = ConfigManager.Instance;
            base.Install();
        }

        protected void SaveData<T>(T data, string fileName)
            where T : Google.Protobuf.IMessage
        {
            try
            {

                string fileFullPath = _nativeHelper.GetApplicationPersistentDataPath() + "/" + fileName;
                byte[] bytes = GetBytesFromProtoObject(data);
                FileStream stream = null;

                if (File.Exists(fileFullPath))
                {
                    stream = new FileStream(fileFullPath, FileMode.Open);
                }
                else
                {
                    stream = new FileStream(fileFullPath, FileMode.Create);
                }

                stream.Write(bytes);
                stream.Close();
                _nativeHelper.SyncDB();

            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        public T ReadData<T>(string fileName)
            where T : Google.Protobuf.IMessage, new()
        {
            try
            {

                string fileFullPath = _nativeHelper.GetApplicationPersistentDataPath() + fileName;
                _nativeHelper.SyncDB();
                if (File.Exists(fileFullPath))
                {
                    FileStream stream = new FileStream(fileFullPath, FileMode.Open);
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    stream.Close();
                    T dt = GetProtobufObjectFromBytes<T>(bytes);
                    return dt;
                }
            }
            catch (System.Exception)
            {
                return new T();
            }

            return new T();
        }

        public T ReadData<T>()
            where T : Google.Protobuf.IMessage, new()
        {
            Type tp = typeof(T);
            return ReadData<T>(tp.FullName + ".bytes");
        }

        public void SaveData<T>(T data)
            where T : Google.Protobuf.IMessage, new()
        {
            Type tp = typeof(T);
            SaveData(data, tp.FullName + ".bytes");
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
    }
}
