using System.Collections.Generic;
using SFramework;
using SFramework.Statics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace App.TurfWorld
{
    public class WorldTiler : MonoBehaviour
    {
        public static WorldTiler Instance;

        [SerializeField]
        private Material tilemapMaterial;

        private void Awake()
        {
            Instance = this;
        }

        public void TileWorld(TurfData[] turfDataArr, Grid grid, WorldGridData worldGridData, TurfData[,] turfGrid)
        {
            Dictionary<string, TilemapBrush> nameToTilemap = new Dictionary<string, TilemapBrush>();
            Dictionary<string, int> nameToSortOrder = new Dictionary<string, int>(turfDataArr.Length);

            for (int i = 0; i < turfDataArr.Length; i++)
                nameToSortOrder[turfDataArr[i].Name] = turfDataArr[i].Order;

            Debug.Log($"Tiling world...");
            int childCount = grid.transform.childCount;

            for (int i = childCount - 1; i >= 0; i--)
                DestroyImmediate(grid.transform.GetChild(i).gameObject);
            
            IList<Texture> tileTextures = AssetsManager.Instance.LoadFromBundleWithSubResources<Texture>(Game_turpworld_map_sfp.BundleName);
            foreach (var texture in tileTextures)
            {
                if (texture.name.StartsWith("_"))
                    continue;
                if (!nameToSortOrder.ContainsKey(texture.name))
                    continue;
                GameObject turfGo = new GameObject($"{texture.name}_tilemap");
                Tilemap turfMap = turfGo.AddComponent<Tilemap>();
                TilemapRenderer tilemapRend = turfGo.AddComponent<TilemapRenderer>();
                tilemapRend.sharedMaterial = Instantiate(tilemapMaterial);
                tilemapRend.sharedMaterial.SetTexture("_DetailTex", texture);
                tilemapRend.sortingLayerName = "Terrain";
                tilemapRend.sortingOrder = nameToSortOrder[texture.name];
                turfGo.transform.SetParent(grid.transform, false);
                IList<Tile> tiles =  AssetsManager.Instance.LoadFromBundleWithSubResources<Tile>($"{Game_turpworld_map_sfp.BundleName}_{texture.name}_tiles.sfp");
                TilemapBrush tilemapBrush = new TilemapBrush();
                tilemapBrush.Tilemap = turfMap;
                tilemapBrush.TurfTiles.AddRange(tiles);
                nameToTilemap[texture.name] = tilemapBrush;
            }


            for (int y = 0; y < worldGridData.GridSize.y; y++)
            {
                for (int x = 0; x < worldGridData.GridSize.x; x++)
                {
                    TurfData turfData = turfGrid[x, y];
                    TilemapBrush tilemap = nameToTilemap[turfData.Name];
                    tilemap.Tilemap.SetTile(MapToCell(x, y, new Vector2Int(worldGridData.GridSize.x, worldGridData.GridSize.y)), tilemap.GetNextTile());
                }
            }
        }

        private Vector3Int MapToCell(int x, int y, Vector2Int mapSize)
        {
            Vector2Int halfSize = mapSize / 2;
            return new Vector3Int(x - halfSize.x, y - halfSize.y);
        }

        private class TilemapBrush
        {
            public Tilemap Tilemap;
            public List<Tile> TurfTiles = new List<Tile>();

            private int index;
            public Tile GetNextTile()
            {
                int nextIndex = (index + 1) % TurfTiles.Count;
                index++;
                return TurfTiles[nextIndex];
            }
        }
    }
}
