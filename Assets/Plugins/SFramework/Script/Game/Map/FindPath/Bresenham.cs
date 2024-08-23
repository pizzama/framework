using System;
using UnityEngine;

namespace SFramework.Game.Map
{
    public class BresenHam
    {
        // ref: https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        private int mDx;
        private int mSx;
        private int mDy;
        private int mSy;
        private int mErr;
        private Vector2Int mCurr;
        private Vector2Int mDest;

        public void Init(in Vector2Int src, in Vector2Int dst)
        {
            mDx = Math.Abs(dst.x - src.x);
            mDy = -Math.Abs(dst.y - src.y);
            mSx = (src.x < dst.x) ? 1 : -1;
            mSy = (src.y < dst.y) ? 1 : -1;
            mErr = mDx + mDy;
            mCurr = src;
            mDest = dst;
        }

        public bool TryGetNext(ref Vector2Int nextP)
        {
            if (mCurr == mDest)
            {
                return false;
            }

            int e2 = 2 * mErr;

            if (e2 >= mDy)
            {
                mErr += mDy;
                mCurr.x += mSx;
            }
            if (e2 <= mDx)
            {
                mErr += mDx;
                mCurr.y += mSy;
            }

            nextP = mCurr;
            return true;
        }
    }
}
