using UnityEngine;

namespace NativeHelper
{
    public class NativeHelperFactory
    {
        private static INativeHelper _instance;
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
    }
}
