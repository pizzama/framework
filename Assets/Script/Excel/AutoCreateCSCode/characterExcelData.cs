/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class characterExcelItem : ExcelItemBase
{
	/// <summary>
	/// 武将id
	/// </summary>>
	public string id;
	/// <summary>
	/// 姓名
	/// </summary>>
	public string chaname;
	/// <summary>
	/// 性别
	/// </summary>>
	public int sex;
}


public class characterExcelData : ExcelDataBase<characterExcelItem>
{
	public characterExcelItem[] items;

	public Dictionary<string,characterExcelItem> itemDic = new Dictionary<string,characterExcelItem>();

	public void Init()
	{
		itemDic.Clear();
		if(items != null && items.Length > 0)
		{
			for(int i = 0; i < items.Length; i++)
			{
				itemDic.Add(items[i].id, items[i]);
			}
		}
	}

	public characterExcelItem GetcharacterExcelItem(string id)
	{
		if(itemDic.ContainsKey(id))
			return itemDic[id];
		else
			return null;
	}
	#region --- Get Method ---

	public string GetChaname(string id)
	{
		var item = GetcharacterExcelItem(id);
		if(item == null)
			return default;
		return item.chaname;
	}

	public int GetSex(string id)
	{
		var item = GetcharacterExcelItem(id);
		if(item == null)
			return default;
		return item.sex;
	}

	#endregion
}


#if UNITY_EDITOR
public class characterAssetAssignment
{
	public static bool CreateAsset(ExcelMediumData excelMediumData, string excelAssetPath)
	{
		var allRowItemDicList = excelMediumData.GetAllRowItemDicList();
		if(allRowItemDicList == null || allRowItemDicList.Count == 0)
			return false;

		int rowCount = allRowItemDicList.Count;
		characterExcelData excelDataAsset = ScriptableObject.CreateInstance<characterExcelData>();
		excelDataAsset.items = new characterExcelItem[rowCount];

		for(int i = 0; i < rowCount; i++)
		{
			var itemRowDic = allRowItemDicList[i];
			excelDataAsset.items[i] = new characterExcelItem();
			excelDataAsset.items[i].id = itemRowDic["id"];
			excelDataAsset.items[i].chaname = itemRowDic["chaname"];
			excelDataAsset.items[i].sex = StringUtility.StringToInt(itemRowDic["sex"]);
		}
		if(!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string fullPath = Path.Combine(excelAssetPath,typeof(characterExcelData).Name) + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(fullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset,fullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif



