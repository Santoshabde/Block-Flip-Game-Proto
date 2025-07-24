using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;

public class LocalSaveSystem
{
  /*  public static PlayerInGameStats playerInGameStats 
    {
        get { return GetValue<PlayerInGameStats>("PlayerInGameStats", new PlayerInGameStats()); }
        set { SetValue("PlayerInGameStats", value); }
    } 

    #region GET AND SET VALUE FUCTIONS
    private static T GetValue<T>(string key, T defaultValue)
    {
        return PlayerPrefs.HasKey(key) ? JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(key)) : defaultValue;
    }

    private static void SetValue(string key, object value)
    {
        string data = JsonConvert.SerializeObject(value, Formatting.None);
        PlayerPrefs.SetString(key, data);
    }
    #endregion */
}

//#region All Save Game Classes

//[System.Serializable]
//public class PlayerInGameStats
//{
//    public PlayerInGameStats()
//    {
//        //Set all default values here
//        currentLevel = 1;
//    }

//    public int currentLevel;
//}
//#endregion