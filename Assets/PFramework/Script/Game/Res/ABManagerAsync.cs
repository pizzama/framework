using UnityEngine;

namespace PFramework
{
    public partial class ABManager: MonoBehaviour
    {

        // //================三种资源异步加载方式======================
        // /// <summary>
        // /// 提供异步加载----注意 这里加载AB包是同步加载，只是加载资源是异步
        // /// </summary>
        // /// <param name="abName">ab包名称</param>
        // /// <param name="resName">资源名称</param>
        // public void LoadResourceAsync(string abName, string resName, System.Action<Object> finishLoadObjectHandler)
        // {
        //     AssetBundle ab = LoadABPackage(abName);
        //     //开启协程 提供资源加载成功后的委托
        //     StartCoroutine(LoadRes(ab, resName, finishLoadObjectHandler));
        // }


        // private IEnumerator LoadRes(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler)
        // {
        //     if (ab == null) yield break;
        //     //异步加载资源API
        //     AssetBundleRequest abr = ab.LoadAssetAsync(resName);
        //     yield return abr;
        //     //委托调用处理逻辑
        //     finishLoadObjectHandler(abr.asset);
        // }


        // //根据Type异步加载资源
        // public void LoadResourceAsync(string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        // {
        //     AssetBundle ab = LoadABPackage(abName);
        //     StartCoroutine(LoadRes(ab, resName, type, finishLoadObjectHandler));
        // }


        // private IEnumerator LoadRes(AssetBundle ab, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        // {
        //     if (ab == null) yield break;
        //     AssetBundleRequest abr = ab.LoadAssetAsync(resName, type);
        //     yield return abr;
        //     //委托调用处理逻辑
        //     finishLoadObjectHandler(abr.asset);
        // }


        // //泛型加载
        // public void LoadResourceAsync<T>(string abName, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
        // {
        //     AssetBundle ab = LoadABPackage(abName);
        //     StartCoroutine(LoadRes<T>(ab, resName, finishLoadObjectHandler));
        // }

        // private IEnumerator LoadRes<T>(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
        // {
        //     if (ab == null) yield break;
        //     AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
        //     yield return abr;
        //     //委托调用处理逻辑
        //     finishLoadObjectHandler(abr.asset as T);
        // }
    }
}