using System;
using SFramework;
using UnityEngine;

namespace NativeHelper
{
    public class NativeHelperMonoBehaviour: SSingletonMonoBehaviour<NativeHelperMonoBehaviour>
    {
        private Action<string> _payResultCallAction;

        public Action<string> PayResultCallAction
        {
            set { _payResultCallAction = value; }
        }
        
        private void OnDestroy()
        {
            _payResultCallAction = null;
        }

        public void PayResultFromPlantForm(string jsonData)
        {
            Debug.Log("");
            _payResultCallAction?.Invoke(jsonData);
        }
    }
}