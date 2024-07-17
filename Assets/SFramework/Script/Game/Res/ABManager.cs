using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SFramework
{
    public delegate void ABLoadCallBack(AssetBundle ab);

    public class ABManager
    {
        private const float _editorTestLoaderTime = 0.1f;
        private List<string> _abLoadQueue = new List<string>(); //记录正在加载的abName保证在异步环境下，ab重复加载的问题。
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

        public void Destroy()
        {
            UnloadAllAssets();
            if (_abCache != null)
                _abCache.Clear();
            _instance = null;
        }

        private Dictionary<string, ABInfo> _abCache;

        private ABManager()
        {
            _manifest = new ABManifest(); //主包的Bundle和依赖关系
            _manifest.LoadAssetBundleManifestAsync().Forget();
            //初始化字典
            _abCache = new Dictionary<string, ABInfo>();
        }

        private ABManifest _manifest;

        #region 同步加载的三个重载
        //加载AB包
        private ABInfo LoadABPackage(string abName)
        {
            ABInfo mainInfo = new ABInfo();
            AssetBundle ab = null;
            //根据manifest获取所有依赖包的名称 固定API
            string[] dependencies = _manifest.GetDependencies(abName.ToLower());
            //循环加载所有依赖包
            for (int i = 0; i < dependencies.Length; i++)
            {
                string dependencyName = dependencies[i];
                //如果不在缓存则加入,防止重复加载
                if (!_abCache.ContainsKey(dependencyName))
                {
                    string defaultName = ABPathHelper.DefaultABPath + "/" + dependencies[i];
                    //先加载外部地址在加载内部地址
                    if (!_abLoadQueue.Contains(dependencyName)) //锁定检查
                        _abLoadQueue.Add(dependencyName);
                    else
                    {
                        Debug.LogWarning("ab has in loading queue:" + dependencyName);
                        continue;
                    }
                    ab = AssetBundle.LoadFromFile(
                        ABPathHelper.GetResPathInPersistentOrStream(defaultName)
                    );
                    if (ab != null)
                    {
                        //注意添加进缓存 防止重复加载AB包
                        ABInfo info = new ABInfo();
                        info.HashName = dependencyName;
                        info.AssetBundle = ab;
                        mainInfo.AddDepends(info);
                        _abCache.Add(dependencyName, info);
                        _abLoadQueue.Remove(dependencyName); //加载完成后解除锁定
                    }
                }
                else
                {
                    ABInfo info = _abCache[dependencyName];
                    mainInfo.AddDepends(info);
                }
            }

            //加载目标包
            if (_abCache.ContainsKey(abName))
                return _abCache[abName];
            else
            {
                if (!_abLoadQueue.Contains(abName)) //锁定检查
                    _abLoadQueue.Add(abName);
                else
                {
                    Debug.LogWarning("ab has in loading queue:" + abName);
                    return null;
                }
                string defaultName = ABPathHelper.DefaultABPath + "/" + abName;
                ab = AssetBundle.LoadFromFile(
                    ABPathHelper.GetResPathInPersistentOrStream(defaultName)
                );
                mainInfo.HashName = abName;
                if (ab != null)
                {
                    mainInfo.AssetBundle = ab;
                    mainInfo.AddRef();
                    _abCache.Add(abName, mainInfo);
                    _abLoadQueue.Remove(abName); //加载完成后解除锁定
                }
                else
                {
                    mainInfo = null;
                }

                return mainInfo;
            }
        }

        public ABInfo LoadABInfo(string abName)
        {
            if (ABPathHelper.SimulationMode)
            {
                ABInfo info = new ABInfo();
                info.HashName = abName;
                return info;
            }
            //加载目标包
            ABInfo ab = LoadABPackage(abName);
            return ab;
        }

        // 同步加载资源---泛型加载
        //Sprite sp = abManager.LoadResource<Sprite>("a_png", "a");
        public T LoadResource<T>(string abName, string resName)
            where T : Object
        {
            if (ABPathHelper.SimulationMode)
            {
                T rt = editorLoadResource<T>(abName, resName);
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

        public List<T> LoadResourceWithSubResource<T>(string abName)
            where T : Object
        {
            if (ABPathHelper.SimulationMode)
            {
                List<T> rt = editorLoadResourceWithSubResource<T>(abName);
                return rt;
            }
            //加载目标包
            ABInfo ab = LoadABPackage(abName);
            return new List<T>(ab.AssetBundle.LoadAllAssets<T>());
        }
        #endregion

        #region async加载的三个重载
        //异步加载AB包
        private async UniTask<ABInfo> LoadABPackageAsync(string abName)
        {
            ABInfo mainInfo = new ABInfo();
            AssetBundle ab = null;
            //根据manifest获取所有依赖包的名称 固定API
            string[] dependencies = _manifest.GetDependencies(abName.ToLower());
            //循环加载所有依赖包
            for (int i = 0; i < dependencies.Length; i++)
            {
                string dependencyName = dependencies[i];
                //如果不在缓存则加入,防止重复加载
                if (!_abCache.ContainsKey(dependencyName))
                {
                    string defaultName = ABPathHelper.DefaultABPath + "/" + dependencyName;
                    //先加载外部地址在加载内部地址
                    if (!_abLoadQueue.Contains(dependencyName)) //锁定检查
                        _abLoadQueue.Add(dependencyName);
                    else
                    {
                        Debug.LogWarning("ab has in loading queue:" + dependencyName);
                        continue;
                    }
                    string url = ABPathHelper.GetResPathInPersistentOrStream(defaultName);
                    ab = await loadABFromPlantFromAsync(url);
                    if (ab != null)
                    {
                        //注意添加进缓存 防止重复加载AB包
                        ABInfo info = new ABInfo();
                        info.HashName = dependencyName;
                        info.AssetBundle = ab;
                        mainInfo.AddDepends(info);
                        _abCache.Add(dependencyName, info);
                    }
                }
                else
                {
                    ABInfo info = _abCache[dependencyName];
                    mainInfo.AddDepends(info);
                }
            }
            //加载目标包
            if (_abCache.ContainsKey(abName))
                return _abCache[abName];
            else
            {
                if (!_abLoadQueue.Contains(abName)) //锁定检查
                    _abLoadQueue.Add(abName);
                else
                {
                    Debug.LogWarning("ab has in loading queue:" + abName);
                    return null;
                }
                string defaultName = ABPathHelper.DefaultABPath + "/" + abName;
                string url = ABPathHelper.GetResPathInPersistentOrStream(defaultName);
                ab = await loadABFromPlantFromAsync(url);
                //注意添加进缓存 防止重复加载AB包
                if (ab != null)
                {
                    mainInfo.AssetBundle = ab;
                    mainInfo.HashName = abName;
                    mainInfo.AddRef();
                    _abCache.Add(abName, mainInfo);
                }
                else
                {
                    mainInfo = null;
                }

                return mainInfo;
            }
        }

        private async UniTask<AssetBundle> loadABFromPlantFromAsync(string url)
        {
            try
            {
                AssetBundle ab = null;
                if (ABPathHelper.GetPlatformName() == "WebGL")
                {
                    ab = await requestAssetBundleFromUrl(url, 0);
                }
                else
                {
                    ab = await AssetBundle.LoadFromFileAsync(url);
                    if (ab == null)
                    {
                        ab = await requestAssetBundleFromUrl(url, 0);
                    }
                }

                return ab;
            }
            catch (System.Exception err)
            {
                Debug.LogError(err);
                return null;
            }
        }

        private async UniTask<AssetBundle> requestAssetBundleFromUrl(string url, int index)
        {
            index++;
            if (index > 3)
            {
                return null;
            }
            //尝试从zip或者是远端获取
            UnityWebRequest abcR = UnityWebRequestAssetBundle.GetAssetBundle(url);
            await abcR.SendWebRequest();
            if (abcR.isDone)
            {
                AssetBundle ab = DownloadHandlerAssetBundle.GetContent(abcR);
                Debug.Log("222222:" + ab + ";" + abcR.isDone);
                abcR.Dispose();

                return ab;
            }
            else
            {
                AssetBundle tempAB = await requestAssetBundleFromUrl(url, index);
                return tempAB;
            }
        }

        public async UniTask<ABInfo> LoadABInfoAsync(string abName)
        {
            //加载目标包
            ABInfo ab = await LoadABPackageAsync(abName);
            return ab;
        }

        // 异步加载资源---泛型加载
        //Sprite sp = await abManager.LoadResourceAsync<Sprite>("a_png", "a");
        public async UniTask<T> LoadResourceAsync<T>(
            string abName,
            string resName,
            CancellationToken token = default
        )
            where T : UnityEngine.Object
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
        public async UniTask<Object> LoadResourceAsync(
            string abName,
            string resName,
            CancellationToken token = default
        )
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
        public async UniTask<Object> LoadResourceAsync(
            string abName,
            string resName,
            System.Type type,
            CancellationToken token = default
        )
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

        public async UniTask<List<T>> LoadResourceWithSubResourceAsync<T>(string abName)
            where T : Object
        {
            if (ABPathHelper.SimulationMode)
            {
                List<T> rt = editorLoadResourceWithSubResource<T>(abName);
                return rt;
            }
            //加载目标包
            ABInfo ab = await LoadABPackageAsync(abName);
            return new List<T>(ab.AssetBundle.LoadAllAssets<T>());
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

        public void UnloadAllAssets()
        {
            foreach (var item in _abCache)
            {
                ABInfo info = item.Value;
                info.Unload();
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

        #region 编辑器处理资源
        private T editorLoadResource<T>(string abName, string resName)
            where T : Object
        {
#if UNITY_EDITOR
            T res = null;
            string[] assetPaths =
                UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
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
            string[] assetPaths =
                UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
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

        private async UniTask<T> editorLoadResourceAsync<T>(string abName, string resName)
            where T : Object
        {
#if UNITY_EDITOR
            T res = default;
            string[] assetPaths =
                UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
            for (int i = 0; i < assetPaths.Length; i++)
            {
                if (res == null)
                {
                    res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);
                }
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(_editorTestLoaderTime), ignoreTimeScale: false);
            if (res == null)
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

            return res;
#else
            await UniTask.Delay(System.TimeSpan.FromSeconds(_editorTestLoaderTime), ignoreTimeScale: false);
            return null;
#endif
        }

        private async UniTask<Object> editorLoadResourceAsync(
            string abName,
            string resName,
            System.Type type
        )
        {
#if UNITY_EDITOR
            Object res = default;
            string[] assetPaths =
                UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, resName);
            for (int i = 0; i < assetPaths.Length; i++)
            {
                if (res == null)
                {
                    res = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPaths[i], type);
                }
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(_editorTestLoaderTime), ignoreTimeScale: false);
            if (res == null)
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

            return res;
#else
            await UniTask.Delay(System.TimeSpan.FromSeconds(_editorTestLoaderTime), ignoreTimeScale: false);
            return null;
#endif
        }

        private List<T> editorLoadResourceWithSubResource<T>(string abPath)
            where T : Object
        {
            List<T> result = new List<T>();
#if UNITY_EDITOR
            string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(abPath);
            for (int i = 0; i < assetPaths.Length; i++)
            {
                T res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);
                if (res != null)
                    result.Add(res);
            }
#endif
            return result;
        }

        private async UniTask<List<T>> editorLoadResourceWithSubResourceAsync<T>(string abPath)
            where T : Object
        {
            List<T> result = new List<T>();
#if UNITY_EDITOR
            string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(abPath);
            for (int i = 0; i < assetPaths.Length; i++)
            {
                T res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);
                if (res != null)
                    result.Add(res);
            }
#endif
            await UniTask.Delay(System.TimeSpan.FromSeconds(_editorTestLoaderTime), ignoreTimeScale: false);
            return result;
        }

        #endregion

        #region 三种资源协程加载方式

        /// <summary>
        /// 提供异步加载----注意 这里加载AB包是同步加载，只是加载资源是异步
        /// </summary>
        /// <param name="abName">ab包名称</param>
        /// <param name="resName">资源名称</param>
        public void LoadResourceCoroutine(
            string abName,
            string resName,
            System.Action<Object> finishLoadObjectHandler
        )
        {
            ABInfo ab = LoadABPackage(abName);
            //开启协程 提供资源加载成功后的委托
            // StartCoroutine(LoadResCoroutine(ab, resName, finishLoadObjectHandler));
            LoadResCoroutine(ab.AssetBundle, resName, finishLoadObjectHandler).ToUniTask();
        }

        private IEnumerator LoadResCoroutine(
            AssetBundle ab,
            string resName,
            System.Action<Object> finishLoadObjectHandler
        )
        {
            if (ab == null)
                yield break;
            //异步加载资源API
            AssetBundleRequest abr = ab.LoadAssetAsync(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }

        //根据Type异步加载资源
        public void LoadResourceCoroutine(
            string abName,
            string resName,
            System.Type type,
            System.Action<Object> finishLoadObjectHandler
        )
        {
            ABInfo ab = LoadABPackage(abName);
            // StartCoroutine(LoadResCoroutine(ab, resName, type, finishLoadObjectHandler));
            LoadResCoroutine(ab.AssetBundle, resName, type, finishLoadObjectHandler).ToUniTask();
        }

        private IEnumerator LoadResCoroutine(
            AssetBundle ab,
            string resName,
            System.Type type,
            System.Action<Object> finishLoadObjectHandler
        )
        {
            if (ab == null)
                yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync(resName, type);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }

        //泛型加载
        public void LoadResourceCoroutine<T>(
            string abName,
            string resName,
            System.Action<Object> finishLoadObjectHandler
        )
            where T : Object
        {
            ABInfo ab = LoadABPackage(abName);
            // StartCoroutine(LoadResCoroutine<T>(ab, resName, finishLoadObjectHandler));
            LoadResCoroutine<T>(ab.AssetBundle, resName, finishLoadObjectHandler)
                .ToUniTask();
        }

        private IEnumerator LoadResCoroutine<T>(
            AssetBundle ab,
            string resName,
            System.Action<Object> finishLoadObjectHandler
        )
            where T : Object
        {
            if (ab == null)
                yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset as T);
        }

        #endregion
    }
}
