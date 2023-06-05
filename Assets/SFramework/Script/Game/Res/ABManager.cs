using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace SFramework
{
    public delegate void ABLoadCallBack(AssetBundle ab);
    public class ABManager
    {
        private static ABManager _instance;
        public static ABManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ABManager();
                return _instance;
            }
        }
        private Dictionary<string, ABInfo> _abCache;
        private ABManager()
        {
            _manifest = new ABManifest(); //主包的Bundle和依赖关系
            _manifest.LoadAssetBundleManifest();
            //初始化字典
            _abCache = new Dictionary<string, ABInfo>();
        }
        private ABManifest _manifest;
        //加载AB包
        private ABInfo LoadABPackage(string abName)
        {
            ABInfo maininfo = new ABInfo();
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
                    if (ab != null)
                    {
                        //注意添加进缓存 防止重复加载AB包
                        ABInfo info = new ABInfo();
                        info.HashName = dependencies[i];
                        info.AssetBundle = ab;
                        maininfo.AddDepends(info);
                        _abCache.Add(dependencies[i], info);
                    }
                }
                else
                {
                    ABInfo info = _abCache[dependencies[i]];
                    maininfo.AddDepends(info);
                }
            }
            //加载目标包
            if (_abCache.ContainsKey(abName)) return _abCache[abName];
            else
            {
                ab = AssetBundle.LoadFromFile(ABPathHelper.GetResPathInPersistentOrStream(abName));
                maininfo.HashName = abName;
                if (ab != null)
                {
                    maininfo.AssetBundle = ab;
                    maininfo.AddRef();
                    _abCache.Add(abName, maininfo);
                }
                else
                {
                    maininfo = null;
                }

                return maininfo;
            }
        }


        #region 同步加载的三个重载
        // 同步加载资源---泛型加载 简单直观 无需显示转换
        //Sprite sp = abManager.LoadResource<Sprite>("a_png", "a");
        public T LoadResource<T>(string abName, string resName) where T : Object
        {
            if (ABPathHelper.SimulationMode)
            {
                T rt = editorLoadResource<T>(abName, resName);
                if (rt != null)
                    return rt;
            }
            //加载目标包
            ABInfo ab = LoadABPackage(abName);

            //返回资源
            return ab.AssetBundle.LoadAsset<T>(resName);
        }


        //不指定类型 有重名情况下不建议使用 使用时需显示转换类型
        //Object sp = abManager.LoadResource("a_png", "a");
        public Object LoadResource(string abName, string resName)
        {
            if (ABPathHelper.SimulationMode)
            {
                Object rt = editorLoadResource<Object>(abName, resName);
                if (rt != null)
                    return rt;
            }
            //加载目标包
            ABInfo ab = LoadABPackage(abName);
            if (ab == null)
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);
            //返回资源
            return ab.AssetBundle.LoadAsset(resName);
        }


        //利用参数传递类型，适合对泛型不支持的语言调用，使用时需强转类型
        //Object sp = abManager.LoadResource("a_png", "a", typeof(Texture2D));
        public Object LoadResource(string abName, string resName, System.Type type)
        {
            if (ABPathHelper.SimulationMode)
            {
                Object rt = editorLoadResource(abName, resName, type);
                if (rt != null)
                    return rt;
            }
            //加载目标包
            ABInfo ab = LoadABPackage(abName);

            //返回资源
            return ab.AssetBundle.LoadAsset(resName, type);
        }

        //加载AB包
        private async UniTask<ABInfo> LoadABPackageAsync(string abName)
        {
            ABInfo maininfo = new ABInfo();
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
                    ab = await AssetBundle.LoadFromFileAsync(ABPathHelper.GetResPathInPersistentOrStream(dependencies[i]));
                    //注意添加进缓存 防止重复加载AB包
                    if (ab != null)
                    {
                        //注意添加进缓存 防止重复加载AB包
                        ABInfo info = new ABInfo();
                        info.HashName = dependencies[i];
                        info.AssetBundle = ab;
                        maininfo.AddDepends(info);
                        _abCache.Add(dependencies[i], info);
                    }
                }
                else
                {
                    ABInfo info = _abCache[dependencies[i]];
                    maininfo.AddDepends(info);
                }
            }
            //加载目标包
            if (_abCache.ContainsKey(abName)) return _abCache[abName];
            else
            {
                ab = await AssetBundle.LoadFromFileAsync(ABPathHelper.GetResPathInPersistentOrStream(abName));
                if (ab != null)
                {
                    maininfo.AssetBundle = ab;
                    maininfo.HashName = abName;
                    maininfo.AddRef();
                    _abCache.Add(abName, maininfo);
                }
                else
                {
                    maininfo = null;
                }

                return maininfo;
            }
        }

        #endregion

        #region async加载的三个重载
        // 同步加载资源---泛型加载 简单直观 无需显示转换
        //Sprite sp = abManager.LoadResource<Sprite>("a_png", "a");
        public async UniTask<T> LoadResourceAsync<T>(string abName, string resName, CancellationToken token = default) where T : UnityEngine.Object
        {
            T res = default;
            if (ABPathHelper.SimulationMode)
            {
                T rt = await editorLoadResourceAsync<T>(abName, resName);
                if (rt != null)
                    return rt;
            }

            ABInfo ab = await LoadABPackageAsync(abName);
            if (ab != null)
                res = (T)await ab.AssetBundle.LoadAssetAsync<T>(resName).WithCancellation(token);
            else
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

            //返回资源
            return res;
        }


        //不指定类型 有重名情况下不建议使用 使用时需显示转换类型
        //Object sp = abManager.LoadResource("a_png", "a");
        public async UniTask<Object> LoadResourceAsync(string abName, string resName, CancellationToken token = default)
        {
            Object res = null;
            if (ABPathHelper.SimulationMode)
            {
                Object rt = await editorLoadResourceAsync<Object>(abName, resName);
                if (rt != null)
                    return rt;
            }
            ABInfo ab = await LoadABPackageAsync(abName);
            if (ab != null)
                res = await ab.AssetBundle.LoadAssetAsync(resName).WithCancellation(token);
            else
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);
            return res;
        }


        //利用参数传递类型，适合对泛型不支持的语言调用，使用时需强转类型
        //Object sp = abManager.LoadResource("a_png", "a", typeof(Texture2D));
        public async UniTask<Object> LoadResourceAsync(string abName, string resName, System.Type type, CancellationToken token = default)
        {
            Object res = null;
            if (ABPathHelper.SimulationMode)
            {
                Object rt = await editorLoadResourceAsync(abName, resName, type);
                if (rt != null)
                    return rt;
            }
            ABInfo ab = await LoadABPackageAsync(abName);
            if (ab != null)
                res = await ab.AssetBundle.LoadAssetAsync(resName).WithCancellation(token);
            else
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);
            return res;
        }
        #endregion

        //单个包卸载
        public void UnLoad(string abName)
        {
            if (_abCache.ContainsKey(abName))
            {
                ABInfo ab = _abCache[abName];
                ab.Unload();
            }
        }

        public void UnloadUnusedAssets()
        {
            foreach (var item in _abCache)
            {
                ABInfo info = item.Value;
                bool rt = info.Release();
                if (rt)
                {
                    _abCache.Remove(info.HashName);
                }
            }
        }

        private T editorLoadResource<T>(string abName, string resName) where T : Object
        {
#if UNITY_EDITOR
            T res = null;
            string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
            for (int i = 0; i < assetPaths.Length; i++)
            {
                if (res == null)
                {
                    res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);
                }
            }

            if (res == null)
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

            return res;
