using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SFramework.Pool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SFramework.Game
{
    public abstract class SSCENEView : RootView
    {
        private List<string> _buildInSceneNames;
        private Dictionary<string, GameObject> _sceneDict; //�洢��ǰ������Ԫ��

        protected string mAbPath; //scene asset bundle path
        protected string mAbName; //scene asset bundle name

        protected override void init()
        {
            getBuildInSceneNames(out _buildInSceneNames);
        }

        public override void Open()
        {

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

        protected virtual void SetViewTransform(out Transform trans)
        {
            trans = null;
            SetViewPrefabPath(out mAbPath, out mAbName);

            if (!string.IsNullOrEmpty(mAbPath))
            {
                ListGameObjectPool pool = poolManager.CreateGameObjectPool<ListGameObjectPool>(mAbPath);
                if (pool.Prefab == null)
                {
                    pool.Prefab = assetManager.LoadResource<GameObject>(mAbPath, mAbName);
                }

                trans = pool.Request().transform;
            }
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

        protected void collectScene()
        {
            _sceneDict = new Dictionary<string, GameObject>();
            //init current scene
            if (_sceneDict == null)
            {
                foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
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
            }
        }

        protected GameObject getDefineSceneObject(string name)
        {
            GameObject go = null;
            _sceneDict.TryGetValue(name, out go);
            return go;
        }
    }
}

