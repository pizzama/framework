using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.GameCamera
{
    /// <summary>触摸事件</summary>
    public class STouchEvent : MonoBehaviour
    {
        public DelegateInputEventHandle MouseEventHandle;
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began)
                    {
                        // if (onClickEvent != null) onClickEvent(SInputDefine.IsTouchUI(), i, touch.position, touch.tapCount);
                        //Debug.Log(i + "---" + "touch.tapCount:" + touch.tapCount+ "---touch.position:"+ touch.position);
                        // MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.Click, Input.mousePosition, mouseParams[mouseKeyCode].clickCount, mouseKeyCode);
                    }
                }
            }
        }
    }
}