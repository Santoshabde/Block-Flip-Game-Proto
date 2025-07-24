using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerUtility
{
    public static string ConvertSecondsToTimer(int timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        return string.Format("{0}:{1:D2}", minutes, seconds);
    }
}
