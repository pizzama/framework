#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace NativeHelper
{
    public class JsNativeHelper : INativeHelper
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void JSAlert(string content);
        [DllImport("__Internal")] private static extern void JSRestartGame();
        [DllImport("__Internal")] private static extern void JSVibrate(float time); //震动时间秒
#endif

        public void Alert(string content) 
        { 
#if UNITY_WEBGL && !UNITY_EDITOR
            JSAlert(content);
#endif
        }
        //special WebGl platform need data refresh 
        public void Save(byte[] bytes, string fileName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string content = System.Convert.ToBase64String(bytes);
            PlayerPrefs.SetString(fileName, content);
#endif                
        }
        public byte[] Read(string fileName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string content = PlayerPrefs.GetString(fileName);
            if(!string.IsNullOrEmpty(content))
            {
                byte[] decodedBytes = System.Convert.FromBase64String(content);
                return decodedBytes;
            }
#endif
                return null;
        }
        public void Delete(string fileName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
                if (PlayerPrefs.HasKey(fileName))
                {
                        PlayerPrefs.DeleteKey(fileName);  
                }
#endif   
        }

        public string GetApplicationPersistentDataPath()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
                return Application.persistentDataPath + "/idbfs/";
#else
                return Application.persistentDataPath;
#endif
        }

        public void SendEmail(string recipient, string subject, string body)
        {
                Application.OpenURL("mailto:" + recipient + "?subject=" + subject + "&body=" + body);
        }

        public void RestartGame()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
                // 调用JavaScript的location.reload()方法
                JSRestartGame();
#endif   
        }

        public void Vibrate(float time)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        //     JSVibrate(time);
#endif
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

        public void PayOrder(string insideOrderId, string insideSid, string productid, string paytype)
        {
                Debug.Log("PayOrder execute");
        }

        public bool CheckLostOrder(string insideOrderId, string insideSid, string productid, string paytype)
        {
                Debug.Log("CheckLostOrder execute");
                return false;
        }
    }
}
