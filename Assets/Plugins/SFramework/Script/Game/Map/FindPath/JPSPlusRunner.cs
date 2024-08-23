using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game.Map
{
    public class JPSPlusRunner
    {
        private readonly JPSPlus mJpsPlus = new JPSPlus();
        private readonly JPSPlusMapBaker mBaker = new JPSPlusMapBaker();
        private bool[,] mWalls = null;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Vector2Int StartP => mStartP.Value;
        public Vector2Int GoalP => mGoalP.Value;

        private bool mIsWallChanged = true;
        private Vector2Int? mStartP = null;
        private Vector2Int? mGoalP = null;

        public JPSPlusRunner(int width, int height)
        {
            Init(new bool[height, width]);
        }

        public JPSPlusRunner(bool[,] walls)
        {
            Init(walls);
        }

        public void Init(bool[,] walls)
        {
            mWalls = walls;
            Width = walls.GetLength(1);
            Height = walls.GetLength(0);
        }

        public void ToggleWall(in Vector2Int p)
        {
            if (!IsInBoundary(p))
            {
                return;
            }
            mWalls[p.y, p.x] = !mWalls[p.y, p.x];
            mIsWallChanged = true;
        }

        public void SetStart(in Vector2Int p)
        {
            if (!IsInBoundary(p))
            {
                return;
            }
            mStartP = p;
        }

        public void SetGoal(in Vector2Int p)
        {
            if (!IsInBoundary(p))
            {
                return;
            }
            mGoalP = p;
        }

        public bool IsWalkable(in Vector2Int p)
        {
            if (!IsInBoundary(p))
            {
                return false;
            }
            return !mWalls[p.y, p.x];
        }

        public bool StepAll(int stepCount = int.MaxValue)
        {
            if (mIsWallChanged)
            {
                mBaker.Init(mWalls);
                mJpsPlus.Init(mBaker.Bake());
                mIsWallChanged = false;
            }
            _ = mJpsPlus.SetStart(mStartP.Value);
            _ = mJpsPlus.SetGoal(mGoalP.Value);
            return mJpsPlus.StepAll(stepCount);
        }

        public void SetWall(in Vector2Int p, bool isWall)
        {
            if (!IsInBoundary(p))
            {
                return;
            }
            mWalls[p.y, p.x] = isWall;
            mIsWallChanged = true;
        }

        public bool[,] GetWalls()
        {
            return mWalls;
        }

        public IReadOnlyList<AStarNode> GetPaths()
        {
            return mJpsPlus.GetPaths();
        }

        private bool IsInBoundary(in Vector2Int p)
        {
            return 0 <= p.x && p.x < Width && 0 <= p.y && p.y < Height;
        }

    }
}
