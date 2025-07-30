using System;
using System.Collections.Generic;
using BlockFlipProto.Level;
using SNGames.CommonModule;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    [SerializeField] private BF_PowerupConfig powerupConfig;
    [SerializeField] private Transform powerupsContainer;

    private List<BF_BasePowerup> powerups = new List<BF_BasePowerup>();

    void Awake()
    {
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.PowerupActivated, PowerupActivated);
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.PowerUpDeactivated, PowerUpDeactivated);
    }

    private void PowerUpDeactivated(object obj)
    {
        foreach (BF_BasePowerup powerup in powerups)
        {
            powerup.SetPowerupInteractablity(true);
        }
    }

    private void PowerupActivated(object obj)
    {
        PowerupTypeInGame powerupTypeInGame = (PowerupTypeInGame)obj;

        foreach (BF_BasePowerup powerup in powerups)
        {
            powerup.SetPowerupInteractablity(false);
        }
    }

    private void FreshLevelStarted(object obj)
    {
        BF_LevelData levelData = (BF_LevelData)obj;
        SpawnPowerups(levelData.levelNumber);
    }

    private void SpawnPowerups(int level)
    {
        CleanupEarlierSpawnedPowerups();
        foreach (PowerupsInGameData powerupData in powerupConfig.powerupsInGame)
        {
            BF_BasePowerup powerup = Instantiate(powerupData.powerup, powerupsContainer);
            int amount = BF_GameSaveSystem.GetPowerupAmount(powerupData.powerupType);
            amount = amount == -1 ? powerupData.defaultAmount : amount;
            if (level >= powerupData.levelInWhichThisUnlocks)
            {
                powerup.UnlockPowerup();
                powerup.Init(true, powerupData.levelInWhichThisUnlocks, amount);
            }
            else
            {
                powerup.LockPowerup();
                powerup.Init(false, powerupData.levelInWhichThisUnlocks, amount);
            }

            BF_GameSaveSystem.SavePowerups(powerupData.powerupType, amount);

            powerups.Add(powerup);
        }
    }

    private void CleanupEarlierSpawnedPowerups()
    {
        foreach (BF_BasePowerup powerup in powerups)
        {
            Destroy(powerup.gameObject);
        }

        powerups.Clear();
        powerups = new List<BF_BasePowerup>();
    }

    void OnDestroy()
    {
        SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
    }
}
