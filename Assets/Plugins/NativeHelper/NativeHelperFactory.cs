using System.Collections.Generic;
using UnityEngine;

namespace NativeHelper
{
    public class NativeHelperFactory
    {
        private static INativeHelper _instance;
        private static Dictionary<RuntimePlatform, INativeHelper> _helperDict = new Dictionary<RuntimePlatform, INativeHelper>();
        public static INativeHelper Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = Create();
                }
                return _instance; 
            }
        }
        public static INativeHelper Create()
        {
            INativeHelper helper = null;
            bool rt = _helperDict.ContainsKey(Application.platform);
            if (rt)
                helper = _helperDict[Application.platform];
            if (helper != null)
                return helper;
            
            switch (Application.platform)
            {
                case RuntimePlatform.WebGLPlayer:
                    helper = new JsNativeHelper();
                    break;
                case RuntimePlatform.Android:
                    helper = new AndroidNaviteHelper();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    helper = new IOSNativeHelper();
                    break;
                default:
                    helper = new NativeHelper();
                    break;
            }
            
            return helper;
        }

        public static void SetHelper(RuntimePlatform rpt, INativeHelper value)
        {
            _helperDict[rpt] = value;
        }
    }
}
