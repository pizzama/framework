using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace PFramework
{
    public partial class ABManager : MonoBehaviour
    {
        private static ABManager _instance;
        public static ABManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<ABManager>();
                if (_instance != null) return _instance;
                var obj = new GameObject();
                obj.name = "ABManager";
                _instance = obj.AddComponent<ABManager>();
                _instance.init();
                return _instance;
            }
        }
        private Dictionary<string, AssetBundle> _abCache;
        private ABManifest _manifest;
        private void init()
        {
            _abCache = new Dictionary<string, AssetBundle>();
            _manifest = new ABManifest(); //主包的Bundle和依赖关系
            _manifest.LoadAssetBundleManifest();
        }


        protected void Init()
        {
            //初始化字典
            _abCache = new Dictionary<string, AssetBundle>();
        }

        //加载AB包
        private AssetBundle LoadABPackage(string abName)
        {
            AssetBundle ab = null;
            //根据manifest获取所有依赖包的名称 固定API
            string[] dependencies = _manifest.GetDependencies(abName);
            //循环加载所有依赖包
            for (int i = 0; i < dependencies.Length; i++)
            {
                //如果不在缓存则加入,防止重复加载
                if (!_abCache.ContainsKey(dependencies[i]))
                {
                    //先加载外部地址在加载内部地址
                    ab = AssetBundle.LoadFromFile(ABPathHelper.GetResPathInPersistentOrStream(dependencies[i]));
                    //注意添加进缓存 防止重复加载AB包
                    _abCache.Add(dependencies[i], ab);
                }
            }
            //加载目标包
            if (_abCache.ContainsKey(abName)) return _abCache[abName];
            else
            {
                ab = AssetBundle.LoadFromFile(ABPathHelper.GetResPathInPersistentOrStream(abName));
                _abCache.Add(abName, ab);
                return ab;
            }
        }


        #region 同步加载的三个重载
        // 同步加载资源---泛型加载 简单直观 无需显示转换
        //Sprite sp = abManager.LoadResource<Sprite>("a_png", "a");
        public T LoadResource<T>(string abName, string resName) where T : Object
        {
#if UNITY_EDITOR
            if (ABPathHelper.SimulationMode)
            {
                T res = null;
                string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
                for (int i = 0; i < assetPaths.Length; i++)
                {
                    if (res == null)
                    {
                        res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);
                    }
                }

                if(res == null)
                    Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

                return res;
            }
#endif
            //加载目标包
            AssetBundle ab = LoadABPackage(abName);

            //返回资源
            return ab.LoadAsset<T>(resName);
        }


        //不指定类型 有重名情况下不建议使用 使用时需显示转换类型
        //Object sp = abManager.LoadResource("a_png", "a");
        public Object LoadResource(string abName, string resName)
        {
#if UNITY_EDITOR
            if (ABPathHelper.SimulationMode)
            {
                Object res = null;
                string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
                for (int i = 0; i < assetPaths.Length; i++)
                {
                    if (res == null)
                    {
                        res = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(assetPaths[i]);
                    }
                }

                if(res == null)
                    Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

                return res;
            }
#endif
            //加载目标包
            AssetBundle ab = LoadABPackage(abName);
            if(ab == null)
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);
            //返回资源
            return ab.LoadAsset(resName);
        }


        //利用参数传递类型，适合对泛型不支持的语言调用，使用时需强转类型
        //Object sp = abManager.LoadResource("a_png", "a", typeof(Texture2D));
        public Object LoadResource(string abName, string resName, System.Type type)
        {
#if UNITY_EDITOR
            if (ABPathHelper.SimulationMode)
            {
                Object res = null;
                string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
                for (int i = 0; i < assetPaths.Length; i++)
                {
                    if (res == null)
                    {
                        res = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPaths[i], type);
                    }
                }

                if(res == null)
                    Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

                return res;
            }
#endif
            //加载目标包
            AssetBundle ab = LoadABPackage(abName);

            //返回资源
            return ab.LoadAsset(resName, type);
        }

        #endregion


        #region 三种资源异步加载方式

        /// <summary>
        /// 提供异步加载----注意 这里加载AB包是同步加载，只是加载资源是异步
        /// </summary>
        /// <param name="abName">ab包名称</param>
        /// <param name="resName">资源名称</param>
        public void LoadResourceCoroutine(string abName, string resName, System.Action<Object> finishLoadObjectHandler)
        {
            AssetBundle ab = LoadABPackage(abName);
            //开启协程 提供资源加载成功后的委托
            StartCoroutine(LoadResCoroutine(ab, resName, finishLoadObjectHandler));
        }


        private IEnumerator LoadResCoroutine(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler)
        {
            if (ab == null) yield break;
            //异步加载资源API
            AssetBundleRequest abr = ab.LoadAssetAsync(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }


        //根据Type异步加载资源
        public void LoadResourceCoroutine(string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            AssetBundle ab = LoadABPackage(abName);
            StartCoroutine(LoadResCoroutine(ab, resName, type, finishLoadObjectHandler));
        }


        private IEnumerator LoadResCoroutine(AssetBundle ab, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync(resName, type);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }

        //泛型加载
        public void LoadResourceCoroutine<T>(string abName, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
        {
            AssetBundle ab = LoadABPackage(abName);
            StartCoroutine(LoadResCoroutine<T>(ab, resName, finishLoadObjectHandler));
        }

        private IEnumerator LoadResCoroutine<T>(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset as T);
        }

        #endregion

        //单个包卸载
        public void UnLoad(string abName)
        {
            if (_abCache.ContainsKey(abName))
            {
                _abCache[abName].Unload(false);
                //注意缓存需一并移除
                _abCache.Remove(abName);
            }
        }

        //所有包卸载
        public void UnLoadAll()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            //注意清空缓存
            _abCache.Clear();
            _manifest.Dispose();
        }
    }

}