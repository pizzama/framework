using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

        private void Start()
        {
            //Hold GameLauncher all the time
            DontDestroyOnLoad(transform.gameObject);
            initGameLauncher().Forget();
        }

        private async UniTaskVoid initGameLauncher()
        {
            await ABManager.Instance.InitAsync();
            initFrameworkBundle();
            installBundle();
        }

        private void initFrameworkBundle()
        {
            // init game framework bundle
            Tuple<int, Performance> perFormance = SBundleManager.Instance.GetPerformance();
            _targetFrameRate = perFormance.Item1;
            Application.targetFrameRate = _targetFrameRate;
            Application.runInBackground = _runInBackground;
        }

        protected void initAllControl()
        {
            //Otherwise you must init all control by manual 
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
    }
}
