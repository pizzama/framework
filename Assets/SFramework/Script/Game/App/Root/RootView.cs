using SFramework.Pool;
using UnityEngine;
using SFramework.Extension;
using SFramework.Tools;
using SFramework.Tools.Math;

namespace SFramework.Game
{
    public abstract class RootView : SView
    {
        protected const string findTag = "$EXPORT$";
        protected abstract ViewOpenType GetViewOpenType();
        protected AssetsManager assetManager;
        protected GameObjectPoolManager poolManager; //pool manager

        public SUIROOT UIRoot { get { return SUIROOT.Instance; } }

        public override void Install()
        {
            assetManager = AssetsManager.Instance;
            poolManager = GameObjectPoolManager.Instance;
            //init ui
            base.Install();
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

        public GameObject CreateGameObjectUsingPool(string abName, string resName, float lifeTime = -1)
        {
            UnityEngine.GameObject prefab = assetManager.LoadFromBundle<UnityEngine.GameObject>(abName, resName);
            if (prefab == null)
                return null;
            string poolName = assetManager.FullPath(abName, resName);
            return poolManager.Request<ListGameObjectPool>(poolName, prefab, lifeTime);
        }

        public GameObject CreateGameObjectUsingPool(string path, float lifeTime = -1)
        {
            int index = path.LastIndexOf("/");
            if (index > 0)
            {
                string abName = path.Substring(0, index);
                string resName = path.Substring(index + 1, path.Length - index - 1);
                return CreateGameObjectUsingPool(abName, resName, lifeTime);
            }

            return null;
        }

        public T CreateComponent<T>(string prefabFullPath, Transform parent, Vector3 pos = default, float lifeTime = -1) where T: Component 
        {
            GameObject obj = CreateGameObjectUsingPool(prefabFullPath, lifeTime);
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

        public void ReleaseComponent(Component comp)
        {
            ReleaseGameObjectUsingPool(comp.gameObject);
        }

        public T CreateEntity<T>(string prefabFullPath, Transform parent, Vector3 pos = default, float lifeTime = -1, string instanceId = "") where T: RootEntity
        {
            T result = CreateComponent<T>(prefabFullPath, parent, pos, lifeTime);
            if(string.IsNullOrEmpty(instanceId))
                instanceId = MathTools.RandomInt(1000000,9999999).ToString();
            result.SetEntityData(instanceId, this);
            return result;
        }

        protected virtual T getExportObject<T>(string key)
        {
            return default;
        }
    }
}