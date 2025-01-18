using System.Collections;
using SFramework.Extension;
#if SPINE_ANIMATION
using Spine.Unity;
#endif
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
        
        public static IEnumerator Shake(Transform baseTransform, float duration, float magnitude)
        {
            Vector3 originalPos = baseTransform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration) // TODO: Consider if game is paused
            {
                float x = originalPos.x + UnityEngine.Random.Range(-1f, 1f) * magnitude;
                float y = originalPos.y + UnityEngine.Random.Range(-1f, 1f) * magnitude;
                baseTransform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }
            baseTransform.localPosition = originalPos;
        }

        public static void LoadPrefabInRectTransForm(RectTransform parentRectTransform, GameObject prefab)
        {
            prefab.transform.SetParent(parentRectTransform);
            prefab.transform.localPosition = Vector3.zero;
            prefab.transform.localRotation = Quaternion.identity;
            prefab.transform.localScale = new Vector3(100, 100, 1); //是因为场景中的像素比例关系是1：100

            //Replace transform to rectTransform
            foreach (var tran in prefab.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.AddComponent<RectTransform>();
            }

            prefab.AddComponent<Canvas>();
            foreach (var spriteRenderer in prefab.GetComponentsInChildren<SpriteRenderer>())
            {
                var sprite = spriteRenderer.sprite;
                if (sprite == null)
                {
                    continue;
                }

                var spriteGo = spriteRenderer.gameObject;
                var img = spriteGo.AddComponent<Image>();
                img.sprite = sprite;
                img.SetNativeSize();
                img.raycastTarget = false;
                img.useSpriteMesh = true;
                var collider = spriteRenderer.GetComponent<Collider2D>();
                if (collider)
                {
                    Object.Destroy(collider);
                }

                spriteGo.transform.localScale /= 100f;
            }
#if SPINE_ANIMATION
            foreach (var skeletonAnimation in prefab.GetComponentsInChildren<SkeletonAnimation>())
            {
                var scale = skeletonAnimation.transform.localScale;
                var asset = skeletonAnimation.skeletonDataAsset;
                var oldAnimationName = skeletonAnimation.AnimationName;
                Object.Destroy(skeletonAnimation);
                Object.Destroy(skeletonAnimation.GetComponent<MeshRenderer>());
                Object.Destroy(skeletonAnimation.GetComponent<MeshFilter>());

                var graphic = skeletonAnimation.gameObject.AddComponent<SkeletonGraphic>();
                graphic.skeletonDataAsset = asset;
                graphic.raycastTarget = false;
                graphic.SetNativeSize();

                graphic.startingLoop = true;
                graphic.startingAnimation = oldAnimationName;
                var collider = graphic.gameObject.GetComponent<Collider2D>();
                if (collider)
                {
                    Object.Destroy(collider);
                }
                graphic.MatchRectTransformWithBounds();
                graphic.transform.localScale /= 100f;
            }
#endif
        }
    }
}