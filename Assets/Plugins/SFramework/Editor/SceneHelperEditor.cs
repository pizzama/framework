using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneHelperEditor : Editor
{
    [InitializeOnLoadMethod]
    private static void EditorInitializeMethod()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private static void OnEditorUpdate()
    {
        SelectLocationElement();
    }

    private static void SelectLocationElement()
    {
        if (!Input.GetMouseButton(1))
            return;

        var eventSystem = UnityEngine.EventSystems.EventSystem.current;
        if (!eventSystem)
            return;

        var uiPointerEventData = new PointerEventData(eventSystem);
        uiPointerEventData.position = Input.mousePosition;

        var uiRayCastResultCache = new List<RaycastResult>();
        eventSystem.RaycastAll(uiPointerEventData, uiRayCastResultCache);
        if (uiRayCastResultCache.Count > 0)
        {
            Selection.activeObject = uiRayCastResultCache[0].gameObject;
        }
    }
}
