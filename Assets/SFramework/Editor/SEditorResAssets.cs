using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

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

                    if(isFind == false)
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
    }
}
