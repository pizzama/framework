using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SFramework
{
    public class SEditorAssetsBundle : EditorWindow
    {
        [MenuItem("SFrameWork/Editor/SFramework Assets Bundle Manager", false, 100)]
        public static void Open()
        {
            var window = GetWindow<SEditorAssetsBundle>($"Assets Bundle Manager");
            window.Show();
        }


        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Set All AssetBundle From Assets/Arts Folder"))
            {
                dealWithAllAssetBundleFromArtsFolder();
            }

            if (GUILayout.Button("Clean All AssetBundle From Assets/Arts Folder"))
            {
                cleanAssetBundleFromArtsFolder();
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

            EditorUtility.DisplayDialog("Success", "Set All AssetBundle From Assets/Arts Folder is done", "OK");
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

            EditorUtility.DisplayDialog("Success", "Clean All AssetBundle From Assets/Arts Folder is done", "OK");
        }
    }
}