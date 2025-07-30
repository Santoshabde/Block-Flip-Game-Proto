using SNGames.CommonModule;
using UnityEngine;

public class BF_TimerFreezePowerup : BF_BasePowerup
{
    public override void ExcutePowerup()
    {
        powerupsLeft -= 1;

        BF_GameSaveSystem.SavePowerups(powerupType, powerupsLeft);
        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.FreezeTimer, 10f);
        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.PowerupActivated, powerupType);
        
        powerUpAmount.text = powerupsLeft.ToString();
    }
}
