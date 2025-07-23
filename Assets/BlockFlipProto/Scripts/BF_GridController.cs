using UnityEngine;

public enum TileStatus
{
    Empty,
    Occupied,
    Blocked
}

public class BF_GridController : MonoBehaviour
{
    [SerializeField] private BF_TileData baseTile;
    [SerializeField] private Transform gridParent;
    [SerializeField] private Transform blockerParent;
    [SerializeField] private GameObject blockedPrefab;
    [SerializeField] private int lenght;
    [SerializeField] private int breadth;

    private BF_TileData[,] grid;
    private GameObject[,] blockedTiles;

    public BF_TileData[,] Grid => grid;

    // These offsets allow negative coordinates in blockedTiles array
    private int offsetX = 200;
    private int offsetZ = 200;

    void Awake()
    {
        InitializeBlockerCubesGrid();
        InitializeTileGrid();
    }

    private void InitializeBlockerCubesGrid()
    {
        blockedTiles = new GameObject[1000, 1000];

        // Create 20x20 blocked tiles centered at (0,0)
        for (int i = -14; i < 14; i++)
        {
            for (int j = -25; j < 25; j++)
            {
                GameObject blockedTile = Instantiate(blockedPrefab, new Vector3(i, 0, j), Quaternion.identity);
                if (blockerParent != null)
                    blockedTile.transform.SetParent(blockerParent);
                blockedTile.name = $"BlockedTile_{i}_{j}";
                blockedTiles[i + offsetX, j + offsetZ] = blockedTile;
            }
        }
    }

    private void InitializeTileGrid()
    {
        grid = new BF_TileData[lenght, breadth];

        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < breadth; j++)
            {
                // Safely deactivate blocked tile if it exists
                GameObject blockedTile = blockedTiles[i + offsetX, j + offsetZ];
                if (blockedTile != null)
                {
                    blockedTile.SetActive(false);
                }

                Vector3 position = new Vector3(i, -0.5f, j);
                BF_TileData tile = Instantiate(baseTile, position, Quaternion.identity);
                tile.Init(i, j, TileStatus.Empty);
                tile.transform.SetParent(gridParent);
                tile.name = $"Tile_{i}_{j}";
                grid[i, j] = tile;
            }
        }
    }
}
