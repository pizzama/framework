using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SFramework
{
    public class ABManifest : System.IDisposable
    {
        private readonly Dictionary<string, string[]> _cacheDependencies = new Dictionary<string, string[]>();
        private AssetBundleManifest _manifest;
        private AssetBundle _mainBundle;
        private List<string> _allBundleVariants;
        private List<string> _allBundles;
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
            // string path = Path.Combine(Application.streamingAssetsPath, ABPathHelper.GetPlatformName());
            try
            {
                string path = ABPathHelper.GetResPathInPersistentOrStream(ABPathHelper.GetPlatformName());
                _mainBundle = AssetBundle.LoadFromFile(path);
                if (_mainBundle != null)
                {
                    _manifest = _mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleMainfest");
                    _allBundleVariants = _manifest.GetAllAssetBundlesWithVariant().ToList();
                    _allBundles = _manifest.GetAllAssetBundles().ToList();
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
            string[] result = default;
            if (_cacheDependencies.TryGetValue(bundleName, out result))
            {
                return result;
            }
#if UNITY_EDITOR
            result = AssetDatabase.GetAssetBundleDependencies(bundleName, true);
#else
            if (_manifest == null)
            {
                return default;
            }

            result = _manifest.GetAllDependencies(bundleName);
#endif
            _cacheDependencies[bundleName] = result;
            return result;
        }
    }

}
