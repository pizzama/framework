using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SHoleMask : Mask, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    public static List<object> mInputs = new List<object>();

    public override bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (!isActiveAndEnabled)
            return true;

        return !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, sp, eventCamera);
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (IsInclude(eventData.position)) {
            Pass(eventData, ExecuteEvents.pointerClickHandler);
        } else {
            mInputs.Clear();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
    }

    private bool IsInclude(Vector2 mousePos) {
        Vector2 temp = mousePos;
        RectTransform canvasRect = rectTransform.transform.root.GetComponent<RectTransform>();
        Canvas canvas = canvasRect.GetComponent<Canvas>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePos, canvas.worldCamera, out temp);

        Vector3 posInMask = temp;
        float width = rectTransform.rect.width, height = rectTransform.rect.height;
        Rect rect = new Rect();
        rect.min = new Vector2(rectTransform.anchoredPosition.x - width * 0.5f, rectTransform.anchoredPosition.y - height * 0.5f);
        rect.max = new Vector2(rectTransform.anchoredPosition.x + width * 0.5f, rectTransform.anchoredPosition.y + height * 0.5f);
        return rect.Contains(posInMask);
    }

    private void Pass<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler {
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++) {
            if (current != results[i].gameObject) {
                InputTempData<T> temp = new InputTempData<T>();
                temp.GameObject = results[i].gameObject;
                temp.PointerEventData = data;
                temp.EventFunction = function;
                mInputs.Add(temp);
            }
        }
    }

    public static bool ExecuteEventFunctions<T>(ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler {
        bool result = true;
        for (int i = 0; i < mInputs.Count; i++) {
            InputTempData<T> temp = mInputs[i] as InputTempData<T>;
            if (temp != null) {
                //ExecuteEvents.Execute(temp.GameObject, temp.PointerEventData, ExecuteEvents.pointerUpHandler);
                bool e = ExecuteEvents.Execute(temp.GameObject, temp.PointerEventData, ExecuteEvents.pointerClickHandler);
                if (e == false) {
                    result = false;
                }
                //ExecuteEvents.Execute(temp.GameObject, temp.PointerEventData, ExecuteEvents.pointerDownHandler);
            }
        }
        mInputs.Clear();
        return result;
    }

    public void SetOnDisable()
    {
        OnDisable();
    }
    public void SetOnEnable()
    {
        OnEnable();
    }


    //            protected override void OnDisable();
    //protected override void OnEnable();
}

public class InputTempData<T> where T : IEventSystemHandler {
    public GameObject GameObject { get; set; }
    public PointerEventData PointerEventData { get; set; }
    public ExecuteEvents.EventFunction<T> EventFunction { get; set; }
}
