using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SFramework.Extension
{
    public static class UnityExtensions
    {
        //Fischer-Yates shuffle
        public static void ShuffleList<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static bool IsPrefab(this GameObject gameObject)
        {
            if (gameObject == null)
            {
                throw new NotFoundException("not found GameObject");
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


        internal static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
        {
            if (go == null)
            {
                return null;
            }

            var result = go.GetComponent<T>();
            return result != null ? result : go.AddComponent<T>();
        }

        //deep copy component
        public static T GetCopyOf<T>(this Component component, T componentToCopy) where T : Component
        {
            System.Type type = component.GetType();

            if (type != componentToCopy.GetType()) return null;

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

            PropertyInfo[] propertyInformation = type.GetProperties(flags);

            foreach (var information in propertyInformation)
            {
                if (information.CanWrite)
                {
                    information.SetValue(component, information.GetValue(componentToCopy, null), null);
                }
            }

            FieldInfo[] fieldInformation = type.GetFields(flags);

            foreach (var information in fieldInformation)
            {
                information.SetValue(component, information.GetValue(componentToCopy));
            }

            return component as T;
        }
    }
}