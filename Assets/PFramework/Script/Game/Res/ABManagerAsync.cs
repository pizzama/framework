using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PFramework
{
    public partial class ABManager : MonoBehaviour
    {
        //加载AB包
        private async UniTask<AssetBundle> LoadABPackageAsync(string abName)
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
                    ab = await AssetBundle.LoadFromFileAsync(ABPathHelper.GetResPathInPersistentOrStream(dependencies[i]));
                    //注意添加进缓存 防止重复加载AB包
                    _abCache.Add(dependencies[i], ab);
                }
            }
            //加载目标包
            if (_abCache.ContainsKey(abName)) return _abCache[abName];
            else
            {
                ab = await AssetBundle.LoadFromFileAsync(ABPathHelper.GetResPathInPersistentOrStream(abName));
                _abCache.Add(abName, ab);
                return ab;
            }
        }

        #region async加载的三个重载
        // 同步加载资源---泛型加载 简单直观 无需显示转换
        //Sprite sp = abManager.LoadResource<Sprite>("a_png", "a");
        public async UniTask<T> LoadResourceAsync<T>(string abName, string resName, CancellationToken token) where T : Object
        {
            T res = null;
#if UNITY_EDITOR
            if (ABPathHelper.SimulationMode)
            {
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
            }
#endif
            //加载目标包
            AssetBundle ab = await LoadABPackageAsync(abName);
            res = (T)await ab.LoadAssetAsync<T>(resName).WithCancellation(token);
            //返回资源
            return res;
        }


        //不指定类型 有重名情况下不建议使用 使用时需显示转换类型
        //Object sp = abManager.LoadResource("a_png", "a");
        public async UniTask<Object> LoadResourceAsync(string abName, string resName)
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

                if (res == null)
                    Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

                return res;
            }
#endif
            //加载目标包
            AssetBundle ab = await LoadABPackageAsync(abName);
            if (ab == null)
                Debug.LogError("Failed Load Asset:" + abName + ":" + resName);
            //返回资源
            return await ab.LoadAssetAsync(resName);
        }


        //利用参数传递类型，适合对泛型不支持的语言调用，使用时需强转类型
        //Object sp = abManager.LoadResource("a_png", "a", typeof(Texture2D));
        public async UniTask<Object> LoadResourceAsync(string abName, string resName, System.Type type)
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

                if (res == null)
                    Debug.LogError("Failed Load Asset:" + abName + ":" + resName);

                return res;
            }
#endif
            //加载目标包
            AssetBundle ab = await LoadABPackageAsync(abName);

            //返回资源
            return await ab.LoadAssetAsync(resName, type);
        }

        #endregion
    }
}