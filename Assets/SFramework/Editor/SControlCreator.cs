using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SControlCreator : EditorWindow
{
    private string pathText = "";
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

        bool outFolder = false;
        outFolder = EditorGUILayout.BeginFoldoutHeaderGroup(outFolder, "Result Info");
        if (outFolder)
        {
            Vector2 scrollPosition = Vector2.zero;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);

            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.EndVertical();
    }

    private void createFolder()
    {
        int index = pathText.LastIndexOf("/");
        string path = pathText.Substring(0, index);
        string name = pathText.Substring(index + 1, pathText.Length - index - 1);
        string parentPath = Application.dataPath + "/Script/" + path;
        if (!Directory.Exists(parentPath))
        {
            DirectoryInfo info = Directory.CreateDirectory(parentPath);
            Debug.Log("Success Create Folder:" + parentPath);
        }
    }
}
