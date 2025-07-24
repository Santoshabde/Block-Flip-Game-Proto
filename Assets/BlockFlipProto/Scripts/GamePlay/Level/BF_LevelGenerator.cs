using System;
using BlockFlipProto.Gameplay;
using BlockFlipProto.Level;
using UnityEngine;

public class BF_LevelGenerator : MonoBehaviour
{
    [SerializeField] private BF_LevelConfig levelConfig;
    [SerializeField] private int currentLevel = 1;

    [SerializeField] private BF_GridController gridController;
    [SerializeField] private BF_BlocksController blocksController;

    void Awake()
    {
        // -- Fetching level data from the config
        BF_LevelData currentLevelData = JsonUtility.FromJson<BF_LevelData>(levelConfig.levelDatas[currentLevel - 1].levelJson);

        // -- Setting up the level data
        gridController.InitializeBaseTileGrid(currentLevelData.tileLenght, currentLevelData.tileBreadth);
        gridController.SpawnBlockersOnBlockedTiles(currentLevelData.blockedTilesIndices);
        gridController.SpawnHomeTiles(currentLevelData.yellowTilesIndices);

        // -- Setup camera position and rotation
        Camera.main.transform.position = currentLevelData.cameraPosition;
        Camera.main.transform.rotation = Quaternion.Euler(currentLevelData.cameraRotation);
        Camera.main.fieldOfView = currentLevelData.cameraFOV;

        // -- Set Home Block Movement Directions
        blocksController.SetTileMovementDirectionsInHome(currentLevelData.tileMovementDirectionsInHome);

        // -- Spawning blocks based on the level data
        foreach (BlockData blockData in currentLevelData.blocksData)
        {
            switch (blockData.blockType)
            {
                case BlockTypes.OneByOne:
                    GameObject toInstantiateOneByOne = levelConfig.blocksInfo.Find(b => b.blockType == BlockTypes.OneByOne).block;
                    GameObject blockOneByOne = Instantiate(toInstantiateOneByOne, blockData.blockPosition, blockData.blockRotation);
                    blockOneByOne.GetComponent<BF_BlockVFXController>().AssignBlockMaterial(blockData.blockTileToWhichItBelongs);
                    blocksController.AddBlockToBlocksController(blockOneByOne);
                    break;
                case BlockTypes.OneByTwo:
                    GameObject toInstantiateOneByTwo = levelConfig.blocksInfo.Find(b => b.blockType == BlockTypes.OneByTwo).block;
                    GameObject blockOneByTwo = Instantiate(toInstantiateOneByTwo, blockData.blockPosition, blockData.blockRotation);
                    blockOneByTwo.GetComponent<BF_BlockVFXController>().AssignBlockMaterial(blockData.blockTileToWhichItBelongs);
                    blocksController.AddBlockToBlocksController(blockOneByTwo);
                    break;
                case BlockTypes.TwoByTwo:
                    GameObject toInstantiateTwoByTwo = levelConfig.blocksInfo.Find(b => b.blockType == BlockTypes.TwoByTwo).block;
                    GameObject blockTwoByTwo = Instantiate(toInstantiateTwoByTwo, blockData.blockPosition, blockData.blockRotation);
                    blockTwoByTwo.GetComponent<BF_BlockVFXController>().AssignBlockMaterial(blockData.blockTileToWhichItBelongs);
                    blocksController.AddBlockToBlocksController(blockTwoByTwo);
                    break;
            }

            blocksController.WarmUpBlocks();
        }
    }
}
