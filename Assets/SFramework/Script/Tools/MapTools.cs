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
            screenPos.z = 0;
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

        public static Vector2 UIToScreenPos(Vector3 uiPos, Canvas canvas)
        {
            var canvasPixelRect = canvas.pixelRect; //canvas pixel
            float canvasWidth = canvasPixelRect.width / canvas.scaleFactor; //calculate canvas real pixel width and height
            float canvasHeight = canvasPixelRect.height / canvas.scaleFactor;
            return new Vector2(uiPos.x + canvasWidth / 2, uiPos.y + canvasHeight / 2);
        }
    }
}
