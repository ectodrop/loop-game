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

    public SoundEffect computerUpdateSFX;
    public SoundEffect computerUpdateCompleteSFX;
    [Header("Listening To")]
    public ScheduleEvent displayPasswordEvent;
    // public SharedBool timeStopped;
    private bool _canInteract = true;
    private AudioSource _audioSource;
    public enum Status
    {
        Off,
        Loading,
        On,
        Password,
        Crash
    }
    private Status PCStatus;

    private void Start()
    {
        PCStatus = Status.Off;
        LoadingScreen.SetActive(false);
        OnScreen.SetActive(false);
        CrashScreen.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        computerUpdateSFX.Play(_audioSource);
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
        if (PCStatus == Status.Off)
        {
            _canInteract = false;
            LoadingScreen.SetActive(true);
        }
        else if (PCStatus == Status.On)
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
        return _canInteract ? "Interact [E]" : "";
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
            computerUpdateCompleteSFX.Play(_audioSource);
            PCStatus = Status.On;
            _canInteract = true;
            LoadingScreen.SetActive(false);
            OnScreen.SetActive(true);
        }
    }
    
    public void Crash()
    {
        _audioSource.Stop();
        PCStatus = Status.Crash;
        LoadingScreen.SetActive(false);
        OnScreen.SetActive(false);
        CrashScreen.SetActive(true);
    }
}
