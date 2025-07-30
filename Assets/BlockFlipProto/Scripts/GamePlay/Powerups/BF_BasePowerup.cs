using UnityEngine;
using UnityEngine.UI;

public abstract class BF_BasePowerup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Button powerupButton;
    [SerializeField] protected Text levelAtWichPowerupUnlocks;
    [SerializeField] private GameObject lockContainer;
    [SerializeField] private GameObject unlockContainer;

    [Header("ONLY DEBUG!!")]
    [SerializeField] protected bool currentStatus;
    [SerializeField] protected int levelInWhichThisUnlocks;

    public bool CurrentStatus => currentStatus;
    public int LevelInWhichThisUnlocks => levelInWhichThisUnlocks;

    public void Init(bool currentStatus, int levelInWhichThisUnlocks)
    {
        this.currentStatus = currentStatus;
        this.levelInWhichThisUnlocks = levelInWhichThisUnlocks;

        levelAtWichPowerupUnlocks.text = $"LEVEL {levelInWhichThisUnlocks}";
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

    public abstract void ExcutePowerup();
}
