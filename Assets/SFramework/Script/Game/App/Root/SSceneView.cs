using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SFramework.Game
{
    public abstract class SSCENEView : RootView
    {
        private List<string> _buildInSceneNames;
        protected string mAbPath; //scene asset bundle path
        protected string mAbName; //scene asset bundle name

        protected override void init()
        {
            getBuildInSceneNames(out _buildInSceneNames);
        }

        public override void Open()
        {
            SetScenePath(out mAbPath, out mAbName);
            loadScene(mAbPath, mAbName, (LoadSceneMode)GetViewOpenType()).Forget();
        }

        private async UniTaskVoid loadScene(string scenePath, string sceneName, LoadSceneMode mode)
        {
            await LoadSceneAsync(mAbPath, mAbName, (LoadSceneMode)GetViewOpenType());
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

        protected virtual void SetViewPrefabPath(out string prefabPath, out string prefabName)
        {
            System.Type tp = GetType();
            string path = tp.FullName;
            path = path.Replace('.', '/');
            prefabPath = path;
            prefabName = tp.Name;
        }

        private void SetScenePath(out string prefabPath, out string prefabName)
        {
            SetViewPrefabPath(out prefabPath, out prefabName);
            
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

        protected Dictionary<string, T> collectScene<T>() where T : Object
        {
            Dictionary<string, T>  _sceneDict = new Dictionary<string, T>();
            T[] all = Object.FindObjectsOfType<T>(true);
            foreach (T obj in all)
            {
                if (_sceneDict.ContainsKey(obj.name))
                {
                    Debug.LogWarning("Scene has same name:" + obj.name);
                }
                else
                {
                    _sceneDict[obj.name] = obj;
                }
            }

            return _sceneDict;
        }
    }
}

