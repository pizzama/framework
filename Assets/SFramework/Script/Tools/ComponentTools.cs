using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SFramework.Tools
{
    public class ComponentTools
    {

        //获取组件 没有自动添加
        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            if (go == null)
            {
                throw new NotFoundException("Gameobject go is null");
            }
            T target = go.GetComponent<T>();
            if (target == null)
                target = go.AddComponent<T>();
            return target;
        }

        public static T GetOrAddComponent<T>(Transform go) where T : Component
        {
            return GetOrAddComponent<T>(go.gameObject);
        }

        // Pre Calculate text width and height
        public static Vector2 GetTextPreferredWidthAndHeight(Text txt, string content)
        {
            var tg = new TextGenerator();
            var settings = txt.GetGenerationSettings(txt.GetComponent<RectTransform>().sizeDelta);
            float width = tg.GetPreferredWidth(content, settings);
            float height = tg.GetPreferredHeight(content, settings);

            return new UnityEngine.Vector2(width, height);
        }
    }
}