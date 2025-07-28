using System;
using System.Collections;
using BlockFlipProto.Level;
using DG.Tweening;
using SNGames.CommonModule;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Transform topPannel;
    [SerializeField] private Transform bottomPannel;

    [SerializeField] private Text timerText;
    [SerializeField] private Text levelText;

    private Vector3 originalTopPannelPosition;
    private Vector3 originalBottomPannelPosition;

    void Start()
    {
        originalTopPannelPosition = topPannel.position;
        originalBottomPannelPosition = bottomPannel.position;

        topPannel.position = new Vector3(originalTopPannelPosition.x, originalTopPannelPosition.y + 500f, originalTopPannelPosition.z);
        bottomPannel.position = new Vector3(originalBottomPannelPosition.x, originalBottomPannelPosition.y - 500f, originalBottomPannelPosition.z);

        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
        SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.LevelComplete, LevelComplete);
    }

    void OnDestroy()
    {
        SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
        SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.LevelComplete, LevelComplete);
    }

    private void AnimateTopPannelUnHide()
    {
        topPannel.gameObject.SetActive(true);
        topPannel.DOMoveY(originalTopPannelPosition.y, 1f);
    }

    private void AnimateBottomPannelUnHide()
    {
        bottomPannel.gameObject.SetActive(true);
        bottomPannel.DOMoveY(originalBottomPannelPosition.y, 1f);
    }

    public void AnimateTopPannelHide()
    {
        topPannel.DOMoveY(originalTopPannelPosition.y + 500f, 1f).OnComplete(() => { topPannel.gameObject.SetActive(false); });
    }

    public void AnimateBottomPannelHide()
    {
        bottomPannel.DOMoveY(originalBottomPannelPosition.y - 500f, 1f).OnComplete(() => { bottomPannel.gameObject.SetActive(false); });
    }

    private void FreshLevelStarted(object obj)
    {
        BF_LevelData levelData = (BF_LevelData)obj;
        UpdateLevel(levelData.levelNumber);
        StartTimer(levelData.levelTimeInSeconds);

        AnimateTopPannelUnHide();
        AnimateBottomPannelUnHide();
    }

    private void LevelComplete(object obj)
    {
        AnimateTopPannelHide();
        AnimateBottomPannelHide();
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
