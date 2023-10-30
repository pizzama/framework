using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SFramework.Game
{
    public abstract class SSCENEView : RootView
    {
        private const string findTag = "$SCENE$";
        private List<string> _buildInSceneNames;
        private Dictionary<string, GameObject> goDict;
        protected string mAbPath; //scene asset bundle path
        protected string mAbName; //scene asset bundle name
        protected override void initing()
        {
            getBuildInSceneNames(out _buildInSceneNames);// init scene int editor buiding settings
        }

        public override void Open()
        {
            SetScenePath(out mAbPath, out mAbName);
            loadSceneProcessAsync(mAbPath, mAbName, (LoadSceneMode)GetViewOpenType()).Forget();
        }

        //if you need control the loading progress you will override the method
        protected virtual async UniTask<bool> loadingScene(float progress)
        {
            await UniTask.Yield();
            return true;
        }

        protected virtual async UniTaskVoid loadSceneProcessAsync(string scenePath, string sceneName, LoadSceneMode mode)
        {
            AsyncOperation operation = await LoadSceneAsync(scenePath, sceneName, (LoadSceneMode)GetViewOpenType());
            if (operation == null)
            {
                return;
            }
            operation.allowSceneActivation = false;
            if (operation != null)
            {
                var progress = Progress.Create<float>((p) =>
                {
                    UniTask.Void(
                        async () =>
                        {
                            operation.allowSceneActivation = await loadingScene(p);
                        }
                    );
                });
                await operation.ToUniTask(progress);
            }

            Control.CloseAllControl(new List<ISBundle>() { Control });
            goDict = collectSceneByTag();
            //loading scene success the view is open
            base.Open();
            ViewCallback?.Invoke();
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
                UnityEngine.Object request = await assetManager.LoadFromBundleAsync<UnityEngine.Object>(scenePath, sceneName);
                if (request != null)
                {
#if UNITY_EDITOR
                    if (request is not UnityEditor.SceneAsset)//check current assets is or not scene assets
                    {
                        return null;
                    }
                    string obj_path = UnityEditor.AssetDatabase.GetAssetPath(request);
                    operation = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(obj_path, new LoadSceneParameters(mode));
#else

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
            string path = tp.Namespace;
            path = path.Replace('.', '/');
            prefabPath = path + ".sf";
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

        private Dictionary<string, GameObject> collectSceneByTag()
        {
            Dictionary<string, GameObject> sceneDict = new Dictionary<string, GameObject>();
            GameObject[] alls = GameObject.FindGameObjectsWithTag(findTag);
            for (int i = 0; i < alls.Length; i++)
            {
                var go = alls[i];
                sceneDict[go.name] = go;
            }

            return sceneDict;
        }

        protected Dictionary<string, T> collectScene<T>() where T : UnityEngine.Object
        {
            Dictionary<string, T> sceneDict = new Dictionary<string, T>();
            T[] all = UnityEngine.Object.FindObjectsOfType<T>(true);
            foreach (T obj in all)
            {
                if (sceneDict.ContainsKey(obj.name))
                {
                    Debug.LogWarning("Scene has same name:" + obj.name);
                }
                else
                {
                    sceneDict[obj.name] = obj;
                }
            }

            return sceneDict;
        }

        protected T getSceneObject<T>(string key)
        {
            GameObject go = null;
            goDict.TryGetValue(key, out go);
            if (go != null)
                return go.GetComponent<T>();
            else
            {
                goDict.TryGetValue(key + "(Clone)", out go);
                if (go != null)
                {
                    return go.GetComponent<T>();
                }
                else
                {
                    Debug.LogWarning("scene not found object by key:" + key);
                }
            }

            return default(T);
        }
    }
}

