using UnityEngine;
using TMPro; // Namespace for TextMeshPro
using UnityEngine.UI;

public class CoolantSystem : MonoBehaviour
{
    public TMP_InputField passwordInputField; // Make sure this is the TMP_InputField
    public Button drainCoolantButton;
    public PowerGeneratorSwitch powerGeneratorSwitch; // Reference to the PowerGeneratorSwitch script
    public EmergencyPowerSwitch emergencyPowerSwitch; // Reference to the EmergencyPowerSwitch script
    
    [Header("Triggers")]
    public GameEvent coolantDrainedEvent;

    public void DrainCoolant()
    {
        // Perform the coolant drain
        coolantDrainedEvent.TriggerEvent();
        Debug.Log("Coolant is being drained.");
    }
}

