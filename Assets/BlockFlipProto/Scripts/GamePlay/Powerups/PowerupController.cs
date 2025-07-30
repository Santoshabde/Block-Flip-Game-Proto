using System;
using BlockFlipProto.Level;
using SNGames.CommonModule;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    [SerializeField] private BF_PowerupConfig powerupConfig;
    [SerializeField] private Transform powerupsContainer;

    void Awake()
    {
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
    }

    private void FreshLevelStarted(object obj)
    {
        BF_LevelData levelData = (BF_LevelData)obj;
        SpawnPowerups(levelData.levelNumber);
    }

    private void SpawnPowerups(int level)
    {
        foreach (var powerupData in powerupConfig.powerupsInGame)
        {
            BF_BasePowerup powerup = Instantiate(powerupData.powerup, powerupsContainer);

            if (level >= powerupData.levelInWhichThisUnlocks)
            {
                powerup.UnlockPowerup();
                powerup.Init(true, powerupData.levelInWhichThisUnlocks);
            }
            else
            {
                powerup.LockPowerup();
                powerup.Init(false, powerupData.levelInWhichThisUnlocks);
            }
        }
    }

    void OnDestroy()
    {
        SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
    }
}
