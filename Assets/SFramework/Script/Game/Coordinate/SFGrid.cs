using SFramework.Coordinate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFGrid
{
    public static int Radius;
    public static float CellSize;
    public List<SFVertexHex> Hexes = new List<SFVertexHex>();

    public SFGrid(int radius, float cellSize) 
    {
        SFGrid.Radius = radius;
        SFGrid.CellSize = cellSize;
        SFVertexHex.Hex(Hexes, radius);
    }
}
