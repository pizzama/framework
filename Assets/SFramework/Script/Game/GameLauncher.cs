using System;
using System.Collections.Generic;
using SFramework.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SEnum;

namespace SFramework
{
    public abstract class GameLauncher : MonoBehaviour
    {
        [SerializeField]
        private int _targetFrameRate = 60;

        [SerializeField]
        private bool _runInBackground = true;

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
            Application.targetFrameRate = GetFrameRate();
            Application.runInBackground = _runInBackground;
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

        private void Update() { }

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

        private int GetFrameRate()
        {
            if (
                SystemInfo.systemMemorySize >= 4096
                && SystemInfo.processorFrequency >= 2048
                && SystemInfo.processorCount >= 4
            )
            {
                //"performance better";
                SBundleManager.Instance.SetPerformance(Performance.High);
                return 60;
            }
            else if (SystemInfo.systemMemorySize <= 2048)
            {
                SBundleManager.Instance.SetPerformance(Performance.Low);
                return 30;
            }
            else
            {
                SBundleManager.Instance.SetPerformance(Performance.Middle);
                return 30;
            }
        }
    }
}
