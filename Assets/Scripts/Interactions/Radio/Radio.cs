using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource melody1;
    public AudioSource melody2;
    public AudioSource melody3;
    public float _interval = 20f;
    public bool debug = false;
    
    // The station that will trigger the growth
    [Range(0, 2)]
    public int correctStation = 2;

    [Header("Listening To")]
    public GameEvent powerOn;
    public GameEvent powerOff;
    public GameEvent radioButtonClick;
    public GameEvent timeStopStart;
    public GameEvent timeStopEnd;

    public AudioSource[] stations = new AudioSource[3];
    // Listen to these for the mushroom growth
    [Header("Triggers")]
    public GameEvent startGrow;
    public GameEvent stopGrow;
    // State of the radio
    private bool _hasPower = false;
    private bool _timerOn = false;
    private int _station = 0; // Stations 0, 1, 2
    private float _elapsedTime = 0f;

    private void Start()
    {
        stations[0] = melody1;
        stations[1] = melody2;
        stations[2] = melody3;
        foreach (var melody in stations)
        {
            melody.mute = true;
        }
    }

    private void OnEnable()
    {
        powerOn.AddListener(HandlePowerOn);
        powerOff.AddListener(HandlePowerOff);
        radioButtonClick.AddListener(HandleRadioButton);
        timeStopStart.AddListener(HandleTimeStopStart);
        timeStopEnd.AddListener(HandleTimeStopEnd);
    }

    private void OnDisable()
    {
        powerOn.RemoveListener(HandlePowerOn);
        powerOff.RemoveListener(HandlePowerOff);
        radioButtonClick.RemoveListener(HandleRadioButton);
        timeStopStart.RemoveListener(HandleTimeStopStart);
        timeStopEnd.RemoveListener(HandleTimeStopEnd);
    }

    private void HandleTimeStopStart()
    {
        foreach (var melody in stations)
        {
            melody.Pause();
        }
    }

    private void HandleTimeStopEnd()
    {
        foreach (var melody in stations)
        {
            melody.UnPause();
        }
    }

    private void HandlePowerOn()
    {
        _hasPower = true;
        StartTimer();
        stations[_station].mute = false;
        PlayStation(_station);
    }

    private void HandlePowerOff()
    {
        _hasPower = false;
        stations[_station].mute = true;
    }

    private void HandleRadioButton()
    {
        _elapsedTime = 0f;
        PlayNextStation();
    }

    void PlayNextStation()
    {
        _station = (_station + 1) % 3;

        // Play station
        PlayStation(_station);
    }

    void PlayStation(int station)
    {
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
        
        // mute other stations
        for (int i = 0; i < stations.Length; i++)
        {
            if (i == station)
            {
                stations[i].mute = false;
            }
            else
            {
                bgm.volume = 0.05f;
                stations[i].mute = true;
            }
        }
    }

    void StartTimer()
    {
        if (!_timerOn)
        {
            // StartCoroutine(Timer());
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
