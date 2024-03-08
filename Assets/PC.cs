using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PC : MonoBehaviour
{
    public GameEvent PowerOn;
    public GameEvent PowerOff;
    public GameObject LoadingScreen;
    public GameObject OnScreen;
    public GameObject CrashScreen;

    public ScheduleEvent displayPasswordEvent;
    // public SharedBool timeStopped;
    private bool _canInteract = true;
    public enum Status
    {
        Loading,
        On,
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
        displayPasswordEvent.AddListener(ShowPassword);
        PowerOn.AddListener(SetInteractOn);
        PowerOff.AddListener(SetInteractOff);
    }

    private void OnDisable()
    {
        displayPasswordEvent.RemoveListener(ShowPassword);
        PowerOn.RemoveListener(SetInteractOn);
        PowerOff.RemoveListener(SetInteractOff);
    }

    void SetInteractOn()
    {
        _canInteract = true;
    }
    
    void SetInteractOff()
    {
        _canInteract = false;
        Crash();
    }

    void ShowPassword()
    {
        if (PCStatus != Status.Crash)
        {
            PCStatus = Status.On;
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
