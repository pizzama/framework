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
		public const string AssetBundlesOutputPath = "AssetBundles";
		private const string Mark_AssetBundle = "Assets/SFramework/AssetBundle Folder";

		[MenuItem(Mark_AssetBundle)]
		public static void MarkPTABDir()
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
					OnWriteConfig(sceneName, namePathDictionary);
				}
			}
			AssetDatabase.Refresh();
			AssetDatabase.RemoveUnusedAssetBundleNames();
			Debug.Log("设置标记成功...");
		}

		/// <summary>
		/// 记录配置文件
		/// </summary>
		/// <param name="sceneDirectory"></param>
		/// <param name="namePathDictionary"></param>
		private static void OnWriteConfig(string sceneName, Dictionary<string, string> namePathDictionary)
		{
			string path = Application.dataPath + "/AssetBundles/" + sceneName;

			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			Debug.Log(path);
			using (FileStream fs = new FileStream(path + "/Record.txt", FileMode.OpenOrCreate, FileAccess.Write))
			{
				using (StreamWriter sw = new StreamWriter(fs))
				{
					sw.WriteLine(namePathDictionary.Count);
					foreach (KeyValuePair<string, string> kv in namePathDictionary)
					{
						Debug.Log(kv.Value);
						sw.WriteLine(kv.Key + "/" + kv.Value);
					}
				}
			}
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
					SetLables(fileInfo, sceneNama, namePathDictionary);
				}
			}
		}

		/// <summary>
		/// 修改资源 assetbundle lables
		/// </summary>
		private static void SetLables(FileInfo fileInfo, string sceneName, Dictionary<string, string> namePathDictionary)
		{
			if (fileInfo.Extension == ".meta") return;
			string bundleName = GetBundleName(fileInfo, sceneName);
			int index = fileInfo.FullName.IndexOf("Assets");
			string assetPath = fileInfo.FullName.Substring(index);
			AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
			assetImporter.assetBundleName = bundleName;
			if (fileInfo.Extension == ".unity")
				assetImporter.assetBundleVariant = "u3d";
			else
				assetImporter.assetBundleVariant = "sf";
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

				return sceneName + "/" + tmp[0];
			}
			return sceneName;
		}
	}
}
