using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game.Map
{
    public static class DirFlags
    {
        public static int ToArrayIndex(EDirFlags dir)
        {
            switch (dir)
            {
                case EDirFlags.NORTHWEST:
                    return 0;
                case EDirFlags.NORTH:
                    return 1;
                case EDirFlags.NORTHEAST:
                    return 2;
                case EDirFlags.WEST:
                    return 3;
                case EDirFlags.EAST:
                    return 4;
                case EDirFlags.SOUTHWEST:
                    return 5;
                case EDirFlags.SOUTH:
                    return 6;
                case EDirFlags.SOUTHEAST:
                    return 7;
                default:
                    return -1;
            }
        }

        public static bool IsStraight(EDirFlags dir)
        {
            return (dir & (EDirFlags.NORTH | EDirFlags.SOUTH | EDirFlags.EAST | EDirFlags.WEST)) != EDirFlags.NONE;
        }

        public static bool IsDiagonal(EDirFlags dir)
        {
            return (dir & (EDirFlags.NORTHEAST | EDirFlags.NORTHWEST | EDirFlags.SOUTHEAST | EDirFlags.SOUTHWEST)) != EDirFlags.NONE;
        }

        public static EDirFlags DiagonalToEastWest(EDirFlags dir)
        {
            if ((dir & (EDirFlags.NORTHEAST | EDirFlags.SOUTHEAST)) != EDirFlags.NONE)
            {
                return EDirFlags.EAST;
            }

            if ((dir & (EDirFlags.NORTHWEST | EDirFlags.SOUTHWEST)) != EDirFlags.NONE)
            {
                return EDirFlags.WEST;
            }

            return EDirFlags.NONE;
        }

        public static EDirFlags DiagonalToNorthSouth(EDirFlags dir)
        {
            if ((dir & (EDirFlags.NORTHEAST | EDirFlags.NORTHWEST)) != EDirFlags.NONE)
            {
                return EDirFlags.NORTH;
            }

            if ((dir & (EDirFlags.SOUTHEAST | EDirFlags.SOUTHWEST)) != EDirFlags.NONE)
            {
                return EDirFlags.SOUTH;
            }

            return EDirFlags.NONE;
        }

        private static readonly Dictionary<EDirFlags, Vector2Int> DirToPos = new Dictionary<EDirFlags, Vector2Int>()
        {
            { EDirFlags.NORTH,new Vector2Int(0, -1) },
            { EDirFlags.SOUTH,new Vector2Int(0, 1) },
            { EDirFlags.EAST,new Vector2Int(1, 0) },
            { EDirFlags.WEST,new Vector2Int(-1, 0) },
            { EDirFlags.NORTHEAST,new Vector2Int(1, -1) },
            { EDirFlags.NORTHWEST,new Vector2Int(-1, -1) },
            { EDirFlags.SOUTHEAST,new Vector2Int(1, 1) },
            { EDirFlags.SOUTHWEST,new Vector2Int(-1, 1) },
        };

        public static Vector2Int ToPos(EDirFlags dir)
        {
            return DirToPos[dir];
        }
    }
}
