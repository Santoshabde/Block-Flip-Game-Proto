using System;
using System.Collections;
using BlockFlipProto.Level;
using SNGames.CommonModule;
using UnityEngine;

public class BF_GameEndController : MonoBehaviour
{
    [SerializeField] private BF_LevelGenerator levelGenerator;

    private BF_LevelData cachedLevelData;

    private int totalBlocksToClear;

    void Awake()
    {
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.BlockSettledInHome, OnBlockSettledInHome);
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
    }

    private void FreshLevelStarted(object levelData)
    {
        cachedLevelData = (BF_LevelData)levelData;
        totalBlocksToClear = cachedLevelData.blocksData.Count;
    }

    private void OnBlockSettledInHome(object obj)
    {
        GameObject tileSettled = obj as GameObject;
        Debug.Log($"[BlockFlip_Gameplay][GameEnd] Block-{tileSettled.name} has settled in home tile");

        totalBlocksToClear -= 1;
        if (totalBlocksToClear <= 0)
        {
            Debug.Log("[BlockFlip_Gameplay][GameEnd] All blocks have been cleared. Triggering game end.");
            StartCoroutine(OnLevelCompleted());
        }
    }

    private IEnumerator OnLevelCompleted()
    {
        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.LevelComplete, cachedLevelData);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.OpenDialog<BF_Dlalog_LevelCompleted>();
    }

    void OnDestroy()
    {
        SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.BlockSettledInHome, OnBlockSettledInHome);
        SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
    }
}
