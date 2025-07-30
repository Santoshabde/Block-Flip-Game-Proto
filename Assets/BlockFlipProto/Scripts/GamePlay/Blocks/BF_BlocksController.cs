using System;
using System.Collections.Generic;
using BlockFlipProto.Level;
using Unity.Android.Gradle;
using UnityEngine;

namespace BlockFlipProto.Gameplay
{
    public class BF_BlocksController : SerializeSingleton<BF_BlocksController>
    {
        [Header("Gameplay Parameters")]
        [Tooltip("Speed at which blocks rotate when swiped - Degress Per Second")]
        [Range(0f, 720f)]
        [SerializeField] private float blockRotationSpeed = 360f;

        [SerializeField] private List<BlockMaterialType> blockMaterials;
        private List<BF_BlockMovementController> blocksInGame;
        private List<TileMovementDirectionInHome> tileMovementDirectionsInHome;

        public List<TileMovementDirectionInHome> TileMovementDirectionsInHome => tileMovementDirectionsInHome;
        public List<BlockMaterialType> BlockMaterialType => blockMaterials;

        public void WarmUpBlocks()
        {
            AssignBlockDimentionsCalculationEvents();
            InitializeBlocks();
        }

        public void ClearBlock(BF_BlockMovementController block)
        {
            blocksInGame.Remove(block);

            block.onBlockDimentionCalculationBegin -= onBlockDimentionCalculationBegin;
            block.onBlockDimentionCalculationEnd -= onBlockDimentionCalculationEnd;

            Destroy(block.gameObject);
        }

        public void ClearAllBlocksInGame()
        {
            UnAssignBlockDimentionsCalculationEvents();
            if (blocksInGame != null)
            {
                foreach (BF_BlockMovementController block in blocksInGame)
                {
                    Destroy(block.gameObject);
                }

                blocksInGame.Clear();
            }
        }

        public void AddBlockToBlocksController(GameObject block)
        {
            if (blocksInGame == null)
            {
                blocksInGame = new List<BF_BlockMovementController>();
            }

            blocksInGame.Add(block.GetComponent<BF_BlockMovementController>());
        }

        public void RemoveBlockFromBlocksController(GameObject block)
        {
            blocksInGame.Remove(block.GetComponent<BF_BlockMovementController>());
        }

        public void SetTileMovementDirectionsInHome(List<TileMovementDirectionInHome> tileMovementDirections)
        {
            tileMovementDirectionsInHome = tileMovementDirections;
        }

        private void AssignBlockDimentionsCalculationEvents()
        {
            foreach (BF_BlockMovementController block in blocksInGame)
            {
                block.onBlockDimentionCalculationBegin += onBlockDimentionCalculationBegin;
                block.onBlockDimentionCalculationEnd += onBlockDimentionCalculationEnd;
            }
        }

        private void UnAssignBlockDimentionsCalculationEvents()
        {
            if (blocksInGame == null) return;

            foreach (BF_BlockMovementController block in blocksInGame)
            {
                block.onBlockDimentionCalculationBegin -= onBlockDimentionCalculationBegin;
                block.onBlockDimentionCalculationEnd -= onBlockDimentionCalculationEnd;
            }
        }

        private void InitializeBlocks()
        {
            blocksInGame.ForEach(block =>
            {
                block.Init(blockRotationSpeed);
            });
        }

        private void onBlockDimentionCalculationEnd(GameObject block)
        {
            foreach (BF_BlockMovementController controller in blocksInGame)
            {
                controller.GetComponent<BoxCollider>().enabled = true;
            }
        }

        private void onBlockDimentionCalculationBegin(GameObject block)
        {
            foreach (BF_BlockMovementController controller in blocksInGame)
            {
                if (controller.gameObject != block)
                    controller.GetComponent<BoxCollider>().enabled = false;
            }
        }

        void OnDestroy()
        {
            UnAssignBlockDimentionsCalculationEvents();
        }
    }

    [Serializable]
    public struct BlockMaterialType
    {
        public TileType tileType;
        public Material material;
    }
}