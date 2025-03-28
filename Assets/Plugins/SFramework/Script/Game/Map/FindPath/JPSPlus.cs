using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SFramework.Collections;
using UnityEngine;

namespace SFramework.Game.Map
{
    public class JPSPlus
    {
        // =======================
        // Members
        // =======================
        private JPSPlusNode mStart = null;
        private JPSPlusNode mGoal = null;
        private readonly Dictionary<Vector2Int, JPSPlusNode> mCreatedNodes = new Dictionary<Vector2Int, JPSPlusNode>();
        private readonly PriorityQueue<(JPSPlusNode Node, EDirFlags Dir)> mOpenList = new PriorityQueue<(JPSPlusNode Node, EDirFlags Dir)>();
        private readonly HashSet<JPSPlusNode> mCloseList = new HashSet<JPSPlusNode>();
        private JPSPlusBakedMap mBakedMap;

        public JPSPlus()
        {

        }

        public void Init(JPSPlusBakedMap bakedMap)
        {
            mBakedMap = bakedMap;
        }

        public bool StepAll(int stepCount = int.MaxValue)
        {
            mOpenList.Clear();
            mCloseList.Clear();

            foreach (JPSPlusNode node in mCreatedNodes.Values.ToList())
            {
                int index = mBakedMap.BlockLUT[node.Position.y, node.Position.x];
                if (index < 0)
                {
                    _ = mCreatedNodes.Remove(node.Position);
                }
                else
                {
                    node.Refresh(mBakedMap.Blocks[index].JumpDistances);
                }
            }
            mOpenList.Enqueue((mStart, EDirFlags.ALL), mStart.F);
            return Step(stepCount);
        }

        public bool Step(int stepCount)
        {
            int step = stepCount;
            while (true)
            {
                if (step <= 0)
                {
                    return false;
                }
                if (mOpenList.Count == 0)
                {
                    return false;
                }

                (JPSPlusNode Node, EDirFlags fromDir) curr = mOpenList.Dequeue();
                JPSPlusNode currNode = curr.Node;
                EDirFlags fromDir = curr.fromDir;
                _ = mCloseList.Add(currNode);

                Vector2Int currPos = currNode.Position;
                Vector2Int goalPos = mGoal.Position;

                if (currPos == goalPos)
                {
                    return true;
                }

                EDirFlags validDirs = ValidLookUPTable(fromDir);
                for (int i = 0b10000000; i > 0; i >>= 1)
                {
                    EDirFlags processDir = (EDirFlags)i;
                    if ((processDir & validDirs) == EDirFlags.NONE)
                    {
                        continue;
                    }

                    bool isDiagonalDir = DirFlags.IsDiagonal(processDir);
                    int dirDistance = currNode.GetDistance(processDir);
                    int lengthX = RowDiff(currNode, mGoal);
                    int lengthY = ColDiff(currNode, mGoal);


                    JPSPlusNode nextNode;
                    int nextG;
                    if (!isDiagonalDir
                        && IsGoalInExactDirection(currPos, processDir, goalPos)
                        && Math.Max(lengthX, lengthY) <= Math.Abs(dirDistance))
                    {
                        // 직선이동중
                        // 골과 같은 방향
                        // 골 노드거리 방향거리보다 같거나 작으면 그게 바로 골
                        nextNode = mGoal;
                        nextG = currNode.G + (Math.Max(lengthX, lengthY) * 10);
                    }
                    else if (isDiagonalDir
                        && IsGoalInGeneralDirection(currPos, processDir, goalPos)
                        && (lengthX <= Math.Abs(dirDistance) || lengthY <= Math.Abs(dirDistance))
                        )
                    {
                        // Target Jump Point
                        // 대각 이동중
                        // 골과 일반적 방향
                        int minDiff = Math.Min(lengthX, lengthY);
                        nextNode = GetNode(currNode, minDiff, processDir);
                        nextG = currNode.G + (Math.Max(lengthX, lengthY) * 14); // 대각길이 비용.
                    }
                    else if (dirDistance > 0)
                    {
                        // 점프가 가능하면 점프!
                        nextNode = GetNode(currNode, processDir);
                        if (isDiagonalDir)
                        {
                            nextG = currNode.G + (Math.Max(lengthX, lengthY) * 14);
                        }
                        else
                        {
                            nextG = currNode.G + (Math.Max(lengthX, lengthY) * 10);
                        }
                    }
                    else
                    {
                        // 찾지못하면 다음 것으로.
                        continue;
                    }

                    (JPSPlusNode, EDirFlags) openJump = (nextNode, processDir);

                    if (!mOpenList.Contains(openJump) && !mCloseList.Contains(nextNode))
                    {
                        nextNode.Parent = currNode;
                        nextNode.G = nextG;
                        nextNode.H = H(nextNode, mGoal);
                        mOpenList.Enqueue(openJump, nextNode.F);
                    }
                    else if (nextG < nextNode.G)
                    {
                        nextNode.Parent = currNode;
                        nextNode.G = nextG;
                        nextNode.H = H(nextNode, mGoal);
                        mOpenList.UpdatePriority(openJump, nextNode.F);
                    }
                }
                step--;
            }
        }

