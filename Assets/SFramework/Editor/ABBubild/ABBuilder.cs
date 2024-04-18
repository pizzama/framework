using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace SFramework.Build
{
    public class ABBuilder : EditorWindow
    {
        private const string ConfigPath = "/Library/ABBuilder.dat";
        readonly string ConfigKey = "app_build_key";
        List<ABConfig> Configs = new List<ABConfig>();

        [MenuItem("SFrameWork/Editor/Build Window", false, 100)]
        public static void Open()
        {
            var window = GetWindow<ABBuilder>($"打包工具");
            window.Show();
        }

        private void readConfig()
        {
            var dataPath = System.IO.Path.GetFullPath(".");
            dataPath = dataPath.Replace("\\", "/");
            dataPath += ConfigPath;

            if (File.Exists(dataPath))
            {
                FileStream file = File.Open(dataPath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                Configs = bf.Deserialize(file) as List<ABConfig>;
                file.Close();
            }

            if (Configs.Count == 0)
            {
                Configs = new List<ABConfig>()
                {
                    ABConfig.Create(BuildTarget.Android, BuildType.Develop),
                    ABConfig.Create(BuildTarget.Android, BuildType.Release),
                    ABConfig.Create(BuildTarget.Android, BuildType.Publish),
                };
            }
        }

        private void writeConfig()
        {
            var dataPath = System.IO.Path.GetFullPath(".");
            dataPath = dataPath.Replace("\\", "/");
            dataPath += ConfigPath;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(dataPath);
            bf.Serialize(file, Configs);
            file.Close();
        }

        private void OnEnable()
        {
            readConfig();
        }

        private void OnDisable()
        {
            if (Configs.Count > 0)
                writeConfig();
            else
                Configs.Clear();
        }

        private void startBuild(ABConfig config)
        {
            ABBuildHelper.Build(config, false, config.Version);
        }

        //GUIStyle style = GUIStyle.none;
        //
        bool toggle = false;
        BuildTarget target = BuildTarget.Android;
        BuildType type = BuildType.Develop;

        //
        Vector2 scrollPosition = Vector2.zero;

        void OnGUI()
        {
            EditorGUILayout.Space();

            toggle = EditorGUILayout.BeginToggleGroup("添加新的打包配置", toggle);
            if (toggle)
            {
                target = (BuildTarget)EditorGUILayout.EnumPopup("BuildTarget: ", target);
                type = (BuildType)EditorGUILayout.EnumPopup("BuildType: ", type);

                if (GUILayout.Button("创建配置"))
                {
                    Configs.Add(ABConfig.Create(target, type));
                }
            }

            EditorGUILayout.EndToggleGroup();

            EditorGUILayout.Space();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);

            for (int i = 0; i < Configs.Count; i++)
            {
                bool foldout = EditorGUILayout.BeginFoldoutHeaderGroup(
                    Configs[i].Foldout,
                    $"{Configs[i].Key}"
                );

                if (Configs[i].Foldout != foldout)
                {
                    Configs[i].Foldout = foldout;
                }
                if (Configs[i].Foldout)
                {
                    Draw(Configs[i]);

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("开始打包"))
                    {
                        if (
                            EditorUtility.DisplayDialog(
                                "提示",
                                "确定要开始打包么？如果是升了新版本号请确定对应的资源目录是否需要拷贝资源否则时间会过长！",
                                "确定",
                                "取消"
                            )
                        )
                        {
                            try
                            {
                                startBuild(Configs[i]);
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);

                                if (
                                    EditorUtility.DisplayDialog(
                                        "提示",
                                        string.Format("游戏打包失败!{0}", e.ToString()),
                                        "确定"
                                    )
                                )
                                {
                                    // EditorUtility.RevealInFinder(Application.dataPath);
                                }
                            }
                        }
                    }

                    if (GUILayout.Button("删除配置"))
                    {
                        if (EditorUtility.DisplayDialog("提示", "确定要删除这条打包配置么？", "确定", "取消"))
                        {
                            Configs.RemoveAt(i);
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndFoldoutHeaderGroup();

                EditorGUILayout.Space();
            }

            EditorGUILayout.EndScrollView();
            GUIUtility.ExitGUI();   
        }

        public void Draw(ABConfig config)
        {
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUI.BeginDisabledGroup(true);
                    //GUI.enabled = false;
                    var target = (BuildTarget)
                        EditorGUILayout.EnumPopup("BuildTarget: ", config.Target);
                    if (config.Target != target)
                    {
                        config.Target = target;
                    }
                    var type = (BuildType)EditorGUILayout.EnumPopup("BuildType: ", config.Type);
                    if (config.Type != type)
                    {
                        config.Type = type;
                    }
                    //GUI.enabled = true;
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndVertical();
            }

            //EditorGUILayout.Space();

            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.HelpBox("主版本号格式：1.0.0", MessageType.Info);
                string manor = EditorGUILayout.TextField("主版本号：", config.MainVersion);
                if (config.MainVersion != manor)
                {
                    config.MainVersion = manor;
                }
                EditorGUILayout.HelpBox("子版本号，即资源版本号", MessageType.Info);
                int minor = EditorGUILayout.IntField("子版本号：", config.SubVersion);
                if (config.SubVersion != minor)
                {
                    config.SubVersion = minor;
                }
                EditorGUILayout.LabelField("Version: ", config.Version);
                EditorGUILayout.LabelField("VersionCode: ", config.VersionCode);
                EditorGUILayout.EndVertical();
            }

            //EditorGUILayout.Space();

            {
                EditorGUILayout.BeginVertical();
                bool buildAb = EditorGUILayout.Toggle("BuildAB: ", config.BuildAB);
                if (config.BuildAB != buildAb)
                {
                    config.BuildAB = buildAb;
                }
                bool isCopyToStreamingAssets = EditorGUILayout.Toggle(
                    "IsCopyToStreamingAssets: ",
                    config.IsCopyToStreamingAssets
                );
                if (config.IsCopyToStreamingAssets != isCopyToStreamingAssets)
                {
                    config.IsCopyToStreamingAssets = isCopyToStreamingAssets;
                }
                {
                    EditorGUI.BeginDisabledGroup(true);
                    //GUI.enabled = false;
                    EditorGUILayout.Toggle("IsDebug: ", config.IsDebug);
                    EditorGUILayout.Toggle("UseObb: ", config.UseObb);
                    //GUI.enabled = true;
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndVertical();
            }

            //EditorGUILayout.Space();

            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("资源包导出路径：", config.UploadRoot);
                if (GUILayout.Button("选择目录"))
                {
                    config.CheckUploadRoot();
                    config.UploadRoot = EditorUtility.OpenFolderPanel(
                        "资源包导出目录",
                        config.UploadRoot,
                        config.DefaultUploadFolder
                    );
                }
                EditorGUILayout.EndHorizontal();
            }

            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("游戏包导出路径：", config.PackageRoot);
                if (GUILayout.Button("选择目录"))
                {
                    config.CheckPackageRoot();
                    config.PackageRoot = EditorUtility.OpenFolderPanel(
                        "游戏包导出目录",
                        config.PackageRoot,
                        config.DefaultPackageFolder
                    );
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
