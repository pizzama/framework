using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SFramework
{
    public abstract class SSCENEView : RootView
    {
        private List<string> _buildInSceneNames;
        protected override void init()
        {
            ComponentTools.GetBuildInSceneNames(out _buildInSceneNames);
        }

        public async UniTaskVoid LoadSceneAsync(string sceneFullName, LoadSceneMode mode)
        {
            Scene sc = SceneManager.GetActiveScene();
            if (sc.name == sceneFullName)
            {
                return;
            }

            sc = SceneManager.GetSceneByName(sceneFullName);
            if (sc.isLoaded)
            {
                return;
            }

            if (!_buildInSceneNames.Contains(sceneFullName))
            {
                //load scene from ab bundle

            }
            else
            {
                var progress = Progress.Create<float>(p => Debug.LogFormat("array p:{0}", p));
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneFullName, mode);
                await operation.ToUniTask(progress);
                await UniTask.Yield();
            }
        }
    }
}

