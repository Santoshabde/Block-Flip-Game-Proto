using System.Collections.Generic;
using BlockFlipProto.Level;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BlockFlipProto.Gameplay
{
    public enum TileStatus
    {
        Empty,
        Occupied,
        Blocked,
        Home
    }

    public enum TileType
    {
        Base,
        Green_Final,
        Red_Final,
        Yellow_Final,
        Blue_Final,
        Orange_Final
    }

    [System.Serializable]
    public struct TileInfo
    {
        public TileType Type;
        public BF_TileData TileData;
    }

    public class BF_GridController : SerializeSingleton<BF_GridController>
    {
        [Header("Required Components")]
        [Header("Tiles In Game")]
        [SerializeField] private List<TileInfo> tilesData;

        [Space(10)]
        [Header("Tile Grid Settings")]
        [SerializeField] private GameObject blockedTile;
        [SerializeField] private Transform gridParent;

        private BF_TileData[,] grid;
        private List<((int, int), GameObject)> blockedTiles = new List<((int, int), GameObject)>();

        public BF_TileData[,] Grid => grid;

        public void ClearPrevGridTilesAndBlockedTiles()
        {
            if (grid != null)
            {
                foreach (var tile in grid)
                {
                    if (tile != null)
                    {
                        Destroy(tile.gameObject);
                    }
                }
            }

            grid = null;

            foreach (var blockedTile in blockedTiles)
            {
                if (blockedTile.Item2 != null)
                {
                    Destroy(blockedTile.Item2);
                }
            }

            blockedTiles.Clear();
        }

        public void InitializeBaseTileGrid(int lenght, int breadth)
        {
            grid = new BF_TileData[lenght, breadth];

            for (int i = 0; i < lenght; i++)
            {
                for (int j = 0; j < breadth; j++)
                {
                    if (blockedTile != null)
                    {
                        blockedTile.SetActive(false);
                    }

                    Vector3 position = new Vector3(i, -0.5f, j);
                    BF_TileData tile = Instantiate(tilesData.Find(t => t.Type == TileType.Base).TileData, position, Quaternion.identity);
                    tile.Init(i, j, TileStatus.Empty);
                    tile.transform.SetParent(gridParent);
                    tile.name = $"Tile_{i}_{j}";
                    grid[i, j] = tile;
                }
            }
        }

        public void SpawnBlockersOnBlockedTiles(List<TileIndex> blockedTilesIndices)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (blockedTilesIndices.Contains(new TileIndex(i, j)))
                    {
                        Vector3 position = new Vector3(i, 0, j);
                        GameObject instantiatedBlockedTile = Instantiate(blockedTile, position, Quaternion.identity);
                        instantiatedBlockedTile.SetActive(true);
                        grid[i, j].Init(i, j, TileStatus.Blocked);
                        instantiatedBlockedTile.transform.SetParent(gridParent);
                        instantiatedBlockedTile.name = $"BlockedTile_{i}_{j}";

                        blockedTiles.Add(((i, j), instantiatedBlockedTile));
                    }
                }
            }
        }

        public void SpawnHomeTiles(List<TileIndex> tileIndices, TileType tileType)
        {
            if( tileIndices == null || tileIndices.Count == 0)
            {
                return;
            }

            List<BF_TileData> toDeleteTiles = new List<BF_TileData>();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (tileIndices.Contains(new TileIndex(i, j)))
                    {
                        // Deactivate blocked tile if any
                        var blocked = blockedTiles.Find(b => b.Item1 == (i, j));
                        if (blocked.Item2 != null)
                        {
                            blocked.Item2.SetActive(false);
                        }

                        // Mark tile for deletion
                        toDeleteTiles.Add(grid[i, j]);

                        // Spawn new tile
                        Vector3 position = new Vector3(i, -0.5f, j);
                        BF_TileData tilePrefab = tilesData.Find(t => t.Type == tileType).TileData;
                        BF_TileData tile = Instantiate(tilePrefab, position, Quaternion.identity);
                        tile.Init(i, j, TileStatus.Home);
                        tile.transform.SetParent(gridParent);
                        tile.name = $"HomeTile_{i}_{j}_{tileType}";
                        grid[i, j] = tile;
                    }
                }
            }

            toDeleteTiles.ForEach(t => Destroy(t.gameObject));
        }

    }
}
