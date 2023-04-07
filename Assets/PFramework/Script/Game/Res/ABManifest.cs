using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PFramework
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

        public bool ExistsBundleWithVariant(string bundleVariant)
        {
#if UNITY_EDITOR
            return AssetDatabase.GetAssetPathsFromAssetBundle(bundleVariant).Length != 0;
#endif
            if (_allBundleVariants == null || _allBundleVariants.Count <= 0)
            {
                return false;
            }

            return _allBundleVariants.Contains(bundleVariant);
        }

        public bool ExistsBundle(string bundlePath)
        {
#if UNITY_EDITOR
            return AssetDatabase.GetAssetPathsFromAssetBundle(bundlePath).Length != 0;
#endif
            if (_allBundles == null || _allBundles.Count <= 0)
            {
                return false;
            }

            return _allBundles.Contains(bundlePath);
        }


        public string[] GetSortedDependencies(string bundleName)
        {
            Dictionary<string, int> info = new Dictionary<string, int>();
            List<string> parents = new List<string>();
            CollectDependencies(parents, bundleName, info);
            string[] ss = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            return ss;
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

        private void CollectDependencies(List<string> parents, string assetBundleName, Dictionary<string, int> info)
        {
            parents.Add(assetBundleName);
            string[] deps = GetDependencies(assetBundleName);
            if (deps == default)
            {
                return;
            }

            foreach (string parent in parents)
            {
                if (!info.ContainsKey(parent))
                {
                    info[parent] = 0;
                }

                info[parent] += deps.Length;
            }

            foreach (string dep in deps)
            {
                if (parents.Contains(dep))
                {
                    throw new Exception($"包有循环依赖，请重新标记: {assetBundleName} {dep}");
                }

                CollectDependencies(parents, dep, info);
            }

            parents.RemoveAt(parents.Count - 1);
        }
    }

}
