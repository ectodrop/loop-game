using UnityEngine;
using TMPro; // Namespace for TextMeshPro
using UnityEngine.UI;

public class CoolantSystem : MonoBehaviour
{
    public TMP_InputField passwordInputField; // Make sure this is the TMP_InputField
    public Button drainCoolantButton;
    public PowerGeneratorSwitch powerGeneratorSwitch; // Reference to the PowerGeneratorSwitch script
    public EmergencyPowerSwitch emergencyPowerSwitch; // Reference to the EmergencyPowerSwitch script

    private string adminPassword = "123456"; // Replace with your actual admin password

    private void Start()
    {
        // Disable the drain coolant button initially
        drainCoolantButton.interactable = false;
        // Add a listener to the input field to enable the button when text is entered
        passwordInputField.onValueChanged.AddListener(delegate { ValidatePassword(); });
    }

    private void ValidatePassword()
    {
        // Check if the power is off and the correct password is entered to enable the button
        if (powerGeneratorSwitch.GetIndex() == 0 && emergencyPowerSwitch.GetIndex() == 0 && passwordInputField.text == adminPassword)
        {
            drainCoolantButton.interactable = true;
        }
        else
        {
            drainCoolantButton.interactable = false;
        }
    }

    public void DrainCoolant()
    {
        // Perform the coolant drain
        Debug.Log("Coolant is being drained.");

        // Optionally, reset the input field and disable the button again
        passwordInputField.text = "";
        drainCoolantButton.interactable = false;
    }
}

