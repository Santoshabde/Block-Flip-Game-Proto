using System;
using DG.Tweening;
using SNGames.CommonModule;
using UnityEngine;
using UnityEngine.UI;

public class BF_Dlalog_LevelCompleted : BaseUIDialog
{
    [SerializeField] private GameObject coinsIndication;
    [SerializeField] private GameObject banner;
    [SerializeField] private Button buttonNextLevel;

    private Vector3 originalCoinsIndicationPosition;
    private Vector3 originalBannerPosition;
    private Vector3 originalButtonNextLevelPosition;

    public void Init()
    {

    }

    public override void OnOpenDialog()
    {
        originalCoinsIndicationPosition = coinsIndication.transform.position;
        originalBannerPosition = banner.transform.position;
        originalButtonNextLevelPosition = buttonNextLevel.transform.position;

        coinsIndication.transform.position = new Vector3(originalCoinsIndicationPosition.x - 700f, originalCoinsIndicationPosition.y, originalCoinsIndicationPosition.z);
        banner.transform.position = new Vector3(originalBannerPosition.x, originalBannerPosition.y + 700f, originalBannerPosition.z);
        buttonNextLevel.transform.position = new Vector3(originalButtonNextLevelPosition.x, originalButtonNextLevelPosition.y - 700f, originalButtonNextLevelPosition.z);

        coinsIndication.SetActive(true);
        banner.SetActive(true);
        buttonNextLevel.gameObject.SetActive(true);

        coinsIndication.transform.DOMove(originalCoinsIndicationPosition, 0.6f);
        banner.transform.DOMove(originalBannerPosition, 0.6f);
        buttonNextLevel.transform.DOMove(originalButtonNextLevelPosition, 0.6f);

        buttonNextLevel.onClick.AddListener(OnNextLevelStart);

        base.OnOpenDialog();
    }

    private void OnNextLevelStart()
    {
        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.StartNextLevel);

        OnCloseDialog();
    }

    public override void OnCloseDialog()
    {
        base.OnCloseDialog();

        buttonNextLevel.onClick.RemoveListener(OnNextLevelStart);
    }
}
