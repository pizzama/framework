using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using PFramework;

namespace PFrameWork
{
    public partial class ABManager : MonoBehaviour
    {
        private static ABManager _instance;
        public static ABManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ABManager();
                    DontDestroyOnLoad(_instance.gameObject);
                    _instance.init();
                }
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
            AssetBundle ab;
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

        //==================三种资源同步加载方式==================
        //提供多种调用方式 便于其它语言的调用（Lua对泛型支持不好）
        #region 同步加载的三个重载

        /// <summary>
        /// 同步加载资源---泛型加载 简单直观 无需显示转换
        /// </summary>
        /// <param name="abName">ab包的名称</param>
        /// <param name="resName">资源名称</param>
        public T LoadResource<T>(string abName, string resName) where T : Object
        {
            //加载目标包
            AssetBundle ab = LoadABPackage(abName);

            //返回资源
            return ab.LoadAsset<T>(resName);
        }


        //不指定类型 有重名情况下不建议使用 使用时需显示转换类型
        public Object LoadResource(string abName, string resName)
        {
            //加载目标包
            AssetBundle ab = LoadABPackage(abName);

            //返回资源
            return ab.LoadAsset(resName);
        }


        //利用参数传递类型，适合对泛型不支持的语言调用，使用时需强转类型
        public Object LoadResource(string abName, string resName, System.Type type)
        {
            //加载目标包
            AssetBundle ab = LoadABPackage(abName);

            //返回资源
            return ab.LoadAsset(resName, type);
        }

        #endregion


        //================三种资源异步加载方式======================

        /// <summary>
        /// 提供异步加载----注意 这里加载AB包是同步加载，只是加载资源是异步
        /// </summary>
        /// <param name="abName">ab包名称</param>
        /// <param name="resName">资源名称</param>
        public void LoadResourceAsync(string abName, string resName, System.Action<Object> finishLoadObjectHandler)
        {
            AssetBundle ab = LoadABPackage(abName);
            //开启协程 提供资源加载成功后的委托
            StartCoroutine(LoadRes(ab, resName, finishLoadObjectHandler));
        }


        private IEnumerator LoadRes(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler)
        {
            if (ab == null) yield break;
            //异步加载资源API
            AssetBundleRequest abr = ab.LoadAssetAsync(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }


        //根据Type异步加载资源
        public void LoadResourceAsync(string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            AssetBundle ab = LoadABPackage(abName);
            StartCoroutine(LoadRes(ab, resName, type, finishLoadObjectHandler));
        }


        private IEnumerator LoadRes(AssetBundle ab, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync(resName, type);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }


        //泛型加载
        public void LoadResourceAsync<T>(string abName, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
        {
            AssetBundle ab = LoadABPackage(abName);
            StartCoroutine(LoadRes<T>(ab, resName, finishLoadObjectHandler));
        }

        private IEnumerator LoadRes<T>(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset as T);
        }

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