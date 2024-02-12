using UnityEngine;

public class BatterySystemEventsTest : MonoBehaviour
{
    private void OnEnable()
    {
        BatterySystemEvents.BatteryDraining += HandleBatteryDraining;
        BatterySystemEvents.BatteryStoppedDraining += HandleBatteryStoppedDraining;
    }

    private void OnDisable()
    {
        BatterySystemEvents.BatteryDraining -= HandleBatteryDraining;
        BatterySystemEvents.BatteryStoppedDraining -= HandleBatteryStoppedDraining;
    }

    private void HandleBatteryDraining()
    {
        Debug.Log("Battery is draining!");
    }

    private void HandleBatteryStoppedDraining()
    {
        Debug.Log("Battery stopped draining!");
    }
}