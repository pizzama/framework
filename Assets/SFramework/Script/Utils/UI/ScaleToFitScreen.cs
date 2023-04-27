using UnityEngine;
namespace PUtils
{
    [ExecuteInEditMode]
    public class ScaleToFitScreen : MonoBehaviour
    {
        private SpriteRenderer sr;

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();

            // 世界的高永远是相机正交大小的2倍
            float worldScreenHeight = Camera.main.orthographicSize * 2;

            // 世界的宽=世界的高/屏幕的高*屏幕的宽
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            transform.localScale = new Vector3(
                worldScreenWidth / sr.sprite.bounds.size.x,
                worldScreenHeight / sr.sprite.bounds.size.y, 1);
        }

    }
}
