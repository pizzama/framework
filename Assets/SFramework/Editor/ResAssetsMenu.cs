using System;
using UnityEditor;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace SFramework
{
	[InitializeOnLoad]
	public class ResAssetsMenu
	{
		private static List<string> noticeExtension = new List<string> { ".prefab", ".unity", ".png", ".jpg", ".asset", ".psd", ".mat", ".shader", ".controller", ".fbx", ".anim"};
		private static List<string> ignoreExtension = new List<string> { ".meta" };
		private const string StaticNameSpace = "SFramework.Statics";
		private const string Mark_AssetBundle = "Assets/SFramework/AssetBundle All Folder On Assets_Arts";
		private const string StaticClassName = "SFResAssets";
		private const string StaticPath = "SFStaticAsset";

		[MenuItem(Mark_AssetBundle)]
		public static void MarkPTABDir()
		{
			var selectedObject = Selection.activeObject;
			string assetPath = AssetDatabase.GetAssetPath(selectedObject);
			//string assetDirectory = Path.GetDirectoryName(assetPath);
			string assetDirectory = "Assets/Arts";
			DirectoryInfo directoryInfo = new DirectoryInfo(assetDirectory);
			DirectoryInfo[] scenesDirectories = directoryInfo.GetDirectories();
			foreach (var tempDir in scenesDirectories)
			{
				string sceneDirectory = assetDirectory + "/" + tempDir.Name;
				DirectoryInfo sceneDirectoryInfo = new DirectoryInfo(sceneDirectory);
				if (sceneDirectoryInfo == null)
				{
					Debug.Log(sceneDirectoryInfo + "not exists");
					return;
				}
				else
				{
					Dictionary<string, string> namePathDictionary = new Dictionary<string, string>();
					int index = sceneDirectory.LastIndexOf("/");
					string sceneName = sceneDirectory.Substring(index + 1);
					OnSceneFileSystemInfo(sceneDirectoryInfo, sceneName, namePathDictionary);
					//OnWriteConfig(sceneName, namePathDictionary);// write template file
				}
			}
			WriteDataConfig();
			AssetDatabase.Refresh();
			AssetDatabase.RemoveUnusedAssetBundleNames();
			Debug.Log("bundle setting over...");
		}

		private static void WriteDataConfig()
		{
			var path = Path.GetFullPath(Application.dataPath + Path.DirectorySeparatorChar + StaticPath);
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			path = path + "/" + StaticClassName + ".cs";
			var writer = new StreamWriter(File.Open(path, FileMode.Create));
			ResDataCodeGenerator.WriteClass(writer, StaticNameSpace, StaticClassName);
			writer.Close();
			AssetDatabase.Refresh();
		}


		private static void OnSceneFileSystemInfo(FileSystemInfo fileSystemInfo, string sceneNama, Dictionary<string, string> namePathDictionary)
		{
			if (!fileSystemInfo.Exists)
			{
				Debug.Log(fileSystemInfo + "not exists");
				return;
			}
			DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;

			FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
			foreach (var systemInfo in fileSystemInfos)
			{
				FileInfo fileInfo = systemInfo as FileInfo;
				if (fileInfo == null)
				{
					OnSceneFileSystemInfo(systemInfo, sceneNama, namePathDictionary);
				}
				else
				{
					SetLabels(fileInfo, sceneNama, namePathDictionary);
				}
			}
		}

		/// <summary>
		/// setting assetBundle labels
		/// </summary>
		private static void SetLabels(FileInfo fileInfo, string sceneName, Dictionary<string, string> namePathDictionary)
		{
			if (ignoreExtension.Contains(fileInfo.Extension))
				return;

			if (!noticeExtension.Contains(fileInfo.Extension))
				return;
			string bundleName = GetBundleName(fileInfo, sceneName);
			int index = fileInfo.FullName.IndexOf("Assets");
			string assetPath = fileInfo.FullName.Substring(index);
			AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
			assetImporter.assetBundleName = bundleName;
			if (fileInfo.Extension.Contains(".unity"))
			{
				assetImporter.assetBundleVariant = "sfs";
			}
			else
			{
				assetImporter.assetBundleVariant = "sfp";
			}
			string folderName;
			if (bundleName.Contains("/"))
				folderName = bundleName.Split('/')[1];
			else
				folderName = bundleName;
			string bundlePath = assetImporter.assetBundleName + "." + assetImporter.assetBundleVariant;
			if (!namePathDictionary.ContainsKey(folderName))
				namePathDictionary.Add(folderName, bundlePath);
		}

		private static string GetBundleName(FileInfo fileInfo, string sceneName)
		{
			string path = fileInfo.FullName;
			int index = path.IndexOf(sceneName) + sceneName.Length;
			string bundlePath = path.Substring(index + 1);
			bundlePath = bundlePath.Replace(@"\", "/");
			if (bundlePath.Contains("/"))
			{
				string[] tmp = bundlePath.Split('/');
				string all = sceneName;
				for (int i = 0; i < tmp.Length - 1; i++)
				{
					all += "_" + tmp[i];
				}

				return all;
			}
			return sceneName;
		}
	}
}
