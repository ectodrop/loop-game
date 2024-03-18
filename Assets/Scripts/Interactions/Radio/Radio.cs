using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public SoundEffect station1;
    public SoundEffect station2;
    public SoundEffect station3;
    public float _interval = 20f;
    public bool debug = false;

    // The station that will trigger the growth
    [Range(1, 3)]
    public int correctStation = 2;

    [Header("Listening To")]
    public GameEvent powerOn;
    public GameEvent powerOff;
    public GameEvent radioButtonClick;

    // Listen to these for the mushroom growth
    [Header("Triggers")]
    public GameEvent startGrow;
    public GameEvent stopGrow;
    // State of the radio
    private bool _hasPower = false;
    private bool _timerOn = false;
    private int _station = 1; // Stations 0, 1, 2
    private SoundEffect currentStation;
    private float _elapsedTime = 0f;

    private void OnEnable()
    {
        powerOn.AddListener(HandlePowerOn);
        powerOff.AddListener(HandlePowerOff);
        radioButtonClick.AddListener(HandleRadioButton);
    }

    private void OnDisable()
    {
        powerOn.RemoveListener(HandlePowerOn);
        powerOff.RemoveListener(HandlePowerOff);
        radioButtonClick.RemoveListener(HandleRadioButton);
    }

    private void HandlePowerOn()
    {
        _hasPower = true;
        StartTimer();
        PlayStation(_station);
    }

    private void HandlePowerOff()
    {
        _hasPower = false;
        MuteAllStations();
    }

    private void HandleRadioButton()
    {
        _elapsedTime = 0f;
        PlayNextStation();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentStation = station1;
        station1.PlayLoop();
        station2.PlayLoop();
        station3.PlayLoop();
        MuteAllStations();
    }

    void MuteAllStations()
    {
        station1.Mute();
        station2.Mute();
        station3.Mute();
    }

    void PlayNextStation()
    {
        if (_station == 3)
        {
            _station = 1;
        }
        else
        {
            _station += 1;
        }

        // Play station
        PlayStation(_station);
    }

    void PlayStation(int station)
    {
        switch (station)
        {
            case 1:
                currentStation = station1;
                station1.Unmute();
                station2.Mute();
                station3.Mute();
                break;
            case 2:
                currentStation = station2;
                station1.Mute();
                station2.Unmute();
                station3.Mute();
                break;
            case 3:
                currentStation = station3;
                station1.Mute();
                station2.Mute();
                station3.Unmute();
                break;
            default:
                currentStation = station1;
                break;
        }
        if (station == correctStation)
        {
            startGrow.TriggerEvent();
            if (debug) // For Debug
            {
                Debug.Log("Growing");
            }
        }
        else
        {
            stopGrow.TriggerEvent();
            if (debug) // For Debug
            {
                Debug.Log("Stopped Growing");
            }
        }
    }

    void StartTimer()
    {
        if (!_timerOn)
        {
            StartCoroutine(Timer());
        }
    }


    IEnumerator Timer()
    {
        _timerOn = true;
        _elapsedTime = 0f;
        int lastSecond = 0; // For Debug

        while (_elapsedTime < _interval)
        {
            if (debug) // For Debug
            {
                int currentSecond = Mathf.FloorToInt(_elapsedTime);
                if (currentSecond != lastSecond)
                {
                    Debug.Log(_elapsedTime);
                }
                lastSecond = currentSecond;
            }
            if (!_hasPower)
            {
                break;
            }
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restart
        _timerOn = false;
        if (_hasPower)
        {
            // Play next radio station
            PlayNextStation();
            StartTimer();
        }
    }
}
