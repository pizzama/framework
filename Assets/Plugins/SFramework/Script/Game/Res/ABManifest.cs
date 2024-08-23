using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace SFramework
{
    public class ABManifest : System.IDisposable
    {
        private const float _tryAgainTime = 0.2f;
        private const int _maxRetryCount = 10;
        private List<string> _abLoadQueue = new List<string>(); //记录正在加载的abName保证在异步环境下，ab重复加载的问题。
        private readonly Dictionary<string, string[]> _cacheDependencies =
            new Dictionary<string, string[]>();
        private AssetBundleManifest _manifest;
        private AssetBundle _mainBundle;
        private List<string> _allBundleVariants;
        private List<string> _allBundles;

        public bool HasAlreadyLoadedManifest => _manifest != null;

        public void Dispose()
        {
            if (_mainBundle != null)
            {
                _mainBundle.Unload(true);
                _mainBundle = null;
            }

            _cacheDependencies.Clear();
            _allBundleVariants?.Clear();
            _allBundleVariants = null;
            _allBundles?.Clear();
            _allBundles = null;
        }

        public void LoadAssetBundleManifest()
        {
            if (ABPathHelper.SimulationMode)
                return;
            try
            {
                if (_manifest != null)
                {
                    return;
                }
                //Get main Manifest
                string name = ABPathHelper.GetPlatformName();
                string defaultName = ABPathHelper.DefaultABPath + "/" + name;
                string path = ABPathHelper.GetResPathInPersistentOrStream(defaultName);
                _mainBundle = AssetBundle.LoadFromFile(path);
                if (_mainBundle != null)
                {
                    _manifest = _mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    if (_manifest != null)
                    {
                        _allBundleVariants = _manifest.GetAllAssetBundlesWithVariant().ToList();
                        _allBundles = _manifest.GetAllAssetBundles().ToList();
                    }
                    else
                    {
                        Debug.LogWarning("AssetBundleMainfest not found");
                    }
                }
                else
                {
                    Debug.LogWarning("not found mainBundle at path:" + path);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }

        public void LoadAssetBundleManifest(byte[] bytes)
        {
            _mainBundle = AssetBundle.LoadFromMemory(bytes);
            if (_mainBundle == null)
            {
                throw new Exception($"you load assetBundle is none. Manifest");
            }

            _manifest = _mainBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            if (_manifest == null)
            {
                throw new Exception($"you Manifest is none.");
            }

            _allBundleVariants = _manifest.GetAllAssetBundlesWithVariant().ToList();
            _allBundles = _manifest.GetAllAssetBundles().ToList();
        }

        public async UniTask LoadAssetBundleManifestAsync()
        {
            if (ABPathHelper.SimulationMode)
                return;
            try
            {
                if (_manifest != null)
                {
                    return;
                }
                //Get main Manifest
                string name = ABPathHelper.GetPlatformName();
                string defaultName = ABPathHelper.DefaultABPath + "/" + name;
                string path = ABPathHelper.GetResPathInPersistentOrStream(defaultName);
                if (!_abLoadQueue.Contains(name)) //锁定检查
                    _abLoadQueue.Add(name);
                else
                {
                    Debug.LogWarning("ab has in loading queue:" + name);
                    //异步等待直到加载完成
                    await UniTask.NextFrame();
                    while (_abLoadQueue.Contains(name))
                        await UniTask.NextFrame();
                    return;
                }
                _mainBundle = await loadABFromPlantFromAsync(path);
                if (_mainBundle != null)
                {
                    _manifest = _mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    if (_manifest != null)
                    {
                        _allBundleVariants = _manifest.GetAllAssetBundlesWithVariant().ToList();
                        _allBundles = _manifest.GetAllAssetBundles().ToList();
                    }
                    else
                    {
                        Debug.LogWarning("AssetBundleMainfest not found");
                    }
                }
                else
                {
                    Debug.LogWarning("not found mainBundle at path:" + path);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }

        private async UniTask<AssetBundle> loadABFromPlantFromAsync(string url)
        {
            UniTaskCompletionSource<AssetBundle> uniTaskCompletionSource =
                new UniTaskCompletionSource<AssetBundle>();
            try
            {
                AssetBundle ab = null;
                if (ABPathHelper.GetPlatformName() == "WebGL")
                {
                    ab = await requestManifestFromUrlAsync(url, 0);
                }
                else
                {
                    ab = await AssetBundle.LoadFromFileAsync(url);
                    if (ab == null)
                    {
                        ab = await requestManifestFromUrlAsync(url, 0);
                    }
                }

                uniTaskCompletionSource.TrySetResult(ab);
            }
            catch (System.Exception err)
            {
                Debug.LogError("loadABFromPlantFromAsync error:" + url + ";" + err);
                uniTaskCompletionSource.TrySetResult(null);
            }

            return await uniTaskCompletionSource.Task;
        }

        private async UniTask<AssetBundle> requestManifestFromUrlAsync(string url, int index)
        {
            index++;
            await UniTask.Delay(System.TimeSpan.FromSeconds(_tryAgainTime), ignoreTimeScale: false); //wait 1s
            if (index > _maxRetryCount)
            {
                return null;
            }
            //尝试从zip或者是远端获取
            UnityWebRequest abcR = UnityWebRequestAssetBundle.GetAssetBundle(url);
            await abcR.SendWebRequest();
            if (abcR.isDone)
            {
                AssetBundle ab = DownloadHandlerAssetBundle.GetContent(abcR);
                Debug.Log("manifest:" + ab + ";" + abcR.isDone);
                abcR.Dispose();

                return ab;
            }
            else
            {
                AssetBundle tempAB = await requestManifestFromUrlAsync(url, index);
                return tempAB;
            }
        }

        public string ExistsBundleWithVariant(string bundleVariant)
        {
#if UNITY_EDITOR
            string[] result = AssetDatabase.GetAssetPathsFromAssetBundle(bundleVariant);
            if (result.Length > 0)
                return result[0];
            else
                return "";
#else
            if (_allBundleVariants == null || _allBundleVariants.Count <= 0)
            {
                return "";
            }

            if (_allBundleVariants.Contains(bundleVariant))
                return bundleVariant;
            else
                return "";
#endif
        }

        public string ExistsBundle(string bundlePath)
        {
#if UNITY_EDITOR
            string[] result = AssetDatabase.GetAssetPathsFromAssetBundle(bundlePath);
            if (result.Length > 0)
                return result[0];
            else
                return "";
#else
            if (_allBundles == null || _allBundles.Count <= 0)
            {
                return "";
            }
            if (_allBundles.Contains(bundlePath))
                return bundlePath;
            else
                return "";
#endif
        }

        public string[] GetDependencies(string bundleName)
        {
            string[] result;
            if (_cacheDependencies.TryGetValue(bundleName, out result))
            {
                return result;
            }
#if UNITY_EDITOR
            if (ABPathHelper.SimulationMode)
            {
                result = AssetDatabase.GetAssetBundleDependencies(bundleName, true);
                return result;
            }
#endif
            if (_manifest == null)
            {
                Debug.LogWarning("manifest is null, please check your loading process");
                return new string[] { };
            }

            result = _manifest.GetAllDependencies(bundleName);

            _cacheDependencies[bundleName] = result;
            return result;
        }
    }
}
