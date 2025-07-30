using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BF_SaveData
{
    public BF_SaveData()
    {
        powerupsAmounts = new List<PowerupsAmount>();
    }

    public int currentLevel = 1;
    public List<PowerupsAmount> powerupsAmounts;
}

[System.Serializable]
public class PowerupsAmount
{
    public PowerupsAmount(PowerupTypeInGame powerupTypeInGame, int amount)
    {
        this.powerupTypeInGame = powerupTypeInGame;
        this.amount = amount;
    }

    public PowerupTypeInGame powerupTypeInGame;
    public int amount;
}
