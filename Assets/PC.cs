using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PC : MonoBehaviour, IInteractable, ILabel
{
    public GameEvent PowerOn;
    public GameEvent PowerOff;
    public GameObject LoadingScreen;
    public Slider LoadingBar;
    public GameObject OnScreen;
    public GameObject PasswordScreen;
    public GameObject CrashScreen;

    public ScheduleEvent displayPasswordEvent;
    // public SharedBool timeStopped;
    private bool _canInteract = false;
    public enum Status
    {
        Loading,
        On,
        Password,
        Crash
    }
    private Status PCStatus;

    private void Start()
    {
        PCStatus = Status.Loading;
        LoadingScreen.SetActive(true);
        OnScreen.SetActive(false);
        CrashScreen.SetActive(false);
    }
    
    void OnEnable()
    {
        displayPasswordEvent.AddListener(TurnOn);
        PowerOff.AddListener(SetInteractOff);
    }

    private void OnDisable()
    {
        displayPasswordEvent.RemoveListener(TurnOn);
        PowerOff.RemoveListener(SetInteractOff);
    }

    public void Interact()
    {
        if (PCStatus == Status.On)
        {
            _canInteract = false;
            PCStatus = Status.Password;
            OnScreen.SetActive(false);
            PasswordScreen.SetActive(true);
        }
    }

    
    public bool CanInteract()
    {
        return _canInteract;
    }

    public string GetLabel()
    {
        return _canInteract ? "Show Password [E]" : "";
    }
    
    void SetInteractOff()
    {
        _canInteract = false;
        Crash();
    }

    void TurnOn()
    {
        if (PCStatus != Status.Crash)
        {
            PCStatus = Status.On;
            _canInteract = true;
            LoadingScreen.SetActive(false);
            OnScreen.SetActive(true);
        }
    }
    
    public void Crash()
    {
        PCStatus = Status.Crash;
        LoadingScreen.SetActive(false);
        OnScreen.SetActive(false);
        CrashScreen.SetActive(true);
    }
}