#else
            return null;
#endif
        }

        private Object editorLoadResource(string abName, string resName, System.Type type)
        {
#if UNITY_EDITOR
            Object res = null;
            string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
            for (int i = 0; i < assetPaths.Length; i++)
            {
                if (res == null)
                {
                    res = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPaths[i], type);
                }
            }

            if (res == null)
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

            return res;
#else
            return null;
#endif
        }

        private async UniTask<T> editorLoadResourceAsync<T>(string abName, string resName) where T : Object
        {
#if UNITY_EDITOR
            T res = default;
            string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
            for (int i = 0; i < assetPaths.Length; i++)
            {
                if (res == null)
                {
                    res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);
                }
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(1), ignoreTimeScale: false);
            if (res == null)
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

            return res;
#else
            return null;
#endif
        }

        private async UniTask<Object> editorLoadResourceAsync(string abName, string resName, System.Type type)
        {
#if UNITY_EDITOR
            Object res = default;
            string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
            for (int i = 0; i < assetPaths.Length; i++)
            {
                if (res == null)
                {
                    res = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPaths[i], type);
                }
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(1), ignoreTimeScale: false);
            if (res == null)
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

            return res;
#else
            return null;
#endif
        }


        #region 三种资源协程加载方式

        /// <summary>
        /// 提供异步加载----注意 这里加载AB包是同步加载，只是加载资源是异步
        /// </summary>
        /// <param name="abName">ab包名称</param>
        /// <param name="resName">资源名称</param>
        public void LoadResourceCoroutine(string abName, string resName, System.Action<Object> finishLoadObjectHandler)
        {
            ABInfo ab = LoadABPackage(abName);
            //开启协程 提供资源加载成功后的委托
            // StartCoroutine(LoadResCoroutine(ab, resName, finishLoadObjectHandler));
            LoadResCoroutine(ab.AssetBundle, resName, finishLoadObjectHandler).ToUniTask();
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
            ABInfo ab = LoadABPackage(abName);
            // StartCoroutine(LoadResCoroutine(ab, resName, type, finishLoadObjectHandler));
            LoadResCoroutine(ab.AssetBundle, resName, type, finishLoadObjectHandler).ToUniTask();
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
            ABInfo ab = LoadABPackage(abName);
            // StartCoroutine(LoadResCoroutine<T>(ab, resName, finishLoadObjectHandler));
            LoadResCoroutine<T>(ab.AssetBundle, resName, finishLoadObjectHandler).ToUniTask();
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

    }

}