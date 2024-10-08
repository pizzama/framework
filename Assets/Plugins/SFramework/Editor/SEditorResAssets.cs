using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
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
            
            if (GUILayout.Button("Check for missing scripts"))
            {
                CheckForMissingScripts();
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
                List<string> newTags = new List<string>() { "$EXPORT$", "$UICamera$" };
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
        
        public void CheckForMissingScripts()
        {
            Object[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (GameObject obj in allObjects)
            {
                Component[] components = obj.GetComponents<Component>();
                foreach (Component component in components)
                {
                    if (component == null)
                    {
                        Debug.LogError($"Missing script on object: {obj.name}", obj);
                    }
                }
            }
        }
    }
}
