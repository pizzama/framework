
using UnityEngine;

namespace SFramework.Game.Map
{
    public static class Vector2IntExtension
    {
        public static Vector2Int Foward(this Vector2Int x, EDirFlags dir)
        {
            return x + DirFlags.ToPos(dir);
        }

        public static Vector2Int Backward(this Vector2Int x, EDirFlags dir)
        {
            return x - DirFlags.ToPos(dir);
        }
    }
}