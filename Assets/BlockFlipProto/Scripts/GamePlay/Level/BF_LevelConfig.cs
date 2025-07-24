using System.Collections.Generic;
using BlockFlipProto.Level;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData", order = 1)]
public class BF_LevelConfig : ScriptableObject
{
    [Header("Level Configuration")]
    public List<LevelInfo> levelDatas;

    [Space(10)]
    [Header("Blocks Configuration")]
    public List<BlocksInfo> blocksInfo;
}

[System.Serializable]
public struct LevelInfo
{
    public int levelNumber;
    public string levelJson;
}

[System.Serializable]
public struct BlocksInfo
{
    public BlockTypes blockType;
    public GameObject block;
}