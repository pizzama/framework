
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PFramework
{
    public class ABPathHelper
    {
        // 资源路径，优先返回外存资源路径
        public static string GetResPathInPersistentOrStream(string relativePath)
        {
            string resPersistentPath = string.Format("{0}{1}", PersistentDataPath, relativePath);
            if (File.Exists(resPersistentPath))
            {
                return resPersistentPath;
            }
            else
            {
                return StreamingAssetsPath + relativePath;
            }
        }

        private static string _persistentDataPath;
        private static string _streamingAssetsPath;
        // 外部目录  
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

        // 内部目录
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
    }
}