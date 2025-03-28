using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SFramework.GameCamera
{
    public class SInputDefine
    {
        ///<summary>移动的阈值</summary>
        public const float MOVE_THRESHOLD = 10;
        /// <summary>两次点击的时间间隔</summary>
        public const float DOUBLE_CLICK_RATE = 0.3f;
        /// <summary>连续按住0.5s后判定为长按</summary>
        public const float LONG_PRESS_SCALE = 0.5f;


        /// <summary>判断是否点击在UI上面</summary>
        public static bool IsTouchUI()
        {
            if (UnityEngine.EventSystems.EventSystem.current == null) return false;

            // if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer | Application.platform == RuntimePlatform.WebGLPlayer)
            // {
            //     if (Input.touchCount < 1) return false;
            //     if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return true;
            // }
            // else
            // {
            //     if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return true;
            // }

            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>通过layer名字判断是否点击在UI上面</summary>
        public static bool IsTouchUI(Vector3 mousePosition, string layerName = "UI")
        {
            var result = ReturnTouchOnRaycastResults(mousePosition);
            if (result == null)
            {
                return false;
            }

            var num = result.Select(r => r.gameObject).Count(go =>
                LayerMask.LayerToName(go.layer) == layerName
            );
            return num > 0;
        }
        
        public static List<RaycastResult> ReturnTouchOnRaycastResults(Vector3 mousePosition)
        {
            var rrList = new List<RaycastResult>();
            if (UnityEngine.EventSystems.EventSystem.current == null)
            {
                return rrList;
            }

            var pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            var vector3 = pointerEventData.pressPosition = (pointerEventData.position = mousePosition);
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointerEventData, rrList);
            return rrList;
        }
    }
    
    

    /// <summary>输入事件类型</summary>
    public enum SInputEventType
    {
        /// <summary>按下事件</summary>
        Down,
        /// <summary>松开事件</summary>
        Up,
        /// <summary>单击</summary>
        Click,
        /// <summary>双击</summary>
        DoubleClick,
        /// <summary>滑动</summary>
        Move,
        /// <summary>长按</summary>
        LongPress,
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="isTouchUI">是否触摸到UI</param>
    /// <param name="keyCode">(PC端)0表示左键，1表示右键，2表示中键</param>
    /// <param name="clickCount">连续点次数</param>
    public delegate void DelegateInputEventHandle(bool isTouchUI, SInputEventType enumInputEventType, Vector3 mousePosition, int clickCount, int keyCode);
}
