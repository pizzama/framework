using SFramework.Tools.Math;
using UnityEngine;

namespace App.TurfWorld
{
    public class WorldSpawnerController : MonoBehaviour
    {
        [SerializeField]
        private NoiseSpawner[] spawners;

        public GameObject WorldObjects { get; private set; }
        public TurfData[,] TurfData { get; private set; }

        private bool[,] occupiedWorldCells;
        public void SpawnWorldObjects(WorldGridData worldGrid, TurfData[,] turfData)
        {
            Debug.Log($"Spawning world objects...");
            var existing = GameObject.Find("WorldObjects");
            if (existing != null)
                GameObject.DestroyImmediate(existing);

            TurfData = turfData;
            WorldObjects = new GameObject("WorldObjects");
            occupiedWorldCells = new bool[worldGrid.WorldSize.x, worldGrid.WorldSize.y];
            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].Spawn(worldGrid, this);
            }
        }

        public bool WorldCellIsOccupide(float worldPosX, float worldPosY, WorldGridData worldGrid)
        {
            Vector2Int worldCell = WorldPosToWorldCell(worldPosX, worldPosY, worldGrid);
            return occupiedWorldCells[worldCell.x, worldCell.y];
        }

        public void SetWorldCellOccupide(float worldPosX, float worldPosY, WorldGridData worldGrid)
        {
            Vector2Int worldCell = WorldPosToWorldCell(worldPosX, worldPosY, worldGrid);
            occupiedWorldCells[worldCell.x, worldCell.y] = true;
        }

        public Vector2Int WorldPosToWorldCell(float worldPosX, float worldPosY, WorldGridData worldGrid)
        {
            return new Vector2Int()
            {
                x = (int)MathTools.MapToRange(worldPosX, worldGrid.MinWorldPos.x, worldGrid.MaxWorldPos.x, 0, worldGrid.WorldSize.x),
                y = (int)MathTools.MapToRange(worldPosY, worldGrid.MinWorldPos.y, worldGrid.MaxWorldPos.y, 0, worldGrid.WorldSize.y)
            };
        }
    }
}


