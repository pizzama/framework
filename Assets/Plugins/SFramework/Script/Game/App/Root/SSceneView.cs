using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SFramework.Extension;
using SFramework.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SFramework.Game
{
    public abstract class SSCENEView : RootView
    {
        private List<string> _buildInSceneNames;
        private Dictionary<string, GameObject> goDict;
        protected string mAbPath; //scene asset bundle path
        protected string mAbName; //scene asset bundle name

        protected override void initing()
        {
            getBuildInSceneNames(out _buildInSceneNames); // init scene int editor buiding settings
        }

        public override void Open()
        {
            ViewOpenType tp = Control.GetViewOpenType();
            if (tp == ViewOpenType.SingleNone || tp == ViewOpenType.SingleNeverClose)
            {
                ViewCallback?.Invoke();
                return;
            }
            SetScenePath(out mAbPath, out mAbName);
            if (tp == ViewOpenType.Single || tp == ViewOpenType.Additive)
            {
                loadSceneProcessAsync(mAbPath, mAbName, (LoadSceneMode)tp).Forget();
            }
            else
            {
                throw new NotFoundException("not found LoadSceneMode, please check GetViewOpenType");
            }
        }

        public override void Close()
        {
            rootEntityRecycleTrigger(goDict);
        }

        //if you need control the loading progress you will override the method
        protected virtual async UniTask<bool> loadingScene(float progress)
        {
            await UniTask.Yield();
            return true;
        }

        protected virtual async UniTaskVoid loadSceneProcessAsync(
            string scenePath,
            string sceneName,
            LoadSceneMode mode
        )
        {
            ViewOpenType tp = Control.GetViewOpenType();
            if (!(tp == ViewOpenType.Single || tp == ViewOpenType.Additive))
            {
                throw new NotFoundException("not found LoadSceneMode, please check GetViewOpenType");
            }

            AsyncOperation operation = await LoadSceneAsync(
                scenePath,
                sceneName,
                (LoadSceneMode)tp
            );
            if (operation == null)
            {
                return;
            }
            operation.allowSceneActivation = false;
            var progress = Progress.Create<float>(
                (p) =>
                {
                    UniTask.Void(async () =>
                    {
                        operation.allowSceneActivation = await loadingScene(p);
                    });
                }
            );
            await operation.ToUniTask(progress);

            Control.CloseAllControl(new List<ISBundle>() { Control });
            goDict = collectSceneByTag();
            //loading scene success the view is open
            base.Open();
            rootEntityShowTrigger(goDict);
            ViewCallback?.Invoke();
        }

        public async UniTask<AsyncOperation> LoadSceneAsync(
            string scenePath,
            string sceneName,
            LoadSceneMode mode
        )
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
                if (ABPathHelper.SimulationMode)
                {
#if UNITY_EDITOR
                    string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(
                        scenePath.ToLower()
                    );
                    foreach (string path in assetPaths)
                    {
                        if (path.IndexOf(sceneName, StringComparison.Ordinal) >= 0)
                        {
                            operation =
                                UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(
                                    path,
                                    new LoadSceneParameters(mode)
                                );
                        }
                        break;
                    }
#endif
                }
                else
                {
                    //load scene from ab bundle
                    ABInfo request = await assetManager.LoadBundleAsync(scenePath.ToLower());
                    if (request != null)
                    {
                        operation = SceneManager.LoadSceneAsync(sceneName, mode);
                    }
                }
            }
            else
            {
                operation = SceneManager.LoadSceneAsync(sceneName, mode);
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
            path = path.Replace('.', '_');
            prefabPath = path + "." + AssetsManager.SceneExtendName;
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
            string sceneName;
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
            GameObject[] alls = SceneManager.GetActiveScene().GetRootGameObjects(); //collect all gameobject from scene root
            for (int i = 0; i < alls.Length; i++)
            {
                GameObject go = alls[i];
                go.CollectAllGameObject(ref sceneDict, findTag);
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

        protected override T getExportObject<T>(string key)
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

        protected List<T> getExportObjectsWithChild<T>(Transform parent) where T: Component
        {
            List<T> result = new List<T>();
            if (parent == null)
                return result;
            for (var i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                T tchild = child.GetComponent<T>();
                if (tchild != null)
                {
                    if (tchild is RootEntity root)
                    {
                        root.SetEntityData(StringTools.GenerateRandomNumber(5), this);
                    }
                    result.Add(tchild);
                }
                else
                {
                    List<T> temp = getExportObjectsWithChild<T>(child);
                    result = result.Concat(temp).ToList();
                }

            }

            return result;
        }

        protected List<T> getExportObjectsWithChild<T>(string key) where T: Component
        {
            Transform parent = getExportObject<Transform>(key);
            return getExportObjectsWithChild<T>(parent);
        }
    }
}
