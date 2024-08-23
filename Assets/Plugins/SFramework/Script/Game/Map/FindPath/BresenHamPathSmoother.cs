using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game.Map
{
    public class BresenHamPathSmoother
    {
        private readonly BresenHam mBresenHam = new BresenHam();

        public delegate bool DelIsWalkable(in Vector2Int p);

        public List<Vector2Int> SmoothPath(List<Vector2Int> path, DelIsWalkable fnIsWalkable)
        {
            List<Vector2Int> ret = new List<Vector2Int>();
            if (path.Count < 2)
            {
                return ret;
            }

            Vector2Int bp = new Vector2Int();
            Vector2Int prevTargetP = new Vector2Int();

            Vector2Int beginP = path[0];
            Vector2Int targetP = path[1];
            int index = 1;
            mBresenHam.Init(beginP, targetP);
            ret.Add(beginP);

            while (true)
            {
                if (!mBresenHam.TryGetNext(ref bp))
                {
                    prevTargetP = targetP;
                    index++;
                    if (index == path.Count)
                    {
                        ret.Add(targetP);
                        break;
                    }
                    targetP = path[index];
                    mBresenHam.Init(beginP, targetP);
                }
                else if (!fnIsWalkable(bp))
                {
                    ret.Add(prevTargetP);
                    beginP = prevTargetP;
                    mBresenHam.Init(beginP, targetP);
                }
            }
            return ret;
        }
    }
}
