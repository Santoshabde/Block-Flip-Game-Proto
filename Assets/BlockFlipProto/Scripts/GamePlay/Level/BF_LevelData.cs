using System.Collections.Generic;
using BlockFlipProto.Gameplay;
using Unity.VisualScripting;
using UnityEngine;

namespace BlockFlipProto.Level
{
    [System.Serializable]
    public enum BlockTypes
    {
        OneByOne = 0,
        OneByTwo = 1,
        TwoByTwo = 2,
        OneByThree = 3,
    }

    [System.Serializable]
    public class TileIndex
    {
        public int x;
        public int y;

        public TileIndex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            TileIndex other = (TileIndex)obj;
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            return x * 31 + y;
        }
    }

    [System.Serializable]
    public class BF_LevelData
    {
        // Level Settings
        public int levelNumber;
        public string levelName;
        public long levelTimeInSeconds;

        // Tile Settings
        public int tileLenght;
        public int tileBreadth;
        public List<TileIndex> blockedTilesIndices;
        public List<TileIndex> redTilesIndices;
        public List<TileIndex> greenTilesIndices;
        public List<TileIndex> blueTilesIndices;
        public List<TileIndex> yellowTilesIndices;
        public List<BlockData> blocksData;

        // Camera Settings
        public Vector3 cameraPosition;
        public Vector3 cameraRotation;
        public float cameraFOV;

        //Tile Movement Direction in Home
        public List<TileMovementDirectionInHome> tileMovementDirectionsInHome;
    }

    [System.Serializable]
    public enum MovementDirections
    {
        Left,
        Right,
        Up,
        Down
    }

    [System.Serializable]
    public class TileMovementDirectionInHome
    {
        public TileType tileIndex;
        public MovementDirections movementDirection;
    }

    [System.Serializable]
    public class BlockData
    {
        public BlockTypes blockType;
        public Vector3 blockPosition;
        public Vector3 blockRotation;
        public TileType blockTileToWhichItBelongs;
    }
}