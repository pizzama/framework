using UnityEngine;

namespace SFramework.Game.Map
{
    public class AStarNode
    {
        public Vector2Int Position { get; private set; }
        public int G { get; internal set; } = 0;
        public int H { get; internal set; } = 0;
        public long F => G + H;
        public AStarNode Parent { get; internal set; }

        public AStarNode(in Vector2Int p)
        {
            G = 0;
            H = 0;
            Position = p;
        }

        public AStarNode(int x, int y) :
            this(new Vector2Int { x = x, y = y })
        {
        }

        public void Refresh()
        {
            G = 0;
            H = 0;
            Parent = null;
        }
    }
}
