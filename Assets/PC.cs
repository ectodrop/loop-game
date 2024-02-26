using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class PC : MonoBehaviour
{
    public GameEvent PowerOn;
    public GameEvent PowerOff;
    public GameObject LoadingScreen;
    public GameObject OnScreen;
    public GameObject Button;
    public static bool _canInteract = true;
    public enum Status
    {
        Off,
        Loading,
        On,
        Crash
    }
    public static Status PCStatus;
    private const float LoadingDuration = 5.0f;
    private float LoadingCountDown;

    private void Start()
    {
        LoadingCountDown = LoadingDuration;
        LoadingScreen.SetActive(false);
        OnScreen.SetActive(false);
    }
    void OnEnable()
    {
        PowerOn.AddListener(SetInteractOn);
        PowerOff.AddListener(SetInteractOff);
    }
    void SetInteractOn()
    {
        _canInteract = true;
        PCStatus = Status.Off;
    }
    void SetInteractOff()
    {
        _canInteract = false;
        Crash();
    }
    public static void Boot()
    {
        PCStatus = Status.Loading;
    }
    public void Crash()
    {
        PCStatus = Status.Crash;
        LoadingScreen.SetActive(false);
        OnScreen.SetActive(false);
        LoadingCountDown = LoadingDuration;
    }
    private void Update()
    {
        if (PCStatus == Status.Loading)
        {
            LoadingScreen.SetActive(true);
            LoadingCountDown -= Time.deltaTime;
            if (LoadingCountDown <= 0f)
            {
                PCStatus = Status.On;
            }
        }
        if (PCStatus == Status.On)
        {
            LoadingScreen.SetActive(false);
            LoadingCountDown = LoadingDuration;
            OnScreen.SetActive(true);
        }
    }
    public Status GetStatus()
    {
        return PCStatus;
    }
    public float GetLoadingCountDown()
    {
        return LoadingCountDown;
    }
}
