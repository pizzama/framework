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
        [SerializeField] private float moveSpeed = 1f;
        private bool isDrag = false;

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
                isDrag = false;
                stopMove(mouseKeyCode);
                mouseParams[mouseKeyCode].StartTime = Time.realtimeSinceStartup;
                Vector3 pos = Input.mousePosition;
                mouseParams[mouseKeyCode].PrevMousePos = pos;
                MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.Down, pos, mouseParams[mouseKeyCode].ClickCount, mouseKeyCode);
            }

            //鼠标按住
            if (Input.GetMouseButton(mouseKeyCode))
            {
                mouseParams[mouseKeyCode].HoldTime = Time.realtimeSinceStartup - mouseParams[mouseKeyCode].StartTime;
                if (mouseParams[mouseKeyCode].HoldTime >= SInputDefine.LONG_PRESS_SCALE)
                {
                    MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.LongPress, Input.mousePosition, mouseParams[mouseKeyCode].ClickCount, mouseKeyCode);
                }
            }

            //鼠标移动
            if (Input.GetMouseButton(mouseKeyCode))
            {
                Vector3 pos = mouseParams[mouseKeyCode].PrevMousePos;
                Vector3 curPos = Input.mousePosition;
                float dis = (pos - curPos).sqrMagnitude;
                if (dis > SInputDefine.MOVE_THRESHOLD)
                {
                    isDrag = true;
                    //偏差值
                    Vector3 offset = curPos - mouseParams[mouseKeyCode].PrevMousePos;
                    //瞬时速度
                    mouseParams[mouseKeyCode].PrevMousePos = curPos;
                    offset = offset * moveSpeed;
                    MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.Move, offset, mouseParams[mouseKeyCode].ClickCount, mouseKeyCode);
                }

            }

            //鼠标抬起
            if (Input.GetMouseButtonUp(mouseKeyCode))
            {
                if (mouseParams[mouseKeyCode].HoldTime >= SInputDefine.LONG_PRESS_SCALE) return;
                MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.Up, Input.mousePosition, mouseParams[mouseKeyCode].ClickCount, mouseKeyCode);

                if (isDrag == false)
                {
                    mouseParams[mouseKeyCode].T2 = Time.realtimeSinceStartup;
                    if (mouseParams[mouseKeyCode].T2 - mouseParams[mouseKeyCode].T1 < SInputDefine.DOUBLE_CLICK_RATE)
                    {
                        //Debug.Log(mouseKeyCode + ":双击!");
                        mouseParams[mouseKeyCode].ClickCount++;
                        MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.DoubleClick, Input.mousePosition, mouseParams[mouseKeyCode].ClickCount, mouseKeyCode);
                    }
                    else
                    {
                        mouseParams[mouseKeyCode].ClickCount = 1;
                    }

                    mouseParams[mouseKeyCode].T1 = mouseParams[mouseKeyCode].T2;
                    MouseEventHandle?.Invoke(SInputDefine.IsTouchUI(), SInputEventType.Click, Input.mousePosition, mouseParams[mouseKeyCode].ClickCount, mouseKeyCode);
                }

                isDrag = false;
            }
        }

        private void stopMove(int mouseKeyCode)
        {
            mouseParams[mouseKeyCode].PrevMousePos = Vector3.zero;
        }

        public struct SInputParam
        {
            public Vector3 PrevMousePos;
            public float StartTime;
            public float HoldTime;
            public float T1;
            public float T2;
            public int ClickCount;
        }

    }

}