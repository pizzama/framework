using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SFramework
{
    public class SEditorResAssets : EditorWindow
    {
        [MenuItem("SFrameWork/Editor/SFramework Assets Manager", false, 100)]
        public static void Open()
        {
            var window = GetWindow<SEditorResAssets>($"Assets Bundle Manager");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Set All AssetBundle From Assets/Arts Folder"))
            {
                // cleanAssetBundleFromArtsFolder();
                dealWithAllAssetBundleFromArtsFolder();
            }

            if (GUILayout.Button("Clean All AssetBundle From Assets/Arts Folder"))
            {
                cleanAssetBundleFromArtsFolder();
            }

            if (GUILayout.Button("Collect All Controls"))
            {
                collectControls();
            }

            if (GUILayout.Button("Add Default Using Tag"))
            {
                AddOrCreateSFrameworkUsingNewTag();
            }

            if (GUILayout.Button("Add SFramework Using Scripting Defines Symbols"))
            {
                AddDefaultScriptingDefineSymbols();
            }
            
            if (GUILayout.Button("Check Missing Scripts In All Projects"))
            {
                CheckMissingScriptsInProjects();
            }

            if (GUILayout.Button("Check Missing Scripts In Selected"))
            {
                CheckMissingScriptsInSelected();
            }

            EditorGUILayout.EndVertical();
        }

        private void dealWithAllAssetBundleFromArtsFolder()
        {
            try
            {
                ResAssetsMenu.MarkPathABDir();
            }
            catch (System.Exception err)
            {
                EditorUtility.DisplayDialog("Error", err.ToString(), "OK");
            }

            EditorUtility.DisplayDialog(
                "Success",
                "Deal All AssetBundle From Assets/Arts Folder is done",
                "OK"
            );
        }

        private void cleanAssetBundleFromArtsFolder()
        {
            try
            {
                ResCleanAssetsMenu.MarkPathABDir();
            }
            catch (System.Exception err)
            {
                EditorUtility.DisplayDialog("Error", err.ToString(), "OK");
            }

            EditorUtility.DisplayDialog(
                "Success",
                "Clean All AssetBundle From Assets/Arts Folder is done",
                "OK"
            );
        }

        private void collectControls()
        {
            try
            {
                ControlAssetsMenu.MarkPTABDir();
            }
            catch (System.Exception err)
            {
                EditorUtility.DisplayDialog("Error", err.ToString(), "OK");
            }

            EditorUtility.DisplayDialog("Success", "Collect All Controls From Scripts", "OK");
        }

        private void AddOrCreateSFrameworkUsingNewTag()
        {
            UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath(
                "ProjectSettings/TagManager.asset"
            );
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty("tags");
                List<string> newTags = new List<string>() { "$EXPORT$"};
                for (int i = 0; i < newTags.Count; i++)
                {
                    bool isFind = false;
                    string tagName = newTags[i];
                    for (int j = 0; j < tags.arraySize; ++j)
                    {
                        if (tags.GetArrayElementAtIndex(j).stringValue == tagName)
                        {
                            isFind = true;
                            continue; // Tag already present, nothing to do.
                        }
                    }

                    if (isFind == false)
                    {
                        tags.InsertArrayElementAtIndex(0); //可能会打印提示信息”Default GameObject Tag: xxx already registered“
                        tags.GetArrayElementAtIndex(0).stringValue = tagName;
                        so.ApplyModifiedProperties();
                        so.Update();
                    }
                }

                EditorUtility.DisplayDialog(
                    "Success",
                    "Add Or Create SFramework Using NewTag is Done",
                    "OK"
                );
            }
        }

        public void AddDefaultScriptingDefineSymbols()
        {           
            try
            {
                BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
                BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
                var namedBuildTarget = UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(targetGroup);
                
                string[] defaultNeedSymbols = {"AMPLIFY_SHADER_EDITOR", "UNITY_POST_PROCESSING_STACK_V2", "SCINEMACHINE"};
                string[] definesSymbols;
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, out definesSymbols);
                for (int i = 0; i < defaultNeedSymbols.Length; i++)
                {
                    string name = defaultNeedSymbols[i];
                    bool isFind = false;
                    for (int j = 0; j < definesSymbols.Length; j++)
                    {
                        if(name == definesSymbols[j])
                        {
                            isFind = true;
                        }
                    }

                    if(!isFind)
                    {

                        PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, name);
                    }
                    
                }
                // PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, "SCinemachine");

                EditorUtility.DisplayDialog(
                    "Success",
                    "Add Default Scripting Define Symbols is Done",
                    "OK"
                );
            }
            catch (System.Exception err)
            {
                Debug.LogError(err);
            }
           
        }
        
        private static void CheckMissingScriptsInSelected()
        {
            GameObject[] go = Selection.gameObjects;
            int count = 0;
            foreach (GameObject g in go)
            {
                count += FindMissingScriptsInGO(g);
            }
            Debug.Log("Found " + count + " missing scripts in selected objects.");
        }
        
        private static void CheckMissingScriptsInProjects()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab t:Scene");
            int count = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(".prefab"))
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    count += FindMissingScriptsInGO(prefab);
                }
                else if (path.EndsWith(".unity"))
                {
                    EditorSceneManager.OpenScene(path);
                    GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                    foreach (GameObject root in rootObjects)
                    {
                        count += FindMissingScriptsInGO(root);
                    }
                    EditorSceneManager.CloseScene(SceneManager.GetActiveScene(), true);
                }
            }
            Debug.Log("Found " + count + " missing scripts in the project.");
        }

        private static int FindMissingScriptsInGO(GameObject g)
        {
            int count = 0;
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    count++;
                    Debug.Log(g.name + " has an empty script attached in position: " + i, g);
                }
            }
            foreach (Transform childT in g.transform)
            {
                count += FindMissingScriptsInGO(childT.gameObject);
            }
            return count;
        }
    }
}
