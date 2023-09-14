using System;
using UnityEditor;
using System.IO;
using SFramework.Tools;

namespace SFramework
{
	[InitializeOnLoad]
	public class ResSingleAssetsMenu
	{
		public const string AssetBundlesOutputPath = "AssetBundles";
		private const string Mark_AssetBundle = "Assets/SFramework/AssetBundle Mark";

		static ResSingleAssetsMenu()
		{
			Selection.selectionChanged = OnSelectionChanged;
		}

		public static void OnSelectionChanged()
		{
			var path = GetSelectedPathOrFallback();
			if (!string.IsNullOrEmpty(path))
			{
				Menu.SetChecked(Mark_AssetBundle, Marked(path));
			}
		}

		public static bool Marked(string path)
		{
			try
			{
				var ai = AssetImporter.GetAtPath(path);
				//var dir = new DirectoryInfo(path);
				//return string.Equals(ai.assetBundleName, dir.Name.Replace(".", "_").ToLower());
				string tempPath = filterData(path).ToLower();
				return string.Equals(ai.assetBundleName, tempPath);
			}
#pragma warning disable CS0168
			catch (Exception _)
#pragma warning restore CS0168
			{
				return false;
			}
		}

		public static void MarkAB(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				var ai = AssetImporter.GetAtPath(path);
				//var dir = new DirectoryInfo(path);

				if (Marked(path))
				{
					Menu.SetChecked(Mark_AssetBundle, false);
					ai.assetBundleName = null;
				}
				else
				{
					string tempPath = filterData(ai.assetPath);
					Menu.SetChecked(Mark_AssetBundle, true);
					ai.assetBundleName = tempPath;
				}
				AssetDatabase.RemoveUnusedAssetBundleNames();
			}
		}

		private static string filterData(string path)
        {
			int index = path.LastIndexOf("/");
			string tempPath = path.Substring(0, index);
			tempPath = tempPath.Replace("Assets/", "");
			tempPath = tempPath.Replace("Arts/", "");
			tempPath = tempPath.Replace("Art/", "");
			return tempPath;
		}


		[MenuItem(Mark_AssetBundle)]
		public static void MarkPTABDir()
		{
			var path = GetSelectedPathOrFallback();
			MarkAB(path);
		}

		public static string GetSelectedPathOrFallback()
		{
			var path = string.Empty;

			foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);

				if (!string.IsNullOrEmpty(path) && File.Exists(path))
				{
					return path;
				}
			}

			return path;
		}
	}
}
