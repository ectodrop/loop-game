using System;

public class BatterySystemEvents
{
    public static event Action BatteryDraining;
    public static event Action BatteryStoppedDraining;

    public static void TriggerBatteryDraining()
    {
        BatteryDraining?.Invoke();
    }

    public static void TriggerBatteryStoppedDraining()
    {
        BatteryStoppedDraining?.Invoke();
    }
}