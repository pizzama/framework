using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using SFramework.Pool;

namespace SFramework
{
    public class RootManager
    {
        public static string SCENEPREFIX = "$s$";
        public static string UIPREFIX = "$u$";
        private static RootManager _instance;
        public static RootManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RootManager();
                return _instance;
            }
        }

        private SUIROOT _uiRoot;
        private Dictionary<string, GameObject> _sceneDict; //存储当前场景的元素
        private Dictionary<string, Transform> _uiCache; //缓存打开的ui

        private GameObjectPoolManager _poolManager;

        private RootManager()
        {
            initUI();
            collectScene();
            collectCamera();
        }

        private void initUI()
        {
            const string uiName = "SUIROOT";
            if (!_uiRoot)
            {
                var rootPrefab = Resources.Load<GameObject>(uiName);
                if (!rootPrefab)
                {
                    throw new NotFoundException(uiName);
                }
                GameObject uiRoot = Object.Instantiate(rootPrefab);
                Object.DontDestroyOnLoad(uiRoot);
                _uiRoot = ComponentTools.GetOrAddComponent<SUIROOT>(uiRoot);
            }

            if (_uiCache == null)
            {
                _uiCache = new Dictionary<string, Transform>();
            }

            // init ui pool
            _poolManager = GameObjectPoolManager.Instance;

        }

        private void collectScene()
        {
            //init current scene
            if (_sceneDict == null)
            {
                _sceneDict = new Dictionary<string, GameObject>();
                foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
                {
                    //遍历场景中的GameObject 记录需要的object
                    if (obj.name.IndexOf(RootManager.SCENEPREFIX) >= 0)
                    {
                        _sceneDict[obj.name] = obj;
                    }
                }
            }
        }

        private void collectCamera()
        {
            UniversalAdditionalCameraData cameraData = Camera.main.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(_uiRoot.UICamera);
        }

        public SUIROOT GetUIRoot()
        {
            return _uiRoot;
        }

        public GameObject GetDefineSceneObject(string name)
        {
            GameObject go = null;
            _sceneDict.TryGetValue(RootManager.SCENEPREFIX + name, out go);
            return go;
        }

        public Transform GetCacheUI(string name)
        {
            Transform ta = null;
            _uiCache.TryGetValue(name, out ta);
            return ta;
        }

        public GameObject SetCacheUI(string name, GameObject prefab)
        {
            ListGameObjectPool pool = _poolManager.CreatGameObjectPool<ListGameObjectPool>(name);
            if (pool.Prefab == null)
            {
                pool.Prefab = prefab;
            }
            return pool.Request();
        }

    }
}