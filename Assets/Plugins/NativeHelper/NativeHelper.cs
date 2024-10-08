using System;
using UnityEngine;
using System.IO;

namespace NativeHelper
{
    public interface INativeHelper
    {
        void Alert(string content);

        void Save(byte[] bytes, string fileName);

        byte[] Read(string fileName);

        void Delete(string fileName);

        string GetApplicationPersistentDataPath();
    }

    public class NativeHelper : INativeHelper
    {
        public virtual void Alert(string content)
        {
            Debug.Log("Alert: " + content);
        }

        public virtual void Save(byte[] bytes, string fileName)
        {
            string fileFullPath = GetApplicationPersistentDataPath() + "/" + fileName;
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
        }

        public byte[] Read(string fileName)
        {
            string fileFullPath = GetApplicationPersistentDataPath() + "/" + fileName;
            if (File.Exists(fileFullPath))
            {
                FileStream stream = new FileStream(fileFullPath, FileMode.Open);
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Close();
                return bytes;
            }

            return null;
        }

        public void Delete(string fileName)
        {
            string fileFullPath = GetApplicationPersistentDataPath() + "/" + fileName;
            if (File.Exists(fileFullPath))
            {
                try
                {
                    File.Delete(fileFullPath);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

            }
        }

        public virtual string GetApplicationPersistentDataPath()
        {
            return Application.persistentDataPath;
        }
    }
}
