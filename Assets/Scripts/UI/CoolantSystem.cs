using UnityEngine;
using TMPro; // Namespace for TextMeshPro
using UnityEngine.UI;

public class CoolantSystem : MonoBehaviour
{
    public TMP_InputField passwordInputField; // Make sure this is the TMP_InputField
    public Button drainCoolantButton;
    public PowerGeneratorSwitch powerGeneratorSwitch; // Reference to the PowerGeneratorSwitch script
    public EmergencyPowerSwitch emergencyPowerSwitch; // Reference to the EmergencyPowerSwitch script
    public GameEvent coolantDrainedEvent;

    private void Start()
    {
        // Disable the drain coolant button initially
        drainCoolantButton.interactable = false;
        // Add a listener to the input field to enable the button when text is entered
        passwordInputField.onValueChanged.AddListener(delegate { ValidatePassword(); });
    }

    private void ValidatePassword()
    {
        drainCoolantButton.interactable = true;
        coolantDrainedEvent.TriggerEvent();
    }

    public void DrainCoolant()
    {
        // Perform the coolant drain
        Debug.Log("Coolant is being drained.");
    }
}

