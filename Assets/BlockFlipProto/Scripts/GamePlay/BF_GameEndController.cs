using System;
using SNGames.CommonModule;
using UnityEngine;

public class BF_GameEndController : MonoBehaviour
{
    void Awake()
    {
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.BlockSettledInHome, OnBlockSettledInHome);
    }

    private void OnBlockSettledInHome(object obj)
    {
        GameObject tileSettled = obj as GameObject;
        Debug.Log($"[BlockFlip_Gameplay][GameEnd] Block-{tileSettled.name} has settled in home tile");
    }

    void OnDestroy()
    {
        SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.BlockSettledInHome, OnBlockSettledInHome);
    }
}
