using System;
using UnityEngine;

public static class BF_GameSaveSystem
{
    private const string SAVE_KEY = "BF_SAVE_KEY";

    public static void SaveLevel(int currentLevel)
    {
        if (!ContainsSaveData())
        {
            var saveDataNew = new BF_SaveData();
            saveDataNew.currentLevel = currentLevel;
            PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(saveDataNew));
            PlayerPrefs.Save();
            return;
        }

        String savedData = PlayerPrefs.GetString(SAVE_KEY);
        BF_SaveData saveData = JsonUtility.FromJson<BF_SaveData>(savedData);

        if (saveData == null)
        {
            saveData = new BF_SaveData();
        }

        saveData.currentLevel = currentLevel;

        savedData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, savedData);
        PlayerPrefs.Save();
    }

    public static void SavePowerups(PowerupTypeInGame powerupType, int amount)
    {
        if (!ContainsSaveData())
        {
            var saveDataNew = new BF_SaveData();
            saveDataNew.powerupsAmounts.Add(new PowerupsAmount(powerupType, amount));
            PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(saveDataNew));
            PlayerPrefs.Save();
            return;
        }

        String savedData = PlayerPrefs.GetString(SAVE_KEY);
        BF_SaveData saveData = JsonUtility.FromJson<BF_SaveData>(savedData);

        if (saveData == null)
        {
            saveData = new BF_SaveData();
        }

        if (saveData.powerupsAmounts.Exists(x => x.powerupTypeInGame == powerupType))
        {
            saveData.powerupsAmounts.Find(x => x.powerupTypeInGame == powerupType).amount = amount;
        }
        else
        {
            saveData.powerupsAmounts.Add(new PowerupsAmount(powerupType, amount));
        }

        savedData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, savedData);
        PlayerPrefs.Save();
    }

    public static bool ContainsSaveData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }

    public static int GetLevel()
    {
        if (!ContainsSaveData()) return 1;

        String savedData = PlayerPrefs.GetString(SAVE_KEY);
        BF_SaveData saveData = JsonUtility.FromJson<BF_SaveData>(savedData);
        return saveData.currentLevel;
    }

    public static int GetPowerupAmount(PowerupTypeInGame powerupTypeInGame)
    {
        if (!ContainsSaveData()) return -1;

        String savedData = PlayerPrefs.GetString(SAVE_KEY);
        BF_SaveData saveData = JsonUtility.FromJson<BF_SaveData>(savedData);
        if(saveData.powerupsAmounts == null) return -1;
        if(saveData.powerupsAmounts.Find(x => x.powerupTypeInGame == powerupTypeInGame) == null) return -1;
        
        return saveData.powerupsAmounts.Find(x => x.powerupTypeInGame == powerupTypeInGame).amount;
    }
}
