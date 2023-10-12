using UnityEngine;

namespace SFramework.Extension
{
    public static class UnityExtension
    {
        public static bool IsPrefab(this GameObject gameObject)
        {
            if (gameObject == null)
            {
                throw new NotFoundException("not found Gameobject");
            }

            return
                !gameObject.scene.IsValid() &&
                !gameObject.scene.isLoaded &&
                gameObject.GetInstanceID() >= 0 &&
                // I noticed that ones with IDs under 0 were objects I didn't recognize
                !gameObject.hideFlags.HasFlag(HideFlags.HideInHierarchy);
                // I don't care about GameObjects *inside* prefabs, just the overall prefab.
        }
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

        public static string RemoveInvalidateChars(this string name)
        {
            return name.Replace("/", "_")
                .Replace("@", "")
                .Replace("!", "")
                .Replace(" ", "_")
                .Replace("__", "_")
                .Replace("__", "_")
                .Replace("__", "_")
                .Replace("&", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("#", "")
                .Replace(".", "_");
        }

        internal static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
        {
            if (go == null)
            {
                return null;
            }

            var result = go.GetComponent<T>();
            return result != null ? result : go.AddComponent<T>();
        }
    }
}