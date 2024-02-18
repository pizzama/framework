using System;
using UnityEngine;
using SFramework.Tools;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace SFramework
{
    public abstract class GameLauncher : MonoBehaviour
    {
        private void Awake()
        {
            //Hold GameLauncher all the time
            DontDestroyOnLoad(transform.gameObject);
            initFrameworkBundle();
            installBundle();
        }

        private void initFrameworkBundle()
        {
            // init game framework bundle
        }

        protected void initAllControl()
        {
            List<Type> controls = ReflectionTools.GetTypesFormTypeWithAllAssembly(typeof(SControl));
            for (int i = 0; i < controls.Count; i++)
            {
                Type cType = controls[i];
                if (cType.Name == "RootControl")
                    continue;
                ISBundle bd = (ISBundle)Activator.CreateInstance(cType, true);
                SBundleManager.Instance.InstallBundle(bd, "");
            }
        }

        protected abstract void installBundle();

        private void Update()
        {

        }

        private void OnApplicationPause(bool pause)
        {
            Debug.Log("pause");
        }

        private void OnApplicationQuit()
        {
            Debug.Log("quit");
        }

        private void OnApplicationFocus(bool focus)
        {
            Debug.Log("focus");
        }

    }
}
