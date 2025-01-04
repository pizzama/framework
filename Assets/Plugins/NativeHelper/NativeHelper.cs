using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace NativeHelper
{
    public interface INativeHelper
    {
        void Alert(string content);

        void Save(byte[] bytes, string fileName);

        byte[] Read(string fileName);

        void Delete(string fileName);

        string GetApplicationPersistentDataPath();

        void SendEmail(string recipient, string subject, string body);

        void RestartGame();

        void Vibrate(float time); // 平台震动

        // 平台支付相关
        public void LoadProductsOrder(List<string> productids);
        public string QueryProductsOrder(string productid);
        public void ConsumePayOrder(string insideOrderId);
        public void PayOrder(string orderid, string productid, string paytype);
        public bool CheckLostOrder(string orderid, string productid, string paytype);
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
        
        /// <summary>
        /// 打开邮箱 recipient: pizzaman@126.com
        /// </summary>
        public void SendEmail(string recipient, string subject, string body)
        {
            Application.OpenURL("mailto:" + recipient + "?subject=" + subject + "&body=" + body);
        }
        
        public void RestartGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        public void Vibrate(float time)
        {
            Debug.Log("Vibrate execute");
        }

        public void LoadProductsOrder(List<string> productids)
        {
            Debug.Log("LoadProductsOrder execute");
        }

        public string QueryProductsOrder(string productid)
        {
            Debug.Log("QueryProductsOrder execute");
            return "";
        }

        public void ConsumePayOrder(string insideOrderId)
        {
            Debug.Log("ConsumePayOrder execute");
        }

        public void PayOrder(string orderid, string productid, string paytype)
        {
            Debug.Log("PayOrder execute");
        }

        public bool CheckLostOrder(string orderid, string productid, string paytype)
        {
            Debug.Log("CheckLostOrder execute");
            return false;
        }
    }
}
