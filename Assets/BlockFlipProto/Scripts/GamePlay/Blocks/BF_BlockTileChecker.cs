using System.Collections.Generic;
using System.Linq;
using SNGames.CommonModule;
using UnityEngine;

namespace BlockFlipProto.Gameplay
{
    public class BF_BlockTileChecker : MonoBehaviour
    {
        [SerializeField] private BF_BlockController blockController;

        [SerializeField] private List<GameObject> individualCubeLocations;
        [SerializeField] private TileType tileTypeBlockIsAssociatedWith;

        [Header("DEBUG-ONLY values")]
        [SerializeField] private List<BF_TileData> occupiedTiles;

        private BF_GridController gridController => BF_GridController.Instance;

        public List<BF_TileData> CalculateTilesWhichBlockOccupied()
        {
            if (occupiedTiles != null)
            {
                occupiedTiles.ForEach(tile => tile.SetTileStatus(TileStatus.Empty));
            }

            occupiedTiles = new List<BF_TileData>();

            foreach (GameObject locationPoints in individualCubeLocations)
            {
                RaycastHit hit;
                int layerMask = 1 << LayerMask.NameToLayer("Tile");

                if (Physics.Raycast(locationPoints.transform.position, Vector3.down, out hit, 80f, layerMask))
                {
                    BF_TileData tileData = hit.collider.GetComponent<BF_TileData>();
                    if (tileData != null)
                    {
                        if (!occupiedTiles.Contains(tileData))
                        {
                            occupiedTiles.Add(tileData);
                            tileData.SetTileStatus(TileStatus.Occupied);
                        }
                    }
                    else
                    {
                        Debug.Log("[BlockFlip_Gameplay] No BF_TileData component found on the hit object.");
                    }
                }
                else
                {
                    Debug.Log("[BlockFlip_Gameplay] Raycast did not hit any collider below the location point.");
                }
            }

            return occupiedTiles;
        }

