using UnityEngine;

namespace SFramework.Game.Map
{
    public class JPSPlusMapBakerBlock
    {
        public readonly int[] JumpDistances = new int[8];
        public readonly Vector2Int Pos;
        public EDirFlags JumpDirFlags = EDirFlags.NONE;

        public JPSPlusMapBakerBlock(in Vector2Int pos)
        {
            Pos = pos;
        }

        public bool IsJumpable(EDirFlags dir)
        {
            return (JumpDirFlags & dir) == dir;
        }

        public void SetDistance(EDirFlags dir, int distance)
        {
            JumpDistances[DirFlags.ToArrayIndex(dir)] = distance;
        }

        public int GetDistance(EDirFlags dir)
        {
            return JumpDistances[DirFlags.ToArrayIndex(dir)];
        }
    }
}
