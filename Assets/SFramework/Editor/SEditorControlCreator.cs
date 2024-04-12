using System;
using System.Collections.Generic;
using System.IO;
using SFramework.Extension;
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class SEditorControlCreator : EditorWindow
{
    private string pathText = "";
    private List<string> _logTxts = new List<string>();
    private int _selectIndex;
    string[] options = { "SceneView", "UIView" };
    private const string prefix = "App";

    [MenuItem("SFrameWork/Editor/SFramework Control Creator", false, 100)]
    public static void Open()
    {
        var window = GetWindow<SEditorControlCreator>($"Control Creator");
        window.Show();
    }

    private void OnGUI()
    {
        _selectIndex = SEditorGUI.Popup("ViewType:", _selectIndex, options);
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "Input path example:App is default prefix you only need submit path as namespace and name such as Test/Test1",
            MessageType.Info
        );
        pathText = EditorGUILayout.TextField("Input path:", pathText);
        EditorGUILayout.Space();
        if (GUILayout.Button("Create"))
        {
            createFolder(_selectIndex);
        }

        layoutLogInfo();
        EditorGUILayout.EndVertical();
    }

    private void createFolder(int viewType)
    {
        try
        {
            int index = pathText.LastIndexOf("/");
            string path = "";
            if (index > 0)
                path = pathText.Substring(0, index);
            string name = pathText.Substring(index + 1, pathText.Length - index - 1);
            string parentPath = Application.dataPath + "/Script/" + prefix + "/" + path;
            string nameSpace = path.Replace("/", ".");
            nameSpace = prefix + "." + nameSpace;

            SCreateTemplateScript sc = new SCreateTemplateScript(nameSpace, name);
            createControl(parentPath, sc);
            createModel(parentPath, sc);
            createView(parentPath, sc, viewType);

            parentPath = Application.dataPath + "/Arts/" + prefix + "/" + path;
            if (viewType == 0)
            {
                createScenePrefab(parentPath, sc);
            }
            else
            {
                createUIPrefab(parentPath, sc);
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        catch (Exception err)
        {
            logInfo(err.ToString());
            return;
        }
        logInfo("Success Over");
    }

    private void layoutLogInfo()
    {
        if (_logTxts.Count > 0)
        {
            bool outFolder = _logTxts.Count > 0;
            outFolder = EditorGUILayout.BeginFoldoutHeaderGroup(outFolder, "Log Info");
            if (outFolder)
            {
                Vector2 scrollPosition = Vector2.zero;
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);
                foreach (var text in _logTxts)
                {
                    EditorGUILayout.TextArea(text);
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

    private void logInfo(string content)
    {
        string rt = string.Format("[{0:F}] {1}", System.DateTime.Now, content);
        _logTxts.Add(rt);
    }

    private void createControl(string parentPath, SCreateTemplateScript script)
    {
        var scriptFile = string.Format(parentPath + "/{0}Control.cs", (script.GetName()));

        string content = script.CreateTemplateControl();

        if (!File.Exists(scriptFile))
        {
            scriptFile.GetFolderPath().CreateDirIfNotExists();
            File.WriteAllText(scriptFile, content);
        }
    }

    private void createModel(string parentPath, SCreateTemplateScript script)
    {
        var scriptFile = string.Format(parentPath + "/{0}Model.cs", (script.GetName()));

        string content = script.CreateTemplateModel();

        if (!File.Exists(scriptFile))
        {
            scriptFile.GetFolderPath().CreateDirIfNotExists();
            File.WriteAllText(scriptFile, content);
        }
    }

    private void createView(string parentPath, SCreateTemplateScript script, int index)
    {
        var scriptFile = string.Format(parentPath + "/{0}View.cs", (script.GetName()));

        string content = script.CreateTemplateView(index);

        if (!File.Exists(scriptFile))
        {
            scriptFile.GetFolderPath().CreateDirIfNotExists();
            File.WriteAllText(scriptFile, content);
        }
    }

    private void createUIPrefab(string parentPath, SCreateTemplateScript script)
    {
        var prefabFile = string.Format(parentPath + "/{0}View.prefab", (script.GetName()));
        GameObject gameObject = null;
        if (!File.Exists(prefabFile))
        {
            prefabFile.GetFolderPath().CreateDirIfNotExists();
            try
            {
                gameObject = new GameObject("EmptyPrefab");
                gameObject.AddComponent<RectTransform>();
                bool result;
                UnityEngine.Object obj = PrefabUtility.SaveAsPrefabAsset(
                    gameObject,
                    prefabFile,
                    out result
                );
                Destroy(gameObject);
                if (result)
                {
                    logInfo(string.Format("Create {0} Prefab Success", script.GetName()));
                }
                else
                {
                    logInfo(string.Format("Create {0} Prefab Error", script.GetName()));
                }
            }
            catch (System.Exception)
            {
                if(gameObject != null)
                    Destroy(gameObject);
            }
        }
        else
        {
            logInfo(string.Format("{0} Prefab Has already exists", script.GetName()));
        }
    }

    private void createScenePrefab(string parentPath, SCreateTemplateScript script)
    {
        var preSceneFile = string.Format(parentPath + "/{0}View.unity", (script.GetName()));
        if (!File.Exists(preSceneFile))
        {
            string[] assetPaths =
                UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(
                    "sfscene.sfs",
                    "sfscene"
                );
            SceneAsset asset = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPaths[0]);
            // SceneTemplate.CreateSceneTemplate(string sceneTemplatePath)
            preSceneFile.GetFolderPath().CreateDirIfNotExists();
            // SceneTemplateService.CreateTemplateFromScene(asset,  preSceneFile);
            logInfo(string.Format("Create {0} Scene Success", script.GetName()));
        }
    }
}
