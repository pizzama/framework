using UnityEditor;
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
                cleanAssetBundleFromArtsFolder();
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

            if (GUILayout.Button("test"))
            {
                test();
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

            EditorUtility.DisplayDialog("Success", "Deal All AssetBundle From Assets/Arts Folder is done", "OK");
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

        private void test()
        {
            try
            {
                string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle("game_turpworld_map.sfp");
                Debug.Log(assetPaths);
            }
            catch (System.Exception err)
            {
                EditorUtility.DisplayDialog("Error", err.ToString(), "OK");
            }
        }
    }
}