        public bool IsWalkable(in Vector2Int p)
        {
            if (mBakedMap == null)
            {
                return false;
            }

            if (!IsInBoundary(p))
            {
                return false;
            }

            return mBakedMap.BlockLUT[p.y, p.x] >= 0;
        }

        public bool SetStart(in Vector2Int p)
        {
            if (mBakedMap == null)
            {
                return false;
            }

            if (!IsInBoundary(p))
            {
                return false;
            }

            mStart = GetOrCreatedNode(p);
            return true;
        }

        public bool SetGoal(in Vector2Int p)
        {
            if (mBakedMap == null)
            {
                return false;
            }

            if (!IsInBoundary(p))
            {
                return false;
            }

            mGoal = GetOrCreatedNode(p);
            return true;
        }

        public JPSPlusNode GetJPSPlusNode(in Vector2Int p)
        {
            return new JPSPlusNode(p, mBakedMap.Blocks[mBakedMap.BlockLUT[p.y, p.x]].JumpDistances);
        }

        public IReadOnlyList<AStarNode> GetPaths()
        {
            List<AStarNode> ret = new List<AStarNode>();
            AStarNode n = mGoal;
            while (n != null)
            {
                ret.Add(n);
                n = n.Parent;
            }
            ret.Reverse();
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JPSPlusNode GetStart()
        {
            return mStart;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JPSPlusNode GetGoal()
        {
            return mGoal;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PriorityQueue<(JPSPlusNode, EDirFlags)> GetOpenList()
        {
            return mOpenList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashSet<JPSPlusNode> GetCloseList()
        {
            return mCloseList;
        }

        public int Width => mBakedMap.Width;
        public int Height => mBakedMap.Height;

        // =======================
        // Private Methods
        // =======================
        private JPSPlusNode GetOrCreatedNode(in Vector2Int p)
        {
            if (mCreatedNodes.TryGetValue(p, out JPSPlusNode createdNode))
            {
                return createdNode;
            }
            JPSPlusNode newNode = GetJPSPlusNode(p);
            mCreatedNodes.Add(p, newNode);
            return newNode;
        }

        private bool IsInBoundary(in Vector2Int p)
        {
            return 0 <= p.x && p.x < Width && 0 <= p.y && p.y < Height;
        }

        #region plus
        private EDirFlags ValidLookUPTable(EDirFlags dir)
        {
            switch (dir)
            {
                // . N .
                // W . E
                // . S .
                case EDirFlags.NORTH:
                    return EDirFlags.EAST | EDirFlags.NORTHEAST | EDirFlags.NORTH | EDirFlags.NORTHWEST | EDirFlags.WEST;
                case EDirFlags.WEST:
                    return EDirFlags.NORTH | EDirFlags.NORTHWEST | EDirFlags.WEST | EDirFlags.SOUTHWEST | EDirFlags.SOUTH;
                case EDirFlags.EAST:
                    return EDirFlags.SOUTH | EDirFlags.SOUTHEAST | EDirFlags.EAST | EDirFlags.NORTHEAST | EDirFlags.NORTH;
                case EDirFlags.SOUTH:
                    return EDirFlags.WEST | EDirFlags.SOUTHWEST | EDirFlags.SOUTH | EDirFlags.SOUTHEAST | EDirFlags.EAST;
                case EDirFlags.NORTHWEST:
                    return EDirFlags.NORTH | EDirFlags.NORTHWEST | EDirFlags.WEST;
                case EDirFlags.NORTHEAST:
                    return EDirFlags.NORTH | EDirFlags.NORTHEAST | EDirFlags.EAST;
                case EDirFlags.SOUTHWEST:
                    return EDirFlags.SOUTH | EDirFlags.SOUTHWEST | EDirFlags.WEST;
                case EDirFlags.SOUTHEAST:
                    return EDirFlags.SOUTH | EDirFlags.SOUTHEAST | EDirFlags.EAST;
                default:
                    return dir;
            }
        }

        private bool IsGoalInExactDirection(in Vector2Int curr, EDirFlags processDir, in Vector2Int goal)
        {
            int dx = goal.x - curr.x;
            int dy = goal.y - curr.y;

            switch (processDir)
            {
                case EDirFlags.NORTH:
                    return dx == 0 && dy < 0;
                case EDirFlags.SOUTH:
                    return dx == 0 && dy > 0;
                case EDirFlags.WEST:
                    return dx < 0 && dy == 0;
                case EDirFlags.EAST:
                    return dx > 0 && dy == 0;
                case EDirFlags.NORTHWEST:
                    return dx < 0 && dy < 0 && (Math.Abs(dx) == Math.Abs(dy));
                case EDirFlags.NORTHEAST:
                    return dx > 0 && dy < 0 && (Math.Abs(dx) == Math.Abs(dy));
                case EDirFlags.SOUTHWEST:
                    return dx < 0 && dy > 0 && (Math.Abs(dx) == Math.Abs(dy));
                case EDirFlags.SOUTHEAST:
                    return dx > 0 && dy > 0 && (Math.Abs(dx) == Math.Abs(dy));
                default:
                    return false;
            }
        }

        private bool IsGoalInGeneralDirection(in Vector2Int curr, EDirFlags processDir, in Vector2Int goal)
        {
            int dx = goal.x - curr.x;
            int dy = goal.y - curr.y;

            switch (processDir)
            {
                case EDirFlags.NORTH:
                    return dx == 0 && dy < 0;
                case EDirFlags.SOUTH:
                    return dx == 0 && dy > 0;
                case EDirFlags.WEST:
                    return dx < 0 && dy == 0;
                case EDirFlags.EAST:
                    return dx > 0 && dy == 0;
                case EDirFlags.NORTHWEST:
                    return dx < 0 && dy < 0;
                case EDirFlags.NORTHEAST:
                    return dx > 0 && dy < 0;
                case EDirFlags.SOUTHWEST:
                    return dx < 0 && dy > 0;
                case EDirFlags.SOUTHEAST:
                    return dx > 0 && dy > 0;
                default:
                    return false;
            }
        }

        private JPSPlusNode GetNode(JPSPlusNode node, EDirFlags dir)
        {
            return GetOrCreatedNode(node.Position + (DirFlags.ToPos(dir) * node.GetDistance(dir)));
        }

        private JPSPlusNode GetNode(JPSPlusNode node, int dist, EDirFlags dir)
        {
            return GetOrCreatedNode(node.Position + (DirFlags.ToPos(dir) * dist));
        }

        private int ColDiff(JPSPlusNode currNode, JPSPlusNode goalNode)
        {
            return Math.Abs(goalNode.Position.x - currNode.Position.x);
        }

        private int RowDiff(JPSPlusNode currNode, JPSPlusNode goalNode)
        {
            return Math.Abs(goalNode.Position.y - currNode.Position.y);
        }
        #endregion plus

        // =========================================
        // Statics
        // =========================================
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int H(JPSPlusNode n, JPSPlusNode goal)
        {
            // calculate estimated cost
            return (Math.Abs(goal.Position.x - n.Position.x) + Math.Abs(goal.Position.y - n.Position.y)) * 10;
        }
    }
}
