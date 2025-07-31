using SNGames.CommonModule;
using UnityEngine;

public class BF_BlockDestroyerPowerup : BF_BasePowerup
{
    public override void ExcutePowerup()
    {
        powerupsLeft -= 1;
        BF_GameSaveSystem.SavePowerups(powerupType, powerupsLeft);
        powerUpAmount.text = powerupsLeft.ToString();
        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.PowerupActivated, powerupType);
    }
}
