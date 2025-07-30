using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerupData", menuName = "Powerups/PowerupData", order = 1)]
public class BF_PowerupConfig : ScriptableObject
{
    public List<PowerupsInGameData> powerupsInGame;
}

[System.Serializable]
public enum PowerupsInGame
{
    FreezeTimerPowerup,
    BlockDestroyer,
    TestPower
}

[System.Serializable]
public class PowerupsInGameData
{
    public PowerupsInGame powerupType;
    public int levelInWhichThisUnlocks;
    public BF_BasePowerup powerup;
}
