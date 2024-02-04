using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using SFramework.Extension;

public class SControlCreator : EditorWindow
{
    private string pathText = "";
    private List<string> logTxts = new List<string>();
    [MenuItem("SFrameWork/Editor/SFramework Control Creator", false, 100)]
    public static void Open()
    {
        var window = GetWindow<SControlCreator>($"Control Creator");
        window.Show();
    }


    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        bool pathFold = true;
        pathFold = EditorGUILayout.BeginFoldoutHeaderGroup(pathFold, "Set Out Path");
        if (pathFold)
        {
            EditorGUILayout.LabelField("Input Path");
            pathText = EditorGUILayout.TextArea(pathText);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space();
        if (GUILayout.Button("Create"))
        {
            createFolder();
        }

        layoutLogInfo();
        EditorGUILayout.EndVertical();
    }

    private void createFolder()
    {
        try
        {
            int index = pathText.LastIndexOf("/");
            string path = pathText.Substring(0, index);
            string name = pathText.Substring(index + 1, pathText.Length - index - 1);
            string parentPath = Application.dataPath + "/Script/" + path;
            string nameSpace = path.Replace("/", ".");

            SCreateTemplateScript sc = new SCreateTemplateScript(nameSpace, name);
            //if (!Directory.Exists(parentPath))
            //{
            //    DirectoryInfo info = Directory.CreateDirectory(parentPath);
            //    logInfo("Success Create Folder:" + parentPath);
            //}

            createControl(parentPath, sc);
            createModel(parentPath, sc);

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
        if (logTxts.Count > 0)
        {
            bool outFolder = logTxts.Count > 0;
            outFolder = EditorGUILayout.BeginFoldoutHeaderGroup(outFolder, "Log Info");
            if (outFolder)
            {
                Vector2 scrollPosition = Vector2.zero;
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);
                foreach (var text in logTxts)
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
        logTxts.Add(rt);
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

        string content = script.CreateTemplateView();

        if (!File.Exists(scriptFile))
        {
            scriptFile.GetFolderPath().CreateDirIfNotExists();
            File.WriteAllText(scriptFile, content);
        }
    }
}