        public BlockRotationDirection CalculatePosibleMovements(List<BF_TileData> tilesBlockIsOccupying, int forwardWidthOfBlock, int rightWidthOfBlock, int depthOfBlock)
        {
            // This method can be implemented to calculate possible movements based on occupied tiles.
            Debug.Log($"[BlockFlip_Gameplay][PossibleMovements] ---------------CalculatePosibleMovements()-------------------");

            BlockRotationDirection possibleRotationDirections = BlockRotationDirection.None;
            tilesBlockIsOccupying = tilesBlockIsOccupying
                .OrderBy(t => t.XPos)
                .ThenBy(t => t.YPos)
                .ToList();

            //--- Left Movement
            // check for depth and forwardWidth
            bool isLeftMovementPossible = true;
            BF_TileData referenceTile = tilesBlockIsOccupying[0];
            for (int dy = 0; dy < forwardWidthOfBlock; dy++)
            {
                for (int dz = 1; dz <= depthOfBlock; dz++)
                {
                    if (referenceTile.XPos - dz < 0 || referenceTile.YPos + dy >= gridController.Grid.GetLength(1))
                    {
                        Debug.Log("[BlockFlip_Gameplay][PossibleMovements] Out of bounds for Left Movement check.");
                        isLeftMovementPossible = false;
                        break;
                    }

                    BF_TileData tile = gridController.Grid[referenceTile.XPos - dz, referenceTile.YPos + dy];
                    Debug.Log($"[BlockFlip_Gameplay][PossibleMovements] Checking Left Movement at Tile: {tile.XPos}, {tile.YPos} --- TileStatus: {tile.TileStatus}");

                    if (tile.TileStatus == TileStatus.Home && tile.TileType == tileTypeBlockIsAssociatedWith)
                    {
                        isLeftMovementPossible = true;
                        break;
                    }

                    if (tile.TileStatus != TileStatus.Empty)
                    {
                        isLeftMovementPossible = false;
                        break;
                    }
                }
            }

            if (isLeftMovementPossible)
            {
                Debug.Log("[BlockFlip_Gameplay][PossibleMovements] Left Movement is possible.");
                possibleRotationDirections |= BlockRotationDirection.Left;
            }

            //--- Right Movement
            // check for depth and forwardWidth
            bool isRightMovementPossible = true;
            referenceTile = gridController.Grid[tilesBlockIsOccupying[0].XPos + rightWidthOfBlock - 1, tilesBlockIsOccupying[0].YPos];
            Debug.Log($"[BlockFlip_Gameplay][PossibleMovements] Reference Tile for Right Movement: {referenceTile.XPos}, {referenceTile.YPos}");
            for (int dy = 0; dy < forwardWidthOfBlock; dy++)
            {
                for (int dz = 1; dz <= depthOfBlock; dz++)
                {
                    if (referenceTile.XPos + dz >= gridController.Grid.GetLength(0) || referenceTile.YPos + dy >= gridController.Grid.GetLength(1))
                    {
                        Debug.Log("[BlockFlip_Gameplay][PossibleMovements] Out of bounds for Right Movement check.");
                        isRightMovementPossible = false;
                        break;
                    }

                    BF_TileData tile = gridController.Grid[referenceTile.XPos + dz, referenceTile.YPos + dy];
                    Debug.Log($"[BlockFlip_Gameplay][PossibleMovements] Checking Left Movement at Tile: {tile.XPos}, {tile.YPos} --- TileStatus: {tile.TileStatus}");

                    if (tile.TileStatus == TileStatus.Home && tile.TileType == tileTypeBlockIsAssociatedWith)
                    {
                        isRightMovementPossible = true;
                        break;
                    }

                    if (tile.TileStatus != TileStatus.Empty)
                    {
                        isRightMovementPossible = false;
                        break;
                    }
                }
            }

            if (isRightMovementPossible)
            {
                Debug.Log("[BlockFlip_Gameplay][PossibleMovements] Right Movement is possible.");
                possibleRotationDirections |= BlockRotationDirection.Right;
            }

            //--- Forward Movement
            // check for rightWidth and depth
            bool isForwardMovementPossible = true;
            referenceTile = gridController.Grid[tilesBlockIsOccupying[0].XPos, tilesBlockIsOccupying[0].YPos + forwardWidthOfBlock - 1];
            Debug.Log($"[BlockFlip_Gameplay][PossibleMovements] Reference Tile for Forward Movement: {referenceTile.XPos}, {referenceTile.YPos}");
            for (int dx = 0; dx < rightWidthOfBlock; dx++)
            {
                for (int dz = 1; dz <= depthOfBlock; dz++)
                {
                    if (referenceTile.XPos + dx >= gridController.Grid.GetLength(0) || referenceTile.YPos + dz >= gridController.Grid.GetLength(1))
                    {
                        Debug.Log("[BlockFlip_Gameplay][PossibleMovements] Out of bounds for Forward Movement check.");
                        isForwardMovementPossible = false;
                        break;
                    }

                    BF_TileData tile = gridController.Grid[referenceTile.XPos + dx, referenceTile.YPos + dz];
                    Debug.Log($"[BlockFlip_Gameplay][PossibleMovements] Checking Left Movement at Tile: {tile.XPos}, {tile.YPos} --- TileStatus: {tile.TileStatus}");

                    if (tile.TileStatus == TileStatus.Home && tile.TileType == tileTypeBlockIsAssociatedWith)
                    {
                        isForwardMovementPossible = true;
                        break;
                    }

                    if (tile.TileStatus != TileStatus.Empty)
                    {
                        isForwardMovementPossible = false;
                        break;
                    }
                }
            }

            if (isForwardMovementPossible)
            {
                Debug.Log("[BlockFlip_Gameplay][PossibleMovements] Forward Movement is possible.");
                possibleRotationDirections |= BlockRotationDirection.Forward;
            }

            //--- Backward Movement
            // check for rightWidth and depth
            bool isBackwardMovementPossible = true;
            referenceTile = tilesBlockIsOccupying[0];
            Debug.Log($"[BlockFlip_Gameplay][PossibleMovements] Reference Tile for Backward Movement: {referenceTile.XPos}, {referenceTile.YPos}");
            for (int dx = 0; dx < rightWidthOfBlock; dx++)
            {
                for (int dz = 1; dz <= depthOfBlock; dz++)
                {
                    if (referenceTile.XPos + dx >= gridController.Grid.GetLength(0) || referenceTile.YPos - dz < 0)
                    {
                        Debug.Log("[BlockFlip_Gameplay][PossibleMovements] Out of bounds for Backward Movement check.");
                        isBackwardMovementPossible = false;
                        break;
                    }

                    BF_TileData tile = gridController.Grid[referenceTile.XPos + dx, referenceTile.YPos - dz];
                    Debug.Log($"[BlockFlip_Gameplay][PossibleMovements] Checking Left Movement at Tile: {tile.XPos}, {tile.YPos} --- TileStatus: {tile.TileStatus}");

                    if (tile.TileStatus == TileStatus.Home && tile.TileType == tileTypeBlockIsAssociatedWith)
                    {
                        isBackwardMovementPossible = true;
                        break;
                    }

                    if (tile.TileStatus != TileStatus.Empty)
                    {
                        isBackwardMovementPossible = false;
                        break;
                    }
                }
            }

            if (isBackwardMovementPossible)
            {
                Debug.Log("[BlockFlip_Gameplay][PossibleMovements] Backward Movement is possible.");
                possibleRotationDirections |= BlockRotationDirection.Backward;
            }

            Debug.Log($"[BlockFlip_Gameplay][PossibleMovements] Possible Rotation Directions: {possibleRotationDirections}");
            return possibleRotationDirections;
        }

        public void CheckForBlockInHomeTile(List<BF_TileData> occupiedTiles)
        {
            if (occupiedTiles.Any(tile => tile.TileType == tileTypeBlockIsAssociatedWith))
            {
                BF_BlocksController.Instance.RemoveBlockFromBlocksController(this.gameObject);

                //Turn all tiles into open tiles
                occupiedTiles.ForEach(tile => tile.SetTileStatus(TileStatus.Empty));

                blockController.BlockVFXController.AnimateBlockReachingHome(tileTypeBlockIsAssociatedWith);

                SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.BlockSettledInHome, this.gameObject); 
            }
        }
    }
}