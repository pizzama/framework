using UnityEngine;
using System.Collections.Generic;
using SFramework.Collections;

namespace SFramework.Game.Map
{
    public class AStar
    {
        // ============================
        // Private
        // ============================
        private AStarNode mStart = null;
        private AStarNode mGoal = null;
        private readonly AStarNode[,] mNodes;
        private readonly PriorityQueue<AStarNode> mOpenList = new PriorityQueue<AStarNode>();
        private readonly HashSet<AStarNode> mCloseList = new HashSet<AStarNode>();
        private readonly bool[,] mWalls;

        // ============================
        // Properties
        // ============================
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Vector2 StartP => mStart.Position;
        public Vector2 GoalP => mGoal.Position;

        public AStar(int width, int height)
        {
            Width = width;
            Height = height;
            mNodes = new AStarNode[Height, Width];
            mWalls = new bool[Height, Width];
        }

        public AStar(bool[,] walls)
        {
            Height = walls.GetLength(0);
            Width = walls.GetLength(1);
            mNodes = new AStarNode[Height, Width];
            mWalls = walls;
        }

        // ============================
        // Public Methods
        // ============================

        public bool StepAll()
        {
            mOpenList.Clear();
            mCloseList.Clear();
            mOpenList.Enqueue(mStart, mStart.F);
            mGoal.Parent = null;
            return Step(int.MaxValue);
        }

        public bool Step(int stepCount)
        {
            for (int step = stepCount; step > 0; --step)
            {
                if (mOpenList.Count == 0)
                {
                    return false;
                }

                AStarNode curr = mOpenList.Dequeue();
                if (curr == mGoal)
                {
                    return true;
                }

                _ = mCloseList.Add(curr);

                for (int i = 0b10000000; i > 0; i >>= 1)
                {

                    EDirFlags dir = (EDirFlags)i;
                    Vector2Int dp = DirFlags.ToPos(dir);
                    AStarNode adjacent = GetNodeOrNull(curr.Position + dp);
                    if (adjacent == null)
                    {
                        continue;
                    }
                    if (IsWall(adjacent.Position))
                    {
                        continue;
                    }

                    if (DirFlags.IsDiagonal(dir))
                    { // for prevent corner cutting
                        if (IsWall(curr.Position + new Vector2Int(dp.x, 0)) || IsWall(curr.Position + new Vector2Int(0, dp.y)))
                        {
                            continue;
                        }
                    }

                    if (mCloseList.Contains(adjacent))
                    {
                        continue;
                    }

                    int nextG = G(curr, adjacent);
                    if (!mOpenList.Contains(adjacent))
                    {
                        adjacent.Parent = curr;
                        adjacent.G = nextG;
                        adjacent.H = H(adjacent, mGoal);
                        mOpenList.Enqueue(adjacent, adjacent.F);
                    }
                    else if (nextG < adjacent.G)
                    {
                        adjacent.Parent = curr;
                        adjacent.G = nextG;
                        adjacent.H = H(adjacent, mGoal);
                        mOpenList.UpdatePriority(adjacent, adjacent.F);
                    }
                }
            }
            return false;
        }

        public bool SetStart(in Vector2Int pos)
        {
            if (!IsInBoundary(pos))
            {
                return false;
            }

            mStart = GetNodeOrNull(pos);
            return true;
        }

        public bool SetGoal(in Vector2Int pos)
        {
            if (!IsInBoundary(pos))
            {
                return false;
            }

            mGoal = GetNodeOrNull(pos);
            return true;
        }

        public void SetWall(in Vector2Int p, bool isWall)
        {
            if (!IsInBoundary(p))
            {
                return;
            }
            mWalls[p.y, p.x] = isWall;
        }

        public bool ToggleWall(in Vector2Int pos)
        {
            if (!IsInBoundary(pos))
            {
                return false;
            }
            mWalls[pos.y, pos.x] = !mWalls[pos.y, +pos.x];
            return true;
        }

        public List<AStarNode> GetPaths()
        {
            List<AStarNode> ret = new List<AStarNode>();
            AStarNode node = mGoal;
            while (node != null)
            {
                ret.Add(node);
                node = node.Parent;
            }
            ret.Reverse();
            return ret;
        }

        public AStarNode GetStart()
        {
            return mStart;
        }

        public AStarNode GetGoal()
        {
            return mGoal;

        }

        public SimplePriorityQueue<AStarNode, long> GetOpenList()
        {
            return mOpenList;
        }

        public HashSet<AStarNode> GetCloseList()
        {
            return mCloseList;
        }

        public bool[,] GetWalls()
        {
            return mWalls;
        }

        public bool IsWalkable(in Vector2Int p)
        {
            if (!IsInBoundary(p))
            {
                return false;
            }
            return !IsWall(p);
        }

        // ============================
        // Private Methods
        // ============================
        private bool IsInBoundary(in Vector2Int pos)
        {
            return IsInBoundary(pos.x, pos.y);
        }

        private bool IsInBoundary(int x, int y)
        {
            return 0 <= x && x < Width && 0 <= y && y < Height;
        }

        private AStarNode GetNodeOrNull(in Vector2Int pos)
        {
            int x = pos.x;
            int y = pos.y;
            if (!IsInBoundary(x, y))
            {
                return null;
            }
            AStarNode node = mNodes[y, x];
            if (node != null)
            {
                return node;
            }
            node = new AStarNode(x, y);
            mNodes[y, x] = node;
            return node;
        }

        private bool IsWall(in Vector2Int pos)
        {
            return mWalls[pos.y, pos.x];
        }

        private static int G(AStarNode from, AStarNode adjacent)
        {
            // cost so far to reach n 
            Vector2Int p = from.Position - adjacent.Position;
            if (p.x == 0 || p.y == 0)
            {
                return from.G + 10;
            }
            else
            {
                return from.G + 14;
            }
        }

        internal static int H(AStarNode n, AStarNode goal)
        {
            // calculate estimated cost
            return Mathf.Abs(goal.Position.x - n.Position.x) + (Mathf.Abs(goal.Position.y - n.Position.y) * 10);
        }
    }
}
