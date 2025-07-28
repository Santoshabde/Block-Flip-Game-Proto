using System;
using System.Collections;
using BlockFlipProto.Level;
using SNGames.CommonModule;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private Text levelText;

    void Start()
    {
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
    }

    private void FreshLevelStarted(object obj)
    {
        BF_LevelData levelData = (BF_LevelData)obj;
        UpdateLevel(levelData.levelNumber);
        StartTimer(levelData.levelTimeInSeconds);
    }

    void OnDestroy()
    {
        SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
    }

    public void UpdateLevel(int level)
    {
        levelText.text = $"LEVEL: {level}";
    }

    public void StartTimer(float duration)
    {
        StartCoroutine(TimerCoroutine(duration));
    }

    private IEnumerator TimerCoroutine(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = Mathf.Max(duration - elapsedTime, 0f);
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = $"{minutes}:{seconds:00}";
            yield return null;
        }
    }
}
