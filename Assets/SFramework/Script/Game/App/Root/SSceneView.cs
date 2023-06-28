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
            getBuildInSceneNames(out _buildInSceneNames);
        }

        public async UniTask<AsyncOperation> LoadSceneAsync(string scenePath, string sceneName ,LoadSceneMode mode)
        {
            Scene sc = SceneManager.GetActiveScene();
            if (sc.name == scenePath)
            {
                return null;
            }

            sc = SceneManager.GetSceneByName(scenePath);
            if (sc.isLoaded)
            {
                return null;
            }

            AsyncOperation operation = null;
            if (!_buildInSceneNames.Contains(scenePath))
            {
#if UNITY_EDITOR
                //load scene from ab bundle
                Object request = await assetManager.LoadResourceAsync<Object>(scenePath, sceneName);
                if (request != null)
                {
                    string obj_path = UnityEditor.AssetDatabase.GetAssetPath(request);
                    operation = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(obj_path, new LoadSceneParameters(mode));
                }
                else
                {
                    operation = SceneManager.LoadSceneAsync(scenePath, mode);
                }
#endif
            }
            else
            {
                operation = SceneManager.LoadSceneAsync(scenePath, mode);
            }

            return operation;
        }

        private void getBuildInSceneNames(out List<string> names)
        {
            int i = 0;
            names = new List<string>();
            var sceneName = string.Empty;
            do
            {
                sceneName = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
                if (!string.IsNullOrEmpty(sceneName))
                {
                    if (!names.Contains(sceneName))
                    {
                        names.Add(sceneName);
                    }
                }

                i++;
            } while (!string.IsNullOrEmpty(sceneName));
        }
    }
}

