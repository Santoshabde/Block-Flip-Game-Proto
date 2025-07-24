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
        private List<BF_BlockMovementController> blocksController;
        private List<TileMovementDirectionInHome> tileMovementDirectionsInHome;

        public List<TileMovementDirectionInHome> TileMovementDirectionsInHome => tileMovementDirectionsInHome;
        public List<BlockMaterialType> BlockMaterialType => blockMaterials;

        public void WarmUpBlocks()
        {
            AssignBlockDimentionsCalculationEvents();
            InitializeBlocks();
        }

        public void AddBlockToBlocksController(GameObject block)
        {
            if (blocksController == null)
            {
                blocksController = new List<BF_BlockMovementController>();
            }

            blocksController.Add(block.GetComponent<BF_BlockMovementController>());
        }

        public void RemoveBlockFromBlocksController(GameObject block)
        {
            blocksController.Remove(block.GetComponent<BF_BlockMovementController>());
        }

        public void SetTileMovementDirectionsInHome(List<TileMovementDirectionInHome> tileMovementDirections)
        {
            tileMovementDirectionsInHome = tileMovementDirections;
        }

        private void AssignBlockDimentionsCalculationEvents()
        {
            foreach (BF_BlockMovementController block in blocksController)
            {
                block.onBlockDimentionCalculationBegin += onBlockDimentionCalculationBegin;
                block.onBlockDimentionCalculationEnd += onBlockDimentionCalculationEnd;
            }
        }

        private void UnAssignBlockDimentionsCalculationEvents()
        {
            foreach (BF_BlockMovementController block in blocksController)
            {
                block.onBlockDimentionCalculationBegin -= onBlockDimentionCalculationBegin;
                block.onBlockDimentionCalculationEnd -= onBlockDimentionCalculationEnd;
            }
        }

        private void InitializeBlocks()
        {
            blocksController.ForEach(block =>
            {
                block.Init(blockRotationSpeed);
            });
        }

        private void onBlockDimentionCalculationEnd(GameObject block)
        {
            foreach (BF_BlockMovementController controller in blocksController)
            {
                controller.GetComponent<BoxCollider>().enabled = true;
            }
        }

        private void onBlockDimentionCalculationBegin(GameObject block)
        {
            foreach (BF_BlockMovementController controller in blocksController)
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