using UnityEngine;

namespace NativeHelper
{
    public class NativeHelperFactory
    {
        public static INativeHelper Create()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return new JsNativeHelper();
            }
            else
            {
                return new NativeHelper();
            }
        }
    }
}
