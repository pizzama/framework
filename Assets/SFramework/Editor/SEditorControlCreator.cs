using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using SFramework.Extension;

public class SEditorControlCreator : EditorWindow
{
    private string pathText = "";
    private List<string> _logTxts = new List<string>();
    private int _selectIndex;
    string[] options = { "SceneView", "UIView" };

    [MenuItem("SFrameWork/Editor/SFramework Control Creator", false, 100)]
    public static void Open()
    {
        var window = GetWindow<SControlCreator>($"Control Creator");
        window.Show();
    }


    private void OnGUI()
    {
        _selectIndex = EditorGUI.Popup(
            new Rect(0, 0, position.width, 20),
            "ViewType:",
            _selectIndex,
            options);
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Input path example:App/Test", MessageType.Info);
        pathText = EditorGUILayout.TextField("Input path£º", "");
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
            string path = pathText.Substring(0, index);
            string name = pathText.Substring(index + 1, pathText.Length - index - 1);
            string parentPath = Application.dataPath + "/Script/" + path;
            string nameSpace = path.Replace("/", ".");

            SCreateTemplateScript sc = new SCreateTemplateScript(nameSpace, name);
            createControl(parentPath, sc);
            createModel(parentPath, sc);
            createView(parentPath, sc, viewType);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        catch(Exception err)
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

    private string readTemplateFile()
    {
        return "";
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
}
