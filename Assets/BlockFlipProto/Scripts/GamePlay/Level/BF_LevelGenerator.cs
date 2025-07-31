using System;
using System.Collections;
using BlockFlipProto.Gameplay;
using BlockFlipProto.Level;
using SNGames.CommonModule;
using UnityEngine;

public class BF_LevelGenerator : MonoBehaviour
{
    [SerializeField] private bool isLevelDebugMode;
    [SerializeField] private BF_LevelConfig levelConfig;
    [SerializeField] private int currentLevel = 1;

    [SerializeField] private BF_GridController gridController;
    [SerializeField] private BF_BlocksController blocksController;


    IEnumerator Start()
    {
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.StartNextLevel, IncreaseCurrentLevel);

        yield return new WaitForSeconds(0.1f);
        FillUpCurrentLevel();
    }

    void OnDestroy()
    {
        SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.StartNextLevel, IncreaseCurrentLevel);
    }

    public void IncreaseCurrentLevel(object obj)
    {
        currentLevel++;
        if (currentLevel > levelConfig.levelDatas.Count)
        {
            Debug.LogWarning("No more levels available. Resetting to level 1.");
            currentLevel = 1;
        }

        BF_GameSaveSystem.SaveLevel(currentLevel);

        // Reload the level with the new current level
        FillUpCurrentLevel();
    }

    private void FillUpCurrentLevel()
    {
        // -- Loading currentLevel From Saved Data
        currentLevel = isLevelDebugMode? currentLevel : BF_GameSaveSystem.GetLevel();

        // -- Fetching level data from the config
        BF_LevelData currentLevelData = JsonUtility.FromJson<BF_LevelData>(levelConfig.levelDatas[currentLevel - 1].levelJson);

        // -- Setting up the level data
        gridController.ClearPrevGridTilesAndBlockedTiles();
        gridController.InitializeBaseTileGrid(currentLevelData.tileLenght, currentLevelData.tileBreadth);
        gridController.SpawnBlockersOnBlockedTiles(currentLevelData.blockedTilesIndices);

        gridController.SpawnHomeTiles(currentLevelData.yellowTilesIndices, TileType.Yellow_Final);
        gridController.SpawnHomeTiles(currentLevelData.blueTilesIndices, TileType.Blue_Final);
        gridController.SpawnHomeTiles(currentLevelData.redTilesIndices, TileType.Red_Final);
        gridController.SpawnHomeTiles(currentLevelData.greenTilesIndices, TileType.Green_Final);

        // -- Setup camera position and rotation
        Camera.main.transform.position = currentLevelData.cameraPosition;
        Camera.main.transform.rotation = Quaternion.Euler(currentLevelData.cameraRotation);
        Camera.main.fieldOfView = currentLevelData.cameraFOV;

        // -- Set Home Block Rotations
        blocksController.ClearAllBlocksInGame();

        // -- Set Home Block Movement Directions
        blocksController.SetTileMovementDirectionsInHome(currentLevelData.tileMovementDirectionsInHome);

        int numberOfTotalBlocksToClear = currentLevelData.blocksData.Count;

        // -- Spawning blocks based on the level data
        foreach (BlockData blockData in currentLevelData.blocksData)
        {
            switch (blockData.blockType)
            {
                case BlockTypes.OneByOne:
                    GameObject toInstantiateOneByOne = levelConfig.blocksInfo.Find(b => b.blockType == BlockTypes.OneByOne).block;
                    GameObject blockOneByOne = Instantiate(toInstantiateOneByOne, blockData.blockPosition, Quaternion.Euler(blockData.blockRotation));
                    blockOneByOne.GetComponent<BF_BlockVFXController>().AssignBlockMaterial(blockData.blockTileToWhichItBelongs);
                    blockOneByOne.GetComponent<BF_BlockTileChecker>().SetTileTypeBlockIsAssociatedWith(blockData.blockTileToWhichItBelongs);
                    blocksController.AddBlockToBlocksController(blockOneByOne);
                    break;
                case BlockTypes.OneByTwo:
                    GameObject toInstantiateOneByTwo = levelConfig.blocksInfo.Find(b => b.blockType == BlockTypes.OneByTwo).block;
                    GameObject blockOneByTwo = Instantiate(toInstantiateOneByTwo, blockData.blockPosition, Quaternion.Euler(blockData.blockRotation));
                    blockOneByTwo.GetComponent<BF_BlockVFXController>().AssignBlockMaterial(blockData.blockTileToWhichItBelongs);
                    blockOneByTwo.GetComponent<BF_BlockTileChecker>().SetTileTypeBlockIsAssociatedWith(blockData.blockTileToWhichItBelongs);
                    blocksController.AddBlockToBlocksController(blockOneByTwo);
                    break;
                case BlockTypes.TwoByTwo:
                    GameObject toInstantiateTwoByTwo = levelConfig.blocksInfo.Find(b => b.blockType == BlockTypes.TwoByTwo).block;
                    GameObject blockTwoByTwo = Instantiate(toInstantiateTwoByTwo, blockData.blockPosition, Quaternion.Euler(blockData.blockRotation));
                    blockTwoByTwo.GetComponent<BF_BlockVFXController>().AssignBlockMaterial(blockData.blockTileToWhichItBelongs);
                    blockTwoByTwo.GetComponent<BF_BlockTileChecker>().SetTileTypeBlockIsAssociatedWith(blockData.blockTileToWhichItBelongs);
                    blocksController.AddBlockToBlocksController(blockTwoByTwo);
                    break;

                case BlockTypes.OneByThree:
                    GameObject toInstantiateOneByThree = levelConfig.blocksInfo.Find(b => b.blockType == BlockTypes.OneByThree).block;
                    GameObject blockOneByThree = Instantiate(toInstantiateOneByThree, blockData.blockPosition, Quaternion.Euler(blockData.blockRotation));
                    blockOneByThree.GetComponent<BF_BlockVFXController>().AssignBlockMaterial(blockData.blockTileToWhichItBelongs);
                    blockOneByThree.GetComponent<BF_BlockTileChecker>().SetTileTypeBlockIsAssociatedWith(blockData.blockTileToWhichItBelongs);
                    blocksController.AddBlockToBlocksController(blockOneByThree);
                    break;
            }

            blocksController.WarmUpBlocks();
        }

        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.FreshLevelStarted, currentLevelData);
    }
}
