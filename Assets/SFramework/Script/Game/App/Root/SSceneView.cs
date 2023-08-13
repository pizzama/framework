using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SFramework.Game
{
    public abstract class SSCENEView : RootView
    {
        private List<string> _buildInSceneNames;
        protected override void init()
        {
            getBuildInSceneNames(out _buildInSceneNames);
        }

        public async UniTask<AsyncOperation> LoadSceneAsync(string scenePath, string sceneName, LoadSceneMode mode)
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
                //load scene from ab bundle
                Object request = await assetManager.LoadResourceAsync<Object>(scenePath, sceneName);
                if (request != null)
                {
#if UNITY_EDITOR
                    string obj_path = UnityEditor.AssetDatabase.GetAssetPath(request);
                    operation = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(obj_path, new LoadSceneParameters(mode));
#endif
                }
                else
                {
                    operation = SceneManager.LoadSceneAsync(scenePath, mode);
                }
            }
            else
            {
                operation = SceneManager.LoadSceneAsync(scenePath, mode);
            }

            return operation;
        }

        public async UniTask<bool> UnloadSceneAsync(string scenePath)
        {
            Scene sc = SceneManager.GetSceneByName(scenePath);
            if (sc != null && sc.isLoaded)
            {
                return true;
            }
            
            await SceneManager.UnloadSceneAsync(scenePath);

            return true;
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

