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
    [SerializeField] private int lenght;
    [SerializeField] private int breadth;

    private BF_TileData[,] grid;

    public BF_TileData[,] Grid => grid;

    void Awake()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new BF_TileData[lenght, breadth];
        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < breadth; j++)
            {
                Vector3 position = new Vector3(i, -0.5f, j);
                BF_TileData tile = Instantiate(baseTile, position, Quaternion.identity);
                tile.Init(i, j, TileStatus.Empty);
                if (gridParent != null)
                    tile.transform.SetParent(gridParent);
                tile.transform.SetParent(gridParent);
                tile.name = $"Tile_{i}_{j}";
                grid[i, j] = tile;
            }
        }
    }
}
