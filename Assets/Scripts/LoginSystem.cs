using UnityEngine;
using TMPro; // Namespace for TextMeshPro
using UnityEngine.UI;


public class LoginSystem : MonoBehaviour

{
    public TMP_InputField passwordInputField; // Make sure this is the TMP_InputField
    public Button loginButton;

    public GameEvent controlPanelInteracted;

    public GameEvent HUDEnableEvent, HUDDisableEvent;

    public GameObject loginPanelUI;
    public GameObject controlPanelUI;

    private string adminPassword = "123456";

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        loginButton.interactable = false; // Start with the button disabled
        passwordInputField.onValueChanged.AddListener(delegate { VerifyPassword(); });
    }

    public void VerifyPassword()
    {
        string enteredPassword = passwordInputField.text;
        if (enteredPassword == adminPassword)
        {
            loginButton.interactable = true; // Only enable the button if the password is correct
        }
        else
        {
            Debug.Log("Incorrect Password.");
            loginButton.interactable = false; // Optionally, keep the button disabled or provide feedback
        }
    }

    private void OnLoginButtonClicked()
    {
        loginPanelUI.SetActive(false);
        // Assuming the password has already been verified to enable this button
        controlPanelInteracted.TriggerEvent();
        // Optionally, disable the login UI
    }
}
