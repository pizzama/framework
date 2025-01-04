// #if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

// #endif

namespace NativeHelper
{
    public class IOSNativeHelper : INativeHelper
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void AlertContentDll(string title, string content);
        [DllImport("__Internal")]
        private static extern void LoadProducts(string productids);
        [DllImport("__Internal")]
        private static extern string QueryProducts(string productids);
        [DllImport("__Internal")]
        private static extern void Pay(string productParams);
        [DllImport("__Internal")]
        private static extern void ConsumePay(string insideOrderId);
        [DllImport("__Internal")]
        private static extern void CheckLostOrder(string productParams); 
        [DllImport("__Internal")]
        private static extern void RestartApp();
#endif
        public void Alert(string content)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            Debug.Log("alert");
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
#if UNITY_IPHONE && !UNITY_EDITOR
            RestartApp();
#endif
        }

        public void Vibrate(float time)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }

        // 加载平台提供的支付项
        public void LoadProductsOrder(List<string> productids)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            // var inapps = new List<string>();
            // inapps.Add("com.budda.assistant.coin1");
            LoadProducts(string.Join(",", productids));
#endif
        }
        
        public string QueryProductsOrder(string productid)
        {
#if UNITY_IPHONE && !UNITY_EDITOR  
            string rt = QueryProducts(productid);
            Debug.Log(productid + ";" + rt);
            return rt;
#endif
            return "";
        }
        
        public void ConsumePayOrder(string insideOrderId)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            ConsumePay(insideOrderId);
#endif
        }
        
        public void PayOrder(string orderid, string productid, string paytype)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            var purchaseInfo = string.Format("{0}|{1}|{2}",orderid,productid, paytype);
            Pay(purchaseInfo);
#endif
        }
        
        public bool CheckLostOrder(string orderid, string productid, string paytype)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            bool isCheckLosting = false;
            var purchaseInfo = string.Format("{0}|{1}|{2}",orderid,productid, paytype);
            CheckLostOrder(purchaseInfo);
            return isCheckLosting;
#endif
            return false;
        }
        
        
    }
}