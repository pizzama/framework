using System;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace SFramework.Build
{
    /// <summary>
    /// 打包类型
    /// </summary>
    [Serializable]
    public enum BuildType
    {
        Develop,
        Release,
        Publish,
    }

    [System.Serializable]
    public class ABVersion
    {
        public string version;
        public string versionCode;
    }

    /// <summary>
    /// 打包配置
    /// </summary>
    [Serializable]
    public class ABConfig
    {
        public const string CompressFileName = "Assets.zip";
        public const string VersionUrl = "Version.txt";
        public const string VersionConfigUrl = "VersionConfig.version";
        public string Key => $"{Target}-{Type}";

        /// <summary>
        /// 编译平台
        /// </summary>
        [XmlEnum]
        public BuildTarget Target;

        /// <summary>
        /// 编译类型
        /// </summary>
        [XmlEnum]
        public BuildType Type;

        /// <summary>
        /// 主版本号
        /// </summary>
        [XmlElement]
        public string MainVersion;

        /// <summary>
        /// 子版本号
        /// </summary>
        [XmlElement]
        public int SubVersion;
        public string Version => $"{MainVersion}.{SubVersion}";
        public string VersionCode => MainVersion.Replace(".", "") + "000"; // string.Format("{0:D03}", SubVersion);

        /// <summary>
        /// 资源打包
        /// </summary>
        [XmlElement]
        public bool BuildAB;

        /// <summary>
        /// wether or not copy to StreamingAssets
        /// </summary>
        public bool IsCopyToStreamingAssets;

        /// <summary>
        /// 调试开关
        /// </summary>
        public bool IsDebug => Type == BuildType.Develop;

        /// <summary>
        /// 正式发布
        /// </summary>
        public bool UseObb => Type == BuildType.Publish;

        /// <summary>
        /// 配置模板
        /// </summary>
        public static readonly ABConfig Default = new ABConfig()
        {
            Target = BuildTarget.Android,
            Type = BuildType.Develop,
            MainVersion = "1.0.0",
            SubVersion = 0,
            BuildAB = true,
            IsBuildPackage = true,
            IsCopyToStreamingAssets = false,
            Foldout = true,
        };

        #region Layout

        /// <summary>
        /// Whether Or Not BuildPackage
        /// </summary>
        [XmlElement]
        public bool IsBuildPackage;

        [XmlElement]
        public bool Foldout; //whether or not foldout tap
        #endregion

        [XmlElement]
        public string UploadRoot;
        public string DefaultUploadFolder => "Upload";
        public string DefaultUploadRoot => Application.dataPath.Replace("Assets", "");

        public void CheckUploadRoot()
        {
            if (string.IsNullOrEmpty(UploadRoot))
            {
                UploadRoot = DefaultUploadRoot;
            }
        }

        [XmlElement]
        public string PackageRoot;
        public string DefaultPackageFolder => "Package";
        public string DefaultPackageRoot => Application.dataPath.Replace("Assets", "");

        public void CheckPackageRoot()
        {
            if (string.IsNullOrEmpty(PackageRoot))
            {
                PackageRoot = DefaultPackageRoot;
            }
        }

        public static ABConfig Create(BuildTarget target, BuildType type)
        {
            var buildConfig = new ABConfig
            {
                Target = target,
                Type = type,
                MainVersion = PlayerSettings.bundleVersion,
                SubVersion = 0,
                BuildAB = true,
                IsCopyToStreamingAssets = false,
                IsBuildPackage = true,
                Foldout = true,
            };
            buildConfig.UploadRoot = buildConfig.DefaultUploadRoot;
            buildConfig.PackageRoot = buildConfig.DefaultPackageRoot;
            return buildConfig;
        }
    }
}
