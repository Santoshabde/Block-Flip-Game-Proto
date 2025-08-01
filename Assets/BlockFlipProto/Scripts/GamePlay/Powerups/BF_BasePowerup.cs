using UnityEngine;
using UnityEngine.UI;

public abstract class BF_BasePowerup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected PowerupTypeInGame powerupType;
    [SerializeField] protected Button powerupButton;
    [SerializeField] protected Text levelAtWichPowerupUnlocks;
    [SerializeField] protected Text powerUpAmount;
    [SerializeField] private GameObject lockContainer;
    [SerializeField] private GameObject unlockContainer;

    [Header("ONLY DEBUG!!")]
    [SerializeField] protected bool currentStatus;
    [SerializeField] protected int levelInWhichThisUnlocks;

    protected int powerupsLeft;

    public bool CurrentStatus => currentStatus;
    public int LevelInWhichThisUnlocks => levelInWhichThisUnlocks;

    public void Init(bool currentStatus, int levelInWhichThisUnlocks, int amount)
    {
        this.currentStatus = currentStatus;
        this.levelInWhichThisUnlocks = levelInWhichThisUnlocks;

        levelAtWichPowerupUnlocks.text = $"LEVEL {levelInWhichThisUnlocks}";
        this.powerUpAmount.text = amount.ToString();

        powerupsLeft = amount;
        if (powerupsLeft <= 0)
        {
            powerupsLeft = 0;
            SetPowerupInteractablity(false);
        }

        powerupButton.onClick.AddListener(ExcutePowerup);
    }

    public void SetPowerupInteractablity(bool interactable)
    {
        powerupButton.interactable = powerupsLeft > 0 ? interactable : false;
    }

    public void LockPowerup()
    {
        unlockContainer.SetActive(false);
        lockContainer.SetActive(true);

        powerupButton.interactable = false;
    }

    public void UnlockPowerup()
    {
        lockContainer.SetActive(false);
        unlockContainer.SetActive(true);

        powerupButton.interactable = true;
    }

    void OnDestroy()
    {
        powerupButton.onClick.RemoveListener(ExcutePowerup);
    }

    public abstract void ExcutePowerup();
}
