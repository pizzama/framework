using System.Collections.Generic;
using SFramework.Pool;
using UnityEngine;
using SFramework.Extension;
using SFramework.Tools.Math;
using Cysharp.Threading.Tasks;

namespace SFramework.Game
{
    public abstract class RootView : SView
    {
        protected const string findTag = "$EXPORT$";
        protected AssetsManager assetManager;
        protected GameObjectPoolManager poolManager; //pool manager

        public SUIROOT UIRoot { get { return SUIROOT.Instance; } }
        public virtual Transform GetViewTransform()
        {
            return null;
        }
        public override void Install()
        {
            assetManager = AssetsManager.Instance;
            poolManager = GameObjectPoolManager.Instance;
            //init ui
            base.Install();
        }

        public override void Open()
        {
            ViewOpenType tp = Control.GetViewOpenType();
            if (tp == ViewOpenType.SingleNone || tp == ViewOpenType.SingleNeverClose)
            {
                ViewCallback?.Invoke();
                return;
            }
            base.Open();
        }

        public T LoadFromResources<T>(string path) where T : UnityEngine.Object
        {
            return (T)assetManager.LoadFromResources<T>(path);
        }

        public T LoadFromBundle<T>(string path) where T : UnityEngine.Object
        {
            int index = path.LastIndexOf("/");
            if (index > 0)
            {
                string abName = path.Substring(0, index);
                string resName = path.Substring(index + 1, path.Length - index - 1);
                return (T)LoadFromBundle<T>(abName, resName);
            }

            return null;
        }

        public T LoadFromBundle<T>(string abName, string resName) where T : UnityEngine.Object
        {
            return (T)assetManager.LoadFromBundle<T>(abName, resName);
        }

        public GameObject CreateGameObjectUsingPool(string abName, string resName, float lifeTime = -1, int maxCount = 10)
        {
            UnityEngine.GameObject prefab = assetManager.LoadFromBundle<UnityEngine.GameObject>(abName, resName);
            if (prefab == null)
                return null;
            string poolName = assetManager.FullPath(abName, resName);
            return poolManager.Request<ListGameObjectPool>(poolName, prefab, lifeTime, maxCount);
        }

        public GameObject CreateGameObjectUsingPool(string path, float lifeTime = -1, int maxCount = 10)
        {
            int index = path.LastIndexOf("/");
            if (index > 0)
            {
                string abName = path.Substring(0, index);
                string resName = path.Substring(index + 1, path.Length - index - 1);
                return CreateGameObjectUsingPool(abName, resName, lifeTime, maxCount);
            }

            return null;
        }

        public T CreateComponent<T>(string prefabFullPath, Transform parent, Vector3 pos = default, float lifeTime = -1, int maxCount = 10) where T : Component
        {
            GameObject obj = CreateGameObjectUsingPool(prefabFullPath, lifeTime, maxCount);
            if (obj == null)
                throw new DataErrorException("Could not found component path is:" + prefabFullPath);
            T result = obj.GetOrAddComponent<T>();
            if (result == null)
                throw new DataErrorException("Could not add component:" + typeof(T));
            if (result != null)
            {
                if (parent == null)
                    throw new NotFoundException("Couldn't find parent as " + prefabFullPath);

                result.transform.SetParent(parent, false);
                result.transform.localPosition = pos;
            }
            return result;
        }

        public void ReleaseGameObjectUsingPool(GameObject obj)
        {
            poolManager.Return(obj);
        }

        public void ReleaseGameObjectDestroy(GameObject obj, bool isImmediate)
        {
            poolManager.DestroyGameObject(obj, isImmediate);
        }

        public T CreateEntity<T>(string prefabFullPath, Transform parent, Vector3 pos = default, float lifeTime = -1, string instanceId = "") where T : Component
        {
            T result = CreateComponent<T>(prefabFullPath, parent, pos, lifeTime);
            if (string.IsNullOrEmpty(instanceId))
                instanceId = MathTools.RandomInt(1000000, 9999999).ToString();
            if (result is RootEntity root)
            {
                root.SetEntityData(instanceId, this);
                root.Show();
            }
            return result;
        }

