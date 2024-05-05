using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Google.Protobuf;
using System.IO;
using System.Xml;

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

        protected void SaveData<T>(T data, string fileName) where T : Google.Protobuf.IMessage
        {
            try
            {
                byte[] bytes = GetBytesFromProtoObject(data);
                FileStream stream = null;
                string fileFullPath = Application.persistentDataPath + "/" + fileName;
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
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        public T ReadData<T>(string fileName) where T : Google.Protobuf.IMessage, new()
        {
            try
            {
                string fileFullPath = Application.persistentDataPath + "/" + fileName;
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
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

            return default;

        }

        public void SaveData<T>(T data) where T : Google.Protobuf.IMessage, new()
        {
            Type tp = typeof(T);
            SaveData(data, tp.FullName + ".bytes");
        }

        public T ReadData<T>() where T :Google.Protobuf.IMessage, new()
        {
            Type tp = typeof(T);
            return ReadData<T>(tp.FullName + ".bytes");
        }

        private byte[] GetBytesFromProtoObject(Google.Protobuf.IMessage msg)
        {
            using (MemoryStream sndms = new MemoryStream())
            {
                Google.Protobuf.CodedOutputStream cos = new Google.Protobuf.CodedOutputStream(sndms);
                cos.WriteMessage(msg);
                cos.Flush();
                return sndms.ToArray();
            }
        }

        private T GetProtobufObjectFromBytes<T>(byte[] bytes) where T : Google.Protobuf.IMessage, new()
        {
            Google.Protobuf.CodedInputStream cis = new Google.Protobuf.CodedInputStream(bytes);
            T msg = new T();
            cis.ReadMessage(msg);
            return msg;
        }
    }


}