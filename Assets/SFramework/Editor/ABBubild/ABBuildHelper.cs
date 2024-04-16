using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using SFramework.Extension;
using Newtonsoft.Json;

namespace SFramework.Build
{
    public static class ABBuildHelper
    {
        /// <summary>
        /// 打包操作
        /// </summary>
        /// <param name="platformType"> 平台 </param>
        /// <param name="buildSetting"> setting</param>
        public static void Build(
            ABConfig config,
            bool isHot = false,
            string AppVersion = ""
        )
        {
            string platformType = ABPathHelper.GetPlatformName();

            int buildTarget = ABPathHelper.GetPlatformBuildTarget();

            string uploadRoot = $"Upload/{platformType}/{config.Version}/";

            if (!string.IsNullOrEmpty(config.UploadRoot))
            {
                uploadRoot =
                    $"{config.UploadRoot}/{platformType}/{config.Version}/";
            }

            if (config.BuildAB)
            {
                var uploadAbPath = $"{uploadRoot}{platformType}";
                // 不能删除缓存（AB包缓存）
                uploadAbPath.CreateDirIfNotExists();

                var localRoot = ABPathHelper.StreamingAssetsPath;
                var localAbPath = $"{localRoot}/{platformType}";
                if (Directory.Exists(localAbPath))
                {
                    Directory.Delete(localAbPath, true);
                    // 强制刷新工程
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
                
                Directory.CreateDirectory(localAbPath);

                // 打包图集
                //Debug.Log("======================Start  PackAllAtlases======================");
                //SpriteAtlasUtility.PackAllAtlases(buildTarget, false);
                //Debug.Log("======================PackAllAtlases  Success======================");

                Debug.Log(
                    "======================Start BuildAssetBundles======================\n"
                        + uploadAbPath
                );
                var buildManifest = BuildPipeline.BuildAssetBundles(
                    uploadAbPath,
                    BuildAssetBundleOptions.DeterministicAssetBundle
                        | BuildAssetBundleOptions.IgnoreTypeTreeChanges
                        | BuildAssetBundleOptions.None //暂时使用LZMA 等待压缩和分包在使用LZ4
                    ,
                    (UnityEditor.BuildTarget)buildTarget
                );
                Debug.Log("======================BuildAssetBundles  Success======================");

                // 文件加密
                //EncryptionConfig();

                List<string> filterFiles = new List<string>();
                filterFiles.AddRange(ABConfig.ProfileNames);
                filterFiles.Add(ABConfig.LocalConfigUrl);
                //filterFiles.Add(PathHelper.LocalLangUrl);
                //filterFiles.Add(PathHelper.LocalMapUrl);

                // 从StreamingAssets拷贝到Upload
                localRoot.DirectoryCopy(uploadRoot, filterFiles, "meta");

                // // 生成资源清单文件
                // Debug.Log(
                //     "======================Start  GenerateVersionConfigInfo======================"
                // );
                // GenerateVersionConfigInfo(
                //     buildSetting.AssetsVersion,
                //     buildSetting.VersionCode,
                //     uploadRoot,
                //     isHot
                // );
                // Debug.Log(
                //     "======================GenerateVersionConfigInfo  Success======================"
                // );

                Debug.Log("======================Start  GenerateVersionInfo======================");
                GenerateVersionInfo(
                    config.VersionCode,
                    AppVersion,
                    uploadRoot
                );
                Debug.Log(
                    "======================GenerateVersionInfo  Success======================"
                );

                // 从Upload拷贝到StreamingAssets
                uploadAbPath.DirectoryCopy(localAbPath, null, "manifest");
                //
                File.Copy(
                    $"{uploadRoot}/{ABConfig.VersionUrl}",
                    $"{localRoot}/{ABConfig.VersionUrl}",
                    true
                );
                // File.Copy(
                //     $"{uploadRoot}/{ABConfig.VersionConfigUrl}",
                //     $"{localRoot}/{ABConfig.VersionConfigUrl}",
                //     true
                // );

                AssetDatabase.Refresh();

                // del upload LocalConfig.ini
                //File.Delete(Path.Combine(uploadRoot, PathHelper.LocalConfigUrl));
                //File.Delete(Path.Combine(uploadRoot, PathHelper.LocalLangUrl));

                //if (EditorUtility.DisplayDialog("提示", "资源打包成功!", "确定"))
                //{
                    //EditorUtility.RevealInFinder(uploadAbPath);
                    //EditorUtility.RevealInFinder(localAbPath);
                //}
            }

            if (config.IsBuildPackage)
            {
                var packagePath = $"Package/{platformType}/{config.Version}/";

                if (!string.IsNullOrEmpty(config.PackageRoot))
                {
                    packagePath =
                        $"{config.PackageRoot}/{platformType}/{config.Version}/";
                }

                Debug.Log("======================Start  BuildPackage======================");
                BuildPackage(config, packagePath);
                Debug.Log("======================BuildPackage  Success======================");

                //if (EditorUtility.DisplayDialog("提示", "游戏打包成功!", "确定"))
                {
                    EditorUtility.RevealInFinder(packagePath);
                }
            }
        }

        /// <summary>
        ///  Compress streaming assets file .
        /// </summary>
        public static void CompressStreamingAssetsFile()
        {
            // var dirInfo = new DirectoryInfo(ABPathHelper.StreamingAssetsPath);
            // if (!dirInfo.Exists)
            // {
            //     return;
            // }

            // var zipPath = $"{ABPathHelper.StreamingAssetsPath}/{ABConfig.CompressFileName}";
            // if (File.Exists(zipPath))
            // {
            //     File.Delete(zipPath);
            // }

            // var assetsBundleRoot = new DirectoryInfo(
            //     ABPathHelper.StreamingAssetsPath + ABPathHelper.GetPlatformName();
            // );
            // if (!assetsBundleRoot.Exists)
            // {
            //     throw new Exception("AssetsBundle folder is not exist.");
            // }
            // var allFiles = new List<FileInfo>();
            // ABPathHelper.GetAllFiles(assetsBundleRoot, allFiles);

            // for (var i = 0; i < allFiles.Count; )
            // {
            //     var file = allFiles[i];
            //     if (file.Extension == "meta")
            //     {
            //         allFiles.RemoveAt(i);
            //         continue;
            //     }
            //     i++;
            // }
            // ZIPHelper.ZipCompress(allFiles.Select(x => x.FullName).ToList(), zipPath);
            // foreach (var file in allFiles)
            // {
            //     if (file.Name.Contains(ABConfig.VersionConfigUrl))
            //     {
            //         continue;
            //     }

            //     if (!file.Exists)
            //     {
            //         continue;
            //     }
            //     var fileDir = file.Directory;
            //     file.Delete();
            //     if (fileDir.Name != "StreamingAssets")
            //     {
            //         fileDir.Delete(true);
            //     }
            // }
            // AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            // Debug.Log("Compress Streaming Assets file Successful.");
        }

    

        // private static void GenerateVersionConfigInfo(
        //     string version,
        //     string versionCode,
        //     string dir,
        //     bool isHot
        // )
        // {
        //     var versionConfig = new VersionConfig()
        //     {
        //         Version = version,
        //         VersionCode = versionCode
        //     };
        //     GenerateVersionConfig(dir, versionConfig, "");
        //     var json = JSON.Encode(versionConfig);
        //     var encryptionText = /* EncryptionHelper.Encrypt256(json);*/
        //     System.Text.Encoding.UTF8.GetBytes(json);
        //     using (
        //         FileStream fileStream = new FileStream(
        //             $"{dir}/{PathHelper.VersionConfigUrl}",
        //             FileMode.Create
        //         )
        //     )
        //     {
        //         fileStream.Write(encryptionText, 0, encryptionText.Length);
        //         fileStream.Flush();
        //         fileStream.Close();
        //     }

        //     if (isHot)
        //     {
        //         var versionList = new List<string>();
        //         versionList.Add($"{dir}/{PathHelper.VersionConfigUrl}");

        //         var zipName = $"{PathHelper.VersionConfigUrl}.zip";

        //         ZIPHelper.ZipCompressSingle(
        //             PathHelper.VersionConfigUrl,
        //             $"{dir}/{PathHelper.VersionConfigUrl}",
        //             $"{dir}/{zipName}"
        //         );

        //         //if(File.Exists($"{dir}/{PathHelper.VersionConfigUrl}"))
        //         //{
        //         //    File.Delete($"{dir}/{PathHelper.VersionConfigUrl}");
        //         //}
        //     }
        // }

        private static void GenerateVersionInfo(
            string version,
            string versionCode,
            string dir
        )
        {
            ABVersion v = new ABVersion() {version = version, versionCode = versionCode };
            using (
                FileStream fileStream = new FileStream(
                    $"{dir}/{ABConfig.VersionUrl}",
                    FileMode.Create
                )
            )
            {
                var json = JsonConvert.SerializeObject(v);
                var bytes = System.Text.Encoding.Default.GetBytes(json);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Flush();
                fileStream.Close();
            }
        }

        private static void EncryptionConfig()
        {
            /*
            if (!Directory.Exists(PathHelper.InAppResPath))
            {
                return;
            }
            var directoryInfo = new DirectoryInfo(PathHelper.InAppResPath);
            foreach (var file in directoryInfo.GetFiles("*.txt",SearchOption.AllDirectories))
            {
                var text = File.ReadAllText(file.FullName);
                text = EncryptionHelper.Encrypt256(text);
                File.WriteAllText();
            }
            */
        }

        // private static void GenerateVersionConfig(
        //     string dir,
        //     VersionConfig versionConfig,
        //     string relativePath
        // )
        // {
        //     foreach (var file in Directory.GetFiles(dir))
        //     {
        //         FileInfo fi = new FileInfo(file);
        //         if (PathHelper.IsLocalFile(fi.Name))
        //         {
        //             continue;
        //         }

        //         if (fi.Extension.Contains(".manifest"))
        //         {
        //             // manifest not build
        //             continue;
        //         }

        //         string md5 = MD5Helper.FileMD5(file);
        //         long size = fi.Length;
        //         string filePath = relativePath == "" ? fi.Name : $"{relativePath}/{fi.Name}";

        //         versionConfig.FileDict.Add(
        //             filePath,
        //             new FileVersionConfig()
        //             {
        //                 FilePath = filePath,
        //                 Size = size,
        //                 MD5 = md5
        //             }
        //         );
        //     }

        //     foreach (var directory in Directory.GetDirectories(dir))
        //     {
        //         var directoryInfo = new DirectoryInfo(directory);
        //         var rel =
        //             relativePath == ""
        //                 ? directoryInfo.Name
        //                 : $"{relativePath}/{directoryInfo.Name}";
        //         GenerateVersionConfig($"{dir}/{directoryInfo.Name}", versionConfig, rel);
        //     }
        // }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="platformType"></param>
        /// <param name="setting"></param>
        /// <param name="rootUrl"></param>
        private static void BuildPackage(
            ABConfig config,
            string rootUrl
        )
        {
            //SVNTools.SVNToolUpdateAll();
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var packageName =
                PlayerSettings.productName.Replace(" ", "").Replace(":", "")
                + "_"
                + config.Version
                + "_"
                + config.VersionCode
                + (config.IsDebug ? "_Debug_" : "_Release_")
                + DateTime.Now.ToString("MMdd_HHmm");

            rootUrl = $"{rootUrl}{(config.IsDebug ? "DebugPackage" : "ReleasePackage")}/";
            var packageDirPath = (rootUrl + packageName);
            if (Directory.Exists(packageDirPath))
            {
                Directory.Delete(packageDirPath, true);
            }

            Directory.CreateDirectory(packageDirPath);
            string platformType = ABPathHelper.GetPlatformName();
            switch (platformType)
            {
                case "Android":
                    packageName += ".apk";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(platformType), platformType, null);
            }

            var playerOptions = new BuildPlayerOptions
            {
                locationPathName = packageDirPath + "/" + packageName,
                scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
            };

            if (playerOptions.scenes.Length <= 0)
            {
                throw new Exception("打包失败.");
            }

            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(playerOptions.scenes[0]);
            if (!Directory.Exists(rootUrl))
            {
                Directory.CreateDirectory(rootUrl);
            }

            EditorUserBuildSettings.allowDebugging = config.IsDebug;
            EditorUserBuildSettings.development = config.IsDebug;
            // EditorUserBuildSettings.connectProfiler = setting.IsDebug;
            EditorUserBuildSettings.buildWithDeepProfilingSupport = config.IsDebug;

            PlayerSettings.bundleVersion = config.Version;
            if (config.IsDebug)
            {
                playerOptions.options =
                    BuildOptions.Development
                    | BuildOptions.EnableDeepProfilingSupport
                    // | BuildOptions.ConnectWithProfiler
                    | BuildOptions.AllowDebugging;
            }
            try
            {
                BuildAndroid(config.IsDebug, playerOptions, config.UseObb, config.VersionCode);
            }
            catch (Exception e)
            {
                Debug.LogError("package error:" + e.ToString());
                return;
            }

            Debug.Log("打包成功");
        }

        private static void BuildAndroid(
            bool isDebug,
            BuildPlayerOptions playerOptions,
            bool useObb,
            string versionCode
        )
        {
            playerOptions.target = BuildTarget.Android;
            playerOptions.targetGroup = BuildTargetGroup.Android;

            PlayerSettings.Android.bundleVersionCode = int.Parse(versionCode.Trim());
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            EditorUserBuildSettings.androidCreateSymbolsZip = !isDebug;
            if (isDebug)
            {
                PlayerSettings.SetScriptingBackend(
                    BuildTargetGroup.Android,
                    ScriptingImplementation.Mono2x
                );
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            }
            else
            {
                PlayerSettings.SetScriptingBackend(
                    BuildTargetGroup.Android,
                    ScriptingImplementation.IL2CPP
                );
                AndroidArchitecture aac = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
                PlayerSettings.Android.targetArchitectures = aac;
            }
            PlayerSettings.Android.keystorePass = "egame123";
            PlayerSettings.Android.keyaliasPass = "egame123";
            PlayerSettings.Android.keystoreName = "./keystore/user.keystore";
            PlayerSettings.Android.keyaliasName = "pixieisland";
            PlayerSettings.Android.useAPKExpansionFiles = useObb;
            PlayerSettings.Android.buildApkPerCpuArchitecture = false;
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;

            Debug.Log("开始打包");
            Debug.Log("输出路径：" + playerOptions.locationPathName);
            BuildPipeline.BuildPlayer(playerOptions);
            var fileInfo = new FileInfo(playerOptions.locationPathName);
            var dirInfo = fileInfo.Directory;
            if (dirInfo == null)
            {
                return;
            }

            foreach (var dirFileInfo in dirInfo.GetFiles())
            {
                if (dirFileInfo == null)
                {
                    continue;
                }

                if (dirFileInfo.Extension.Contains("zip"))
                {
                    //dirFileInfo.Delete();
                    continue;
                }

                if (!useObb)
                {
                    continue;
                }

                if (!dirFileInfo.Extension.Equals(".obb"))
                {
                    continue;
                }

                System.Diagnostics.Debug.Assert(
                    dirFileInfo.Directory != null,
                    "dirFileInfo.Directory != null"
                );
                var newName =
                    $"{dirFileInfo.Directory.FullName}/main.{versionCode}.{Application.identifier}.obb";
                File.Move(dirFileInfo.FullName, newName);
                break;
            }
        }
    }
}
