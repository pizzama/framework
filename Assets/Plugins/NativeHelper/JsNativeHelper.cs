using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class JsNativeHelper
{
    [DllImport("__Internal")]
    public static extern void JSAlert(string content);
}
