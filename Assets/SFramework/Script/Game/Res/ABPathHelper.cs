
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SFramework
{
    public class ABPathHelper
    {
        public const string DefaultABPath = "data";
        // 资源路径，优先返回外存资源路径
        public static string GetResPathInPersistentOrStream(string relativePath)
        {
            string resPersistentPath = string.Format("{0}{1}/{2}", PersistentDataPath, DefaultABPath, relativePath);
            if (File.Exists(resPersistentPath))
            {
                // return "file://" + resPersistentPath;
                return resPersistentPath;
            }
            else
            {
                resPersistentPath = string.Format("{0}{1}/{2}", StreamingAssetsPath, DefaultABPath, relativePath);
                if(File.Exists(resPersistentPath))
                {
                    // return "file://" + resPersistentPath;
                    return resPersistentPath;
                }
                else
                {
                    throw new NotFoundException("not found file:" + resPersistentPath);
                }
            }
        }

        private static string _persistentDataPath;
        private static string _streamingAssetsPath;
        // ExtApp Package Path
        public static string PersistentDataPath
        {
            get
            {
                if (null == _persistentDataPath)
                {
                    _persistentDataPath = Application.persistentDataPath + "/";
                }

                return _persistentDataPath;
            }
        }

        // InAPP Package Path 
        public static string StreamingAssetsPath
        {
            get
            {
                if (null == _streamingAssetsPath)
                {
                    _streamingAssetsPath = Application.streamingAssetsPath + "/";
                }

                return _streamingAssetsPath;
            }
        }

        // ExtApp HotFix Package Path 
        public static string GetPlatformPath
        {
            get 
            {
                return StreamingAssetsPath + "/" + GetPlatformName() ; 
            }
        }
        
        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			return GetPlatformForAssetBundles(UnityEngine.Application.platform);
#endif
        }

        public static int GetPlatformBuildTarget()
        {
#if UNITY_EDITOR
            return (int)EditorUserBuildSettings.activeBuildTarget;
#else
			return (int)UnityEngine.Application.platform;
#endif
        }

        public static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.WSAPlayerARM:
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerX86:
                    return "WSAPlayer";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                    return "StandaloneWindows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                case RuntimePlatform.LinuxPlayer:
                    return "Linux";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }

#if UNITY_EDITOR
        public static string GetPlatformForAssetBundles(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.WSAPlayer:
                    return "WSAPlayer";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "StandaloneWindows";
#if !UNITY_2019_2_OR_NEWER
                case BuildTarget.StandaloneLinux:
#endif
                case BuildTarget.StandaloneLinux64:
#if !UNITY_2019_2_OR_NEWER
                case BuildTarget.StandaloneLinuxUniversal:
#endif
                    return "Linux";
#if !UNITY_2017_3_OR_NEWER
			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
#elif UNITY_5
			case BuildTarget.StandaloneOSXUniversal:
#else
                case BuildTarget.StandaloneOSX:
#endif
                    return "OSX";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }
#endif

        // 上一级目录
        public static string GetParentDir(string dir, int floor = 1)
        {
            string subDir = dir;

            for (int i = 0; i < floor; ++i)
            {
                int last = subDir.LastIndexOf('/');
                subDir = subDir.Substring(0, last);
            }

            return subDir;
        }

        public static void GetFileInFolder(string dirName, string fileName, List<string> outResult)
        {
            if (outResult == null)
            {
                return;
            }

            var dir = new DirectoryInfo(dirName);

            if (null != dir.Parent && dir.Attributes.ToString().IndexOf("System", StringComparison.Ordinal) > -1)
            {
                return;
            }

            var fileInfos = dir.GetFiles(fileName);
            outResult.AddRange(fileInfos.Select(fileInfo => fileInfo.FullName));

            var dirInfos = dir.GetDirectories();
            foreach (var dinfo in dirInfos)
            {
                GetFileInFolder(dinfo.FullName, fileName, outResult);
            }
        }

#if UNITY_EDITOR
        const string kSimulateAssetBundles = "SimulateAssetBundles"; //此处跟editor中保持统一，不能随意更改
#endif
        public static bool SimulationMode
        {
#if UNITY_EDITOR
            get { return UnityEditor.EditorPrefs.GetBool(kSimulateAssetBundles, true); }
            set { UnityEditor.EditorPrefs.SetBool(kSimulateAssetBundles, value); }
#else
            get { return false; }
            set {  }
#endif
        }

        /// <summary>
        /// 获取 所有的 Files
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="allFiles"></param>
        public static void GetAllFiles(DirectoryInfo directoryInfo, List<FileInfo> allFiles)
        {
            foreach (var file in directoryInfo.GetFiles())
            {
                allFiles.Add(file);
            }

            foreach (var director in directoryInfo.GetDirectories())
            {
                GetAllFiles(director, allFiles);
            }
        }
    }
}