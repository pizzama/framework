using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace SFramework.Sprites
{
    [ExecuteAlways]
    public class SpriteShadow : MonoBehaviour
    {
        private void OnEnable()
        {
            if (TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.receiveShadows = true;
                spriteRenderer.shadowCastingMode = ShadowCastingMode.TwoSided;
            }
            else if (TryGetComponent(out TilemapRenderer tilemapRenderer))
            {
                tilemapRenderer.receiveShadows = true;
                tilemapRenderer.shadowCastingMode = ShadowCastingMode.TwoSided;
            }
        }
    }
}
