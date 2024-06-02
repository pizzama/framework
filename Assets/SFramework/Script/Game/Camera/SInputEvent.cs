using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SFramework.GameCamera
{

    /// <summary>鼠标事件</summary>
    public class SInputEvent : MonoBehaviour
    {
        public DelegateInputEventHandle MouseEventHandle;
        

        private SInputParam[] mouseParams = new SInputParam[] { new SInputParam(), new SInputParam(), new SInputParam() };

        private void Update()
        {
            addMouseInputEventTypeJudge();
        }

        /// <summary>添加</summary>
        private void addMouseInputEventTypeJudge()
        {
            for (int i = 0; i < mouseParams.Length; i++)
            {
                mouseInputEventTypeJudge(i);
            }
        }

        /// <summary>鼠标左键输入事件判定</summary>
        private void mouseInputEventTypeJudge(int mouseKeyCode)
        {
            //鼠标按下
            if (Input.GetMouseButtonDown(mouseKeyCode))
            {
                mouseParams[mouseKeyCode].startTime = Time.realtimeSinceStartup;
                Vector3 pos = Input.mousePosition;
                mouseParams[mouseKeyCode].clickPos = pos;
                MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.Down, pos, mouseParams[mouseKeyCode].clickCount, mouseKeyCode);
            }

            //鼠标按住
            if (Input.GetMouseButton(mouseKeyCode))
            {
                mouseParams[mouseKeyCode].holdTime = Time.realtimeSinceStartup - mouseParams[mouseKeyCode].startTime;
                if (mouseParams[mouseKeyCode].holdTime >= SInputDefine.LONG_PRESS_SCALE)
                {
                    MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.LongPress, Input.mousePosition, mouseParams[mouseKeyCode].clickCount, mouseKeyCode);
                }
            }

            //鼠标移动
            if (Input.GetMouseButton(mouseKeyCode))
            {
                Vector3 pos = mouseParams[mouseKeyCode].clickPos;
                Vector3 curPos = Input.mousePosition;
                float dis = (pos - curPos).sqrMagnitude;
                if(dis > SInputDefine.MOVE_THRESHOLD)
                {
                    MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.Move, Input.mousePosition, mouseParams[mouseKeyCode].clickCount, mouseKeyCode);
                }

            }

            //鼠标抬起
            if (Input.GetMouseButtonUp(mouseKeyCode))
            {
                if (mouseParams[mouseKeyCode].holdTime >= SInputDefine.LONG_PRESS_SCALE) return;
                MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.Up, Input.mousePosition, mouseParams[mouseKeyCode].clickCount, mouseKeyCode);

                mouseParams[mouseKeyCode].t2 = Time.realtimeSinceStartup;
                if (mouseParams[mouseKeyCode].t2 - mouseParams[mouseKeyCode].t1 < SInputDefine.DOUBLE_CLICK_RATE)
                {
                    //Debug.Log(mouseKeyCode + ":双击!");
                    mouseParams[mouseKeyCode].clickCount++;
                    MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.DoubleClick, Input.mousePosition, mouseParams[mouseKeyCode].clickCount, mouseKeyCode);
                }
                else
                {
                    mouseParams[mouseKeyCode].clickCount = 1;
                }

                mouseParams[mouseKeyCode].t1 = mouseParams[mouseKeyCode].t2;
                MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.Click, Input.mousePosition, mouseParams[mouseKeyCode].clickCount, mouseKeyCode);
            }
        }

        public struct SInputParam
        {
            public Vector3 clickPos;
            public float startTime;
            public float holdTime;
            public float t1;
            public float t2;
            public int clickCount;
        }

    }

}