using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Google.Protobuf;
using System.IO;

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
            byte[] bytes = GetBytesFromProtoObject(data);
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);
            stream.Write(bytes);
            stream.Close();
        }

        public T ReadData<T>(string fileName) where T : Google.Protobuf.IMessage, new()
        {
            if (File.Exists(Application.persistentDataPath + "/user.dat"))
            {
                FileStream stream = new FileStream((Application.persistentDataPath + "/user.dat"), FileMode.Open);
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                T dt = GetProtobufObjectFromBytes<T>(bytes);
                return dt;
            }

            return default;

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