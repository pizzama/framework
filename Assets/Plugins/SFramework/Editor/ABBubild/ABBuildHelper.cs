using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SFramework.Extension;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.U2D;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SFramework.Build
{
    public static class ABBuildHelper
    {
        /// <summary>
        /// Package Operate Helper
        /// </summary>
        public static void Build(ABConfig config, bool isHot = false, string AppVersion = "")
        {
            string platformType = config.Target.ToString();
            BuildTarget buildTarget = config.Target;
            string uploadRoot = $"Upload/{platformType}/{config.Version}/";
            if (!string.IsNullOrEmpty(config.UploadRoot))
            {
                uploadRoot = $"{config.UploadRoot}/{platformType}/{config.Version}/";
            }

            if (config.BuildAB)
            {
                var uploadAbPath = $"{uploadRoot}{platformType}";
                // 不能删除缓存（AB包缓存）
                uploadAbPath.CreateDirIfNotExists();

                var localRoot = ABPathHelper.StreamingAssetsPath;
                var localAbPath = $"{localRoot}/data";

                localAbPath.EmptyDirIfExists();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

                // 打包图集
                Debug.Log("======================Start  PackAllAtlases======================");
                SpriteAtlasUtility.PackAllAtlases(buildTarget, false);
                Debug.Log("======================PackAllAtlases  Success======================");

                Debug.Log("======================Start BuildAssetBundles======================\n" + uploadAbPath
                );
                try
                {
                    var time = Time.realtimeSinceStartup;
                    var buildManifest = BuildPipeline.BuildAssetBundles(uploadAbPath,
                        BuildAssetBundleOptions.ChunkBasedCompression
                        | BuildAssetBundleOptions.IgnoreTypeTreeChanges
                        | BuildAssetBundleOptions.None //暂时使用LZMA 等待压缩和分包在使用LZ4
                        , buildTarget
                    );
                    Debug.Log($"完成资源打包，总耗时：{Time.realtimeSinceStartup - time}");
                }
                catch (Exception err)
                {
                    Debug.LogException(err);
                    return;
                }

                Debug.Log("======================BuildAssetBundles  Success======================");

                // 文件加密
                //EncryptionConfig();

                // List<string> filterFiles = new List<string>();
                //filterFiles.Add(PathHelper.LocalLangUrl);
                //filterFiles.Add(PathHelper.LocalMapUrl);

                // 从StreamingAssets拷贝到Upload
                // localRoot.DirectoryCopy(uploadRoot, filterFiles, "meta");

                Debug.Log("======================Start  GenerateVersionInfo======================");
                GenerateVersionInfo(config.VersionCode, AppVersion, uploadRoot);
                Debug.Log(
                    "======================GenerateVersionInfo  Success======================"
                );

                // 从Upload拷贝到StreamingAssets
                if (config.IsCopyToStreamingAssets)
                {
                    uploadAbPath.DirectoryCopy(localAbPath, null, "manifest");
                }
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
                    packagePath = $"{config.PackageRoot}/{platformType}/{config.Version}/";
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

        private static void GenerateVersionInfo(string version, string versionCode, string dir)
        {
            ABVersion v = new ABVersion() { version = version, versionCode = versionCode };
            using (
                FileStream fileStream = new FileStream(
                    $"{dir}/{ABConfig.VersionUrl}",
                    FileMode.Create
                )
            )
            {
                var json = JsonUtility.ToJson(v);
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
        /// <param name="config"></param>
        /// <param name="rootUrl"></param>
        private static void BuildPackage(ABConfig config, string rootUrl)
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
            int platformType = ABPathHelper.GetPlatformBuildTarget();
            switch (platformType)
            {
                case (int)BuildTarget.Android:
                    packageName += $"/{packageName}.apk";
                    break;
                case (int)BuildTarget.WebGL:
                    packageName = "";
                    break;
                case (int)BuildTarget.iOS:
                    packageName = config.GetProjectPath();
                    break;
                case (int)BuildTarget.StandaloneWindows:
                case (int)BuildTarget.StandaloneWindows64:
                    packageName += $"/{packageName}.exe";
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
                switch (platformType)
                {
                    case (int)BuildTarget.Android:
                        BuildAndroid(config, playerOptions);
                        break;
                    case (int)BuildTarget.WebGL:
                        BuildWebGl(config, playerOptions);
                        break;
                    case (int)BuildTarget.iOS:
                        BuildIOS(config, playerOptions);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(platformType),
                            platformType,
                            null
                        );
                }
            }
            catch (Exception e)
            {
                Debug.LogError("package error:" + e.ToString());
                return;
            }

            Debug.Log("打包成功");
        }

        private static void BuildAndroid(
            ABConfig config,
            BuildPlayerOptions playerOptions
        )
        {
            playerOptions.target = BuildTarget.Android;
            playerOptions.targetGroup = BuildTargetGroup.Android;

            PlayerSettings.Android.bundleVersionCode = int.Parse(config.VersionCode.Trim());
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            if (config.IsDebug)
            {
                PlayerSettings.SetScriptingBackend(
                    BuildTargetGroup.Android,
                    ScriptingImplementation.Mono2x
                );
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
                EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Debugging;
            }
            else
            {
                PlayerSettings.SetScriptingBackend(
                    BuildTargetGroup.Android,
                    ScriptingImplementation.IL2CPP
                );
                AndroidArchitecture aac = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
                PlayerSettings.Android.targetArchitectures = aac;
                EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Public;
            }
            PlayerSettings.Android.keystorePass = "egame123";
            PlayerSettings.Android.keyaliasPass = "egame123";
            PlayerSettings.Android.keystoreName = "./keystore/user.keystore";
            PlayerSettings.Android.keyaliasName = "pixieisland";
            PlayerSettings.Android.useAPKExpansionFiles = config.UseObb;
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

                if (!config.UseObb)
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
                    $"{dirFileInfo.Directory.FullName}/main.{config.VersionCode}.{Application.identifier}.obb";
                File.Move(dirFileInfo.FullName, newName);
                break;
            }
        }

        private static void BuildWebGl( ABConfig config,
            BuildPlayerOptions playerOptions)
        {
            playerOptions.target = BuildTarget.WebGL;
            playerOptions.targetGroup = BuildTargetGroup.WebGL;

            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WebGL, ApiCompatibilityLevel.NET_4_6);
            if (config.IsDebug)
            {
                
            }
            else
            {
                
            }
            Debug.Log("开始打包");
            Debug.Log("输出路径：" + playerOptions.locationPathName);
            BuildPipeline.BuildPlayer(playerOptions);
        }
        
        private static void BuildIOS( ABConfig config,
            BuildPlayerOptions playerOptions)
        {
             playerOptions.target = BuildTarget.iOS;
            playerOptions.targetGroup = BuildTargetGroup.iOS;
            playerOptions.options = BuildOptions.None | BuildOptions.AcceptExternalModificationsToPlayer;
            //playerOptions.options |= BuildOptions.AcceptExternalModificationsToPlayer;
            //playerOptions.options = BuildOptions.StrictMode;

            EditorUserBuildSettings.iOSXcodeBuildConfig = config.IsDebug ? XcodeBuildConfig.Debug : XcodeBuildConfig.Release;
            EditorUserBuildSettings.buildAppBundle = false;
            // 不论是否是debug都打il2cpp
            // if (setting.IsDebug)
            // {
            //     PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.Mono2x);
            // }
            // else
            {
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            }
            PlayerSettings.iOS.targetOSVersionString = "13.0";

            Debug.Log("开始打包");
            Debug.Log("输出路径：" + playerOptions.locationPathName);
            try
            {
                var time = Time.realtimeSinceStartup;
                var report = BuildPipeline.BuildPlayer(playerOptions);
                var summary = report.summary;
                Debug.Log($"完成iOS打包，总耗时：{Time.realtimeSinceStartup - time}, result={summary.result}");
                if (summary.result == BuildResult.Succeeded)
                {
                    var xcodeProjectPath = playerOptions.locationPathName;
                    var packagePath = config.GetPackagePath();
                    var packageName = config.GetPackageName(PlayerSettings.productName);
                    packagePath += packageName;
                    //var archiveName = $"{packageName}.xcarchive";
                    //var ipaName = $"{packageName}.ipa";
                    BuildIpa(config.IsDebug, xcodeProjectPath, packagePath, packageName);

                    Debug.Log("打包成功");
                    return;
                }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
            
        Debug.LogError("打包失败");
        }
        
        private static void BuildIpa(bool debug, string xcodeProjectPath, string packagePath, string packageName)
        {
            var plistPath = Path.Combine(xcodeProjectPath, "Info.plist");
            var podfilePath = Path.Combine(xcodeProjectPath, "Podfile");
            
    #if UNITY_IPHONE
            // 修改xcode工程配置文件
            // var pbxPath = Path.Combine(xcodeProjectPath, "Unity-iPhone.xcodeproj/project.pbxobject");
            // var pbx = new PBXProject();
            // var target = pbx.TargetGuidByName("Unity-iPhone");
            // var targetGuid = pbx.GetUnityMainTargetGuid();
            // pbx.ReadFromFile(pbxPath);
            // pbx.Addxxx(target, );
            // pbx.Getxxx(target, );
            // pbx.Setxxx(target, );
            // pbx.RemoveFrameworkFromProject(targetGuid, "Push Notifications");
            // File.WriteAllText(pbxPath, pbx.WriteToString());
            
            // 修改plist文件
            Debug.Log("modify plist");
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            var infoDict = plist.root;
            infoDict.SetString("NSUserTrackingUsageDescription", "Your data will be used to deliver personalized ads to you.");
            infoDict.SetString("NSAdvertisingAttributionReportEndpoint", "https://adjust-skadnetwork.com");
            File.WriteAllText(plistPath, plist.WriteToString());
            
            // 修改podfile文件
            Debug.Log("modify podfile");
            //ExecuteCommand("python", pythonPath, null);
            
            var backupPath = Path.Combine(Application.dataPath, "../keystore/ioskeystore/Podfile");
            var podfile = File.ReadAllText(backupPath);
            File.WriteAllText(podfilePath, podfile);

    #endif
            
            // 通过CocosPods下载第三方库
            ExecuteCommand("pod install", xcodeProjectPath, null);
            
            // 暂时关闭后续功能（未完成）
            return;
            
            // Debug.Log("clean xcode project");
            // ExecuteCommand("xcodebuild", xcodeProjectPath, "clean -quiet");
            //
            // Debug.Log("export archive");
            // // if (Directory.Exists(packagePath))
            // //     Directory.Delete(packagePath, true);
            // // Directory.CreateDirectory(packagePath);
            // var workspace = Path.Combine(xcodeProjectPath, "Unity-iPhone.xcworkspace");
            // var archivePath = Path.Combine(packagePath, $"{packageName}.xcarchive");
            // var configuration = debug ? "Debug" : "Release";
            // var arguments = $"archive -workspace \"{workspace}\" -scheme \"Unity-iPhone\" -configuration \"{configuration}\" -sdk iphoneos -archivePath \"{archivePath}\" -quiet";
            // ExecuteCommand("xcodebuild", xcodeProjectPath, arguments);
            //
            // Debug.Log("export ipa");
            // //var ipaPath = Path.Combine(packagePath, $"{packageName}.ipa");
            // arguments = $"-exportArchive -archivePath \"{archivePath}\" -exportPath \"{packagePath}\" -exportOptionsPlist \"{plistPath}\" -quiet";
            // ExecuteCommand("xcodebuild", archivePath, arguments);
            //
            // Debug.Log("open dir");
            // Process.Start(packagePath);
        }
    
        private static void ExecuteCommand(string filename, string workDirectory, string arguments)
        {
            Debug.Log($"process start, filename={filename}, workDirectory={workDirectory}, arguments={arguments}");

            try
            {
    #if PROCESS_INFO
                var info = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    WorkingDirectory = workDirectory,
                    ErrorDialog = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                var process = Process.Start(info);
                if (process == null)
                {
                    Debug.LogError($"process is null! filename={filename}, workDirectory={workDirectory}, arguments={arguments}");
                    return;
                }
    #else
                var process = new Process();
                process.StartInfo.FileName = filename;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = workDirectory;
                process.StartInfo.ErrorDialog = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += (sender, e) => { Debug.Log($"sender={sender}, e={e.Data}"); };
                process.ErrorDataReceived += (sender, e) => { Debug.LogError($"sender={sender}, e={e.Data}"); };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
    #endif
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    Debug.LogError("process exit with error code " + process.ExitCode);
                }
                Debug.Log("StandardOutput.ReadToEnd() " + process.StandardOutput.ReadToEnd());
                Debug.Log("StandardError.ReadToEnd() " + process.StandardError.ReadToEnd());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
