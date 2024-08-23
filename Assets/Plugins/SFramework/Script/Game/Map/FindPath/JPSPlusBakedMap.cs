using UnityEngine;

namespace SFramework.Game.Map
{
    public class JPSPlusBakedMap
    {
        public class JPSPlusBakedMapBlock
        {
            public readonly int[] JumpDistances; // for 8 direction distance;
            public readonly Vector2Int Pos;

            public JPSPlusBakedMapBlock(in Vector2Int pos, int[] jumpDistances)
            {
                JumpDistances = jumpDistances;
                Pos = pos;
            }
        }

        public readonly int[,] BlockLUT;
        public readonly JPSPlusBakedMapBlock[] Blocks;
        public int Width => BlockLUT.GetLength(1);
        public int Height => BlockLUT.GetLength(0);

        public JPSPlusBakedMap(int[,] blockLUT, JPSPlusBakedMapBlock[] blocks)
        {
            BlockLUT = blockLUT;
            Blocks = blocks;
        }
    }
}
