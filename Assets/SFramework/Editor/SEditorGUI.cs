using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// EditorGUI ToolBox
/// select1 = EditorGUILayout.Popup(select1, names);
/// select2 = SEditorGUI.Popup(select2, names);
/// </summary>
public static class SEditorGUI
{

    /// <summary>
    /// Create a normal popup field。
    /// </summary>
    /// <param name="selectIndex"></param>
    /// <param name="displayedOptions"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static int Popup(string label, int selectIndex, string[] displayedOptions, params GUILayoutOption[] options)
    {

        if (displayedOptions == null || displayedOptions.Length == 0)
            return 0;

        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        string display = "(Empty)";

        if (selectIndex >= 0 && selectIndex < displayedOptions.Length)
            display = displayedOptions[selectIndex];
        
        GUILayout.BeginHorizontal();
        GUILayout.Label(label);

        if (GUILayout.Button(display, options))
        {
            CustomPopup popup = new CustomPopup();
            popup.select = selectIndex;
            popup.displayedOptions = displayedOptions;
            popup.info = new CustomPopupInfo(controlId, selectIndex);
            CustomPopupInfo.instance = popup.info;
            PopupWindow.Show(CustomPopupTempStyle.Get(controlId).rect, popup);
        }

        GUILayout.EndHorizontal();

        if (Event.current.type == EventType.Repaint)
        {
            CustomPopupTempStyle style = new CustomPopupTempStyle();
            style.rect = GUILayoutUtility.GetLastRect();
            CustomPopupTempStyle.Set(controlId, style);
        }
        return CustomPopupInfo.Get(controlId, selectIndex);
    }

}

/// <summary>
/// open popup select panel
/// </summary>
public class CustomPopup : PopupWindowContent
{
    public int select;
    public string[] displayedOptions;
    public bool hasOpen;
    string filter;
    public CustomPopupInfo info;

    Vector2 scrollPosition;
    public override void OnGUI(Rect rect)
    {
        editorWindow.minSize = new Vector2(200, 400);
        GUILayout.Label("Search:");
        filter = EditorGUILayout.TextField(filter);
        GUILayout.Space(20);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < displayedOptions.Length; i++)
        {
            string info = displayedOptions[i];

            if (this.filter != null && this.filter.Length != 0)
            {
                if (!info.Contains(this.filter))
                {
                    continue;
                }
            }

            if (select == i)
            {
                info = "--->" + info;
            }
            if (GUILayout.Button(info))
            {
                select = i;
                this.info.Set(i);
                editorWindow.Close();
            }
        }
        EditorGUILayout.EndScrollView();
    }

    public override void OnOpen()
    {
        hasOpen = true;
        base.OnOpen();
    }
}


/// <summary>
/// 自定义Popup的Style缓存可以有多个参数，不止是Rect，也可以自定义其他的
/// </summary>
public class CustomPopupTempStyle
{

    public Rect rect;

    static Dictionary<int, CustomPopupTempStyle> temp = new();

    public static CustomPopupTempStyle Get(int controlId)
    {
        if (!temp.ContainsKey(controlId))
        {
            return null;
        }
        CustomPopupTempStyle t;
        temp.Remove(controlId, out t);
        return t;
    }

    public static void Set(int controlId, CustomPopupTempStyle style)
    {
        temp[controlId] = style;
    }
}

/// <summary>
/// 存储popup的信息如选择等
/// </summary>
public class CustomPopupInfo
{
    public int SelectIndex { get; private set; }
    public int controlId;
    public bool used;
    public static CustomPopupInfo instance;

    public CustomPopupInfo(int controlId, int selectIndex)
    {
        this.controlId = controlId;
        this.SelectIndex = selectIndex;
    }

    public static int Get(int controlID, int selected)
    {
        if (instance == null)
        {
            return selected;
        }

        if (instance.controlId == controlID && instance.used)
        {
            GUI.changed = selected != instance.SelectIndex;
            selected = instance.SelectIndex;
            instance = null;
        }

        return selected;
    }

    public void Set(int selected)
    {
        SelectIndex = selected;
        used = true;
    }
}