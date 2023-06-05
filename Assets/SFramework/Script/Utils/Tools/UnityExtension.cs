using UnityEngine;

namespace SFramework
{
    public static class UnityExtension
    {
        // UI本地坐标转屏幕坐标
        public static Vector2 UILocal2ScreenPoint(this Transform transform, Canvas canvas, Vector2 localPos)
        {
            if (canvas.renderMode == RenderMode.WorldSpace && null != canvas.worldCamera)
            {
                return transform.Local2ScreenPoint(canvas.worldCamera, localPos);
            }
            else if (canvas.renderMode == RenderMode.ScreenSpaceCamera && null != canvas.worldCamera)
            {
                return transform.Local2ScreenPoint(canvas.worldCamera, localPos);
            }
            else
            {
                return transform.TransformPoint(localPos);
            }
        }

        public static Vector2 Local2ScreenPoint(this Transform transform, Camera camera, Vector2 localPos)
        {
            Vector2 worldPos = transform.TransformPoint(localPos);
            return camera.WorldToScreenPoint(worldPos);
        }
    }
}