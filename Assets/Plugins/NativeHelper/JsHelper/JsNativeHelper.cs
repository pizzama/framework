using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NativeHelper
{
    public class JsNativeHelper : NativeHelper
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void JSAlert(string content);

        [DllImport("__Internal")]
        private static extern void JSSyncDB();
#endif

        public override void Alert(string content) 
        { 
#if UNITY_WEBGL && !UNITY_EDITOR
            JSAlert(content);
#endif
        }

        public override void SyncDB()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            JSSyncDB();
#endif
        }

        public override string GetApplicationPersistentDataPath()
        {
                return Application.persistentDataPath + "/idbfs/";
        }
    }
}
