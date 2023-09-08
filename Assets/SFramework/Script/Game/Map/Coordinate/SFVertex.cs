using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Coordinate
{
    public class SFVertex
    {

    }

    public class SFCoord
    {
        private int _q;
        public int q { get { return _q; } }
        private int _r;
        public int r { get { return _r; } }
        private int _s;
        public int s { get { return _s; } }

        private Vector3 _worldPosition;

        public Vector3 WorldPosition { get { return _worldPosition; } }

        public SFCoord(int q, int r, int s)
        {
            _q = q;
            _r = r;
            _s = s;
            _worldPosition = CaculateWorldPosition();
        }

        public Vector3 CaculateWorldPosition()
        {
            return new Vector3(q * Mathf.Sqrt(3) / 2, 0, -(float)r - ((float)q / 2)) * 2 * SFGrid.CellSize;
        }

        public static SFCoord[] Directions = new SFCoord[]
        {
            //∞¥’’À≥ ±’Î
            new SFCoord(0, 1, -1),
            new SFCoord(-1, 1, 0),
            new SFCoord(-1, 0, 1),
            new SFCoord(0, -1, 1),
            new SFCoord(1, -1, 0),
            new SFCoord(1, 0, -1),
        };

        public static SFCoord Direction(int direction)
        {
            return SFCoord.Directions[direction];
        }

        public SFCoord Add(SFCoord coord)
        {
            return new SFCoord(q + coord.q, r + coord.r, s + coord.s);
        }

        public SFCoord Scale(int k)
        {
            return new SFCoord(q * k, r * k, s * k);
        }

        public SFCoord Neighbor(int direction)
        {
            return Add(Direction(direction));
        }

        public static List<SFCoord> CoordRing(int radius)
        {
            List<SFCoord> result = new List<SFCoord>();
            if(radius == 0)
            {
                result.Add(new SFCoord(0, 0, 0));
            }
            else
            {
                SFCoord coord = SFCoord.Direction(4).Scale(radius);
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < radius; j++)
                    {
                        result.Add(coord);
                        coord = coord.Neighbor(i);
                    }
                }
            }

            return result;
        }

        public static List<SFCoord> CoordHex(int radius)
        {
            List<SFCoord> result = new List<SFCoord>();
            for (int i = 0; i <= radius; i++)
            {
                result.AddRange(CoordRing(i));
            }

            return result;
        }

        public override string ToString()
        {
            return q + "," + r + "," + s + ":" + _worldPosition;
        }
    }

    public class SFVertexHex: SFVertex
    {
        private SFCoord _coord;
        public  SFCoord Coord { get { return _coord; } }
        public SFVertexHex(SFCoord coord)
        {
            this._coord = coord;
        }

        public static void Hex(List<SFVertexHex> vertexs, int radius) 
        {
            foreach (SFCoord coord in SFCoord.CoordHex(radius))
            {
                vertexs.Add(new SFVertexHex(coord));
            }
        }
    }
}

