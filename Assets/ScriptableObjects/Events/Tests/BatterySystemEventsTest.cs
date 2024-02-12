using UnityEngine;

public class BatterySystemEventsTest : MonoBehaviour
{
    public GameEvent batteryDraining;
    public GameEvent batteryStopDraining;

    private void OnEnable()
    {
        batteryDraining.AddListener(HandleBatteryDraining);
        batteryStopDraining.AddListener(HandleBatteryStoppedDraining);
    }

    private void OnDisable()
    {
        batteryDraining.RemoveListener(HandleBatteryDraining);
        batteryStopDraining.RemoveListener(HandleBatteryStoppedDraining);
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