using System;
using UnityEditor;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace SFramework
{
	[InitializeOnLoad]
	public class ResCleanAssetsMenu
	{
		private const string StaticlNameSpace = "SFramework.Statics";
		private const string Mark_AssetBundle = "Assets/SFramework/AssetBundle Clean ALL Folder";
		private const string StaticClassName = "SFResAssets";
		private const string StaticPath = "SFStaticAsset";

		[MenuItem(Mark_AssetBundle)]
		public static void MarkPathABDir()
		{
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
			AssetDatabase.Refresh();
			AssetDatabase.RemoveUnusedAssetBundleNames();
		}

		private static void WriteDataConfig()
		{
			var path = Path.GetFullPath(Application.dataPath + Path.DirectorySeparatorChar + StaticPath);
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			path = path + "/" + StaticClassName + ".cs";
			var writer = new StreamWriter(File.Open(path, FileMode.Create));
			ResDataCodeGenerator.WriteClass(writer, StaticlNameSpace, StaticClassName);
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
			int index = fileInfo.FullName.IndexOf("Assets");
			string assetPath = fileInfo.FullName.Substring(index);
			AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
			if(assetImporter != null)
				assetImporter.assetBundleName = null;
		}
	}
}
