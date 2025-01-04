using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NativeHelper
{
    public class AndroidNaviteHelper: INativeHelper
    {
        public void Alert(string content)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            throw new System.NotImplementedException();
#endif
        }
        
        public void Save(byte[] bytes, string fileName)
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
// #if UNITY_ANDROID && !UNITY_EDITOR
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
// #endif   
        }

        public string GetApplicationPersistentDataPath()
        {
            return Application.persistentDataPath;
        }

        public void SendEmail(string recipient, string subject, string body)
        {
            Application.OpenURL("mailto:" + recipient + "?subject=" + subject + "&body=" + body);
        }

        public void RestartGame()
        {
            throw new System.NotImplementedException();
        }

        public void Vibrate(float time)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }

        public void LoadProductsOrder(List<string> productids)
        {
            throw new System.NotImplementedException();
        }

        public string QueryProductsOrder(string productid)
        {
            throw new System.NotImplementedException();
        }

        public void ConsumePayOrder(string insideOrderId)
        {
            throw new System.NotImplementedException();
        }

        public void PayOrder(string orderid, string productid, string paytype)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckLostOrder(string orderid, string productid, string paytype)
        {
            throw new System.NotImplementedException();
        }
    }
}