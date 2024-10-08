#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;

namespace NativeHelper
{
    public class JsNativeHelper : INativeHelper
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void JSAlert(string content);
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
    }
}
