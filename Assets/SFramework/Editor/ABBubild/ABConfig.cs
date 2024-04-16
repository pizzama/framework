using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;

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

    public class BuildSetting
{
    //版本号
    public string Version = "";
    /// <summary>
    /// 资源版本号 1.0.0.0
    /// </summary>
    public string AssetsVersion = "";
    //版本号
    public string VersionCode = "";

    //是否debug
    public bool IsDebug = true;
    
    //是否打ab包
    public bool buildAB = false;
    
    //是否分包
    public bool useObb = false;

    public string pckName = "";

    public string tempBuildPath = "";
    /// <summary>
    /// 资源存放地址
    /// </summary>
    public string assetsBuildPath = "";

    /// <summary>
    /// 是否打包 Apk
    /// </summary>
    public bool isBuildPackage = false;

    /// <summary>
    /// 资源全进首包
    /// </summary>
    public bool isResourceAllInAPK = false;
    //
    // //是否IL2CPP
    // public bool IL2CPP = false;
    //
    // //是否开启动态合批
    // public bool DynamicBatching = false;
    //
    // //程序名称
    // public string Name = "";
}

    /// <summary>
    /// 打包配置
    /// </summary>
    [Serializable]
    public class ABConfig
    {
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
        public string VersionCode => MainVersion.Replace(".", "") + "000";// string.Format("{0:D03}", SubVersion);

        /// <summary>
        /// 资源打包
        /// </summary>
        [XmlElement]
        public bool BuildAB;
        /// <summary>
        /// 调试开关
        /// </summary>
        public bool IsDebug => Type == BuildType.Develop;
        /// <summary>
        /// 正式发布
        /// </summary>
        public bool UseObb => Type == BuildType.Publish;

        /// <summary>
        /// 配置文件名称
        /// </summary>
        public string ProfileName =>
            Type == BuildType.Develop ? ProfileNames [0]:
            Type == BuildType.Release ? ProfileNames[1] : ProfileNames[2];

        public static readonly string[] ProfileNames = new string[]
        {
            "LocalConfig-dev.ini",
            "LocalConfig-release.ini",
            "LocalConfig-gp.ini",
        };

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
                IsBuildPackage = true,
                Foldout = true,
            };
            buildConfig.UploadRoot = buildConfig.DefaultUploadRoot;
            buildConfig.PackageRoot = buildConfig.DefaultPackageRoot;
            return buildConfig;
        }
    }
}
