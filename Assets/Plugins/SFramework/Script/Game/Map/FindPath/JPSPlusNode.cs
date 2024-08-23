using UnityEngine;

namespace SFramework.Game.Map
{
    public class JPSPlusNode : AStarNode
    {
        private int[] mJumpDistances;

        public JPSPlusNode(in Vector2Int p, int[] jumpDistances) : base(p)
        {
            mJumpDistances = jumpDistances;
        }

        public int GetDistance(EDirFlags dir)
        {
            return mJumpDistances[DirFlags.ToArrayIndex(dir)];
        }

        internal void Refresh(int[] jumpDistances)
        {
            mJumpDistances = jumpDistances;
            Refresh();
        }
    }
}
