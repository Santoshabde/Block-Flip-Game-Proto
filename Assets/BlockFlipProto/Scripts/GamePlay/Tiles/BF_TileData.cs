using UnityEngine;
using UnityEngine.Tilemaps;

namespace BlockFlipProto.Gameplay
{
    public class BF_TileData : MonoBehaviour
    {
        [Header("Tile Data -- NEED TO SET BY DEVELOPER")]
        [SerializeField] private TileType tileType;

        [Space(10)]
        [Header("ONLY DEBUG VALUES - DONT NOT CHANGE")]
        [SerializeField] private TileStatus tileStatus;
        [SerializeField] private int xPos;
        [SerializeField] private int yPos;

        public int XPos => xPos;
        public int YPos => yPos;
        public TileStatus TileStatus => tileStatus;
        public TileType TileType => tileType;

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
}
