using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
namespace NativeHelper
{
    public class JsNativeHelper: NativeHelper
    {
        public override void Alert(string content)
        {
    #if UNITY_WEBGL && !UNITY_EDITOR
            [DllImport("__Internal")]
            public static extern void Alert(string content);
    #endif
        }

        public void SyncDB()
        {
    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncDB();
    #endif
        }
    }
}
