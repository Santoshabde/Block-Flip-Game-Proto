using UnityEngine;
using UnityEngine.Tilemaps;

public class BF_TileData : MonoBehaviour
{
    [SerializeField] private TileStatus tileStatus;
    [SerializeField] private int xPos;
    [SerializeField] private int yPos;

    public int XPos => xPos;
    public int YPos => yPos;
    public TileStatus TileStatus => tileStatus;

    public void Init(int x, int y, TileStatus status)
    {
        xPos = x;
        yPos = y;
        tileStatus = status;
    }

    public void SetTileStatus(TileStatus status)
    {
        tileStatus = status;
    }
}