        protected virtual T getExportObject<T>(string key)
        {
            return default;
        }
        
        protected void rootEntityShowTrigger(Dictionary<string, GameObject> goValue)
        {
            if (goValue == null)
                return;
            foreach (var item in goValue)
            {
                ISEntity entity = item.Value.GetComponent<ISEntity>();
                if (entity != null)
                {
                    if (entity is RootEntity root)
                    {
                        root.SetParentView(this);
                    }
                    entity.Show();
                }
            }
        }
        
        protected void rootEntityRecycleTrigger(Dictionary<string, GameObject> goValue)
        {
            if (goValue == null)
                return;
            foreach (var item in goValue)
            {
                if(item.Value != null)
                {
                    ISEntity entity = item.Value.GetComponent<ISEntity>();
                    if (entity != null)
                    {
                        entity.Recycle();
                    }
                }
            }
        }
        
        public async UniTask<T> LoadFromResourcesAsync<T>(string path) where T : UnityEngine.Object
        {
            return await assetManager.LoadFromResourcesAsync<T>(path);
        }
        
        public async UniTask<T> LoadFromBundleAsync<T>(string path) where T : UnityEngine.Object
        {
            int index = path.LastIndexOf("/");
            if (index > 0)
            {
                string abName = path.Substring(0, index);
                string resName = path.Substring(index + 1, path.Length - index - 1);
                return await LoadFromBundleAsync<T>(abName, resName);
            }

            return null;
        }

        public async UniTask<T> LoadFromBundleAsync<T>(string abName, string resName) where T : UnityEngine.Object
        {
            return await assetManager.LoadFromBundleAsync<T>(abName, resName);
        }
        
        public async UniTask<GameObject> CreateGameObjectUsingPoolAsync(string path, float lifeTime = -1, int maxCount = 10)
        {
            int index = path.LastIndexOf("/");
            if (index > 0)
            {
                string abName = path.Substring(0, index);
                string resName = path.Substring(index + 1, path.Length - index - 1);
                return await CreateGameObjectUsingPoolAsync(abName, resName, lifeTime, maxCount);
            }

            return null;
        }

        public async UniTask<GameObject> CreateGameObjectUsingPoolAsync(string abName, string resName, float lifeTime = -1,
            int maxCount = 10)
        {
            UnityEngine.GameObject prefab = await assetManager.LoadFromBundleAsync<UnityEngine.GameObject>(abName, resName);
            if (prefab == null)
                return null;
            string poolName = assetManager.FullPath(abName, resName);
            return poolManager.Request<ListGameObjectPool>(poolName, prefab, lifeTime, maxCount);
        }
        
        public async UniTask<T> CreateComponentAsync<T>(string prefabFullPath, Transform parent, Vector3 pos = default, float lifeTime = -1, int maxCount = 10) where T : Component
        {
            GameObject obj = await CreateGameObjectUsingPoolAsync(prefabFullPath, lifeTime, maxCount);
            if (obj == null)
                throw new DataErrorException("Could not found component path is:" + prefabFullPath);
            T result = obj.GetOrAddComponent<T>();
            if (result == null)
                throw new DataErrorException("Could not add component:" + typeof(T));
            if (result != null)
            {
                if (parent == null)
                    throw new NotFoundException("Couldn't find parent as " + prefabFullPath);

                result.transform.SetParent(parent, false);
                result.transform.localPosition = pos;
            }
            return result;
        }
        
        public async UniTask<T> CreateEntityAsync<T>(string prefabFullPath, Transform parent, Vector3 pos = default, float lifeTime = -1, string instanceId = "") where T : Component
        {
            T result = await CreateComponentAsync<T>(prefabFullPath, parent, pos, lifeTime);
            if (string.IsNullOrEmpty(instanceId))
                instanceId = MathTools.RandomInt(1000000, 9999999).ToString();
            if (result is RootEntity root)
            {
                root.SetEntityData(instanceId, this);
                root.Show();
            }
            return result;
        }
    }
}