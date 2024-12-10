using UnityEngine;

namespace SFramework.Tools
{
    public class MapTools
    {
        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3 GetDirToMouse(Vector3 fromPosition)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            return (mouseWorldPosition - fromPosition).normalized;
        }

        public static Vector3 ScreenToWorldPos(Vector3 pos)
        {
            return ScreenToWorldPos(pos, Camera.main);
        }

        public static Vector3 ScreenToWorldPos(Vector3 screenPos, Camera worldCamera)
        {
            if (worldCamera == null)
                return Vector3.zero;
            
            var viewRay = worldCamera.ScreenPointToRay(screenPos);
            var worldPos = viewRay.GetPoint(-viewRay.origin.z / viewRay.direction.z);
            return worldPos;
        }

        public static Vector3 WorldToUIPos(Vector3 worldPos, Camera worldCamera, Canvas canvas)
        {
            if (worldCamera == null)
                return Vector3.zero;
            Vector3 screenPos = worldCamera.WorldToScreenPoint(worldPos);
            return ScreenToUIPos(screenPos, canvas);
        }

        //UIpos using local position to deal with
        public static Vector3 WorldToUIPos(
            Vector3 worldPos,
            Camera worldCamera,
            Camera uiCamera,
            RectTransform trans
        )
        {
            Vector3 screenPos = worldCamera.WorldToScreenPoint(worldPos);
            return ScreenPosToUIPos(screenPos, trans, uiCamera);
        }

        // Screen axis to UI axis, need covert world camera to ui camera
        public static Vector2 ScreenToUIPos(Vector3 screenPos, Canvas canvas)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.worldCamera,
                out pos
            );
            return pos;
        }

        //Screen axis to UI axis
        public static Vector2 ScreenPosToUIPos(
            Vector3 screenPos,
            RectTransform rectTransform,
            Camera uiCamera
        )
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                screenPos,
                uiCamera,
                out pos
            );
            return pos;
        }

        // public static Vector2 UIToScreenPos(Vector3 uiPos, Canvas canvas)
        // {
        //     RectTransform canvasRect = canvas.transform.GetComponent<RectTransform>();
        //     //自身相对于canvas的位置
        //     Vector3 loadpos = canvas.transform.InverseTransformPoint(uiPos);
        //     // 屏幕位置
        //     Vector3 screenPoint = loadpos + new Vector3(canvasRect.sizeDelta.x, canvasRect.sizeDelta.y, 0) / 2;
        //     return screenPoint;
        // }

        public static Vector2 UIToScreenPos(Vector3 uiPos, Canvas canvas)
        {
            var canvasPixelRect = canvas.pixelRect; //canvas pixel
            float canvasWidth = canvasPixelRect.width / canvas.scaleFactor; //calculate canvas real pixel width and height
            float canvasHeight = canvasPixelRect.height / canvas.scaleFactor;
            return new Vector2(uiPos.x + canvasWidth / 2, uiPos.y + canvasHeight / 2);
        }

        public static Vector3[] ScreenConers()
        {
            return new Vector3[]{
                new Vector2(0,Screen.height),
                new Vector2(Screen.width,Screen.height),
                new Vector2(Screen.width,0),
                new Vector2(0,0),
                };
        }

        public static Vector3 WorldToScreenPoint(Vector3 worldPos, Camera worldCamera)
        {
            if (worldCamera == null)
                return Vector3.zero;
            
            return worldCamera.WorldToScreenPoint(worldPos);
        }

        public static Vector3 UIToWorldPosition(RectTransform trans, Canvas canvas)
        {
            Vector3 ptScreen = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, trans.position);
            ptScreen.z = 0;
            ptScreen.z = Mathf.Abs(canvas.worldCamera.transform.position.z - trans.position.z);
            Vector3 ptWorld = canvas.worldCamera.ScreenToWorldPoint(ptScreen);
            return ptWorld;
        }
        
        public static Vector3 LocalToGlobal(Transform transform, Canvas canvas)
        {
            // 局部转世界
            // 方法1: obj.transform.position 可以直接拿到世界坐标
            // 方法2: 需要有父对象，用父对象来操作
            return canvas.transform.TransformPoint(transform.localPosition);
        }

        public static Vector3 GlobalToLocal(Transform transfom, Canvas canvas)
        {
            //世界转局部, 也是通过父类转变换
            return canvas.transform.InverseTransformPoint(transfom.position);
        }
    }
}
