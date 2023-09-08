using SFramework.Coordinate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFGridGenerate : MonoBehaviour
{
    [SerializeField]
    private int _radius;
    [SerializeField]
    private float _cellSize;
    private SFGrid _grid;
    private void Awake()
    {
        _grid = new SFGrid(_radius, _cellSize);
    }

    private void OnDrawGizmos()
    {
        if (_grid != null)
        {
            Debug.Log(_grid.Hexes.Count);
            foreach (SFVertexHex vertex in _grid.Hexes)
            {
                Debug.Log(vertex.Coord.ToString());
                Gizmos.DrawSphere(vertex.Coord.CaculateWorldPosition(SFGrid.CellSize), 0.3f);    
            }
        }
    }
}
