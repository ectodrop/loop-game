using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public TimeSettings timeSettings;
    public BackgroundMusicScript bgmScript;
    public AudioSource melody1;
    public AudioSource melody2;
    public AudioSource melody3;
    public bool debug = false;

    // The station that will trigger the growth
    public AudioSource correctMelody;

    [Header("Listening To")]
    public GameEvent powerOn;
    public GameEvent powerOff;
    public GameEvent radioButtonClick;
    public GameEvent timeStopStart;
    public GameEvent timeStopEnd;
    public GameEvent changeChannelEvent;
    public GameEvent gamePausedEvent;
    public GameEvent gameUnPausedEvent;
    public GameEvent fastforwardStart;
    public GameEvent fastforwardEnd;

    public AudioSource[] stations = new AudioSource[3];
    // Listen to these for the mushroom growth
    [Header("Triggers")]
    public GameEvent startGrow;
    public GameEvent stopGrow;
    // State of the radio
    private bool _hasPower = false;
    private bool _timerOn = false;
    private int _station = 0; // Stations 0, 1, 2
    private int numSongChanges = 0;

    private void Start()
    {
        stations[0] = melody1;
        stations[1] = melody2;
        stations[2] = melody3;
        MuteAllStations();
    }

    private void OnEnable()
    {
        powerOn.AddListener(HandlePowerOn);
        powerOff.AddListener(HandlePowerOff);
        radioButtonClick.AddListener(HandleRadioButton);
        timeStopStart.AddListener(HandleTimeStopStart);
        timeStopEnd.AddListener(HandleTimeStopEnd);
        changeChannelEvent.AddListener(HandleChangeChannelEvent);
        gamePausedEvent.AddListener(HandleTimeStopStart);
        gameUnPausedEvent.AddListener(HandleTimeStopEnd);
        fastforwardStart.AddListener(HandleStartFastForwardTime);
        fastforwardEnd.AddListener(HandleStopFastForwardTime);
    }

    private void OnDisable()
    {
        powerOn.RemoveListener(HandlePowerOn);
        powerOff.RemoveListener(HandlePowerOff);
        radioButtonClick.RemoveListener(HandleRadioButton);
        timeStopStart.RemoveListener(HandleTimeStopStart);
        timeStopEnd.RemoveListener(HandleTimeStopEnd);
        changeChannelEvent.RemoveListener(HandleChangeChannelEvent);
        gamePausedEvent.RemoveListener(HandleTimeStopStart);
        gameUnPausedEvent.RemoveListener(HandleTimeStopEnd);
        fastforwardStart.RemoveListener(HandleStartFastForwardTime);
        fastforwardEnd.RemoveListener(HandleStopFastForwardTime);
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
        PlayStation(_station);
    }

    private void HandlePowerOff()
    {
        _hasPower = false;
        MuteAllStations();
    }

    private void HandleRadioButton()
    {
        PlayNextStation();
    }
    
    void MuteAllStations()
    {
        foreach (var station in stations)
        {
            station.mute = true;
        }
    }

    void UnMuteStation(int station)
    {
        stations[station].mute = false;
    }

    void PlayNextStation()
    {
        _station = (_station + 1) % 3;

        // Play station
        PlayStation(_station);
    }

    void PlayStation(int station)
    {
        if (stations[station] == correctMelody)
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
        
        MuteAllStations();
        UnMuteStation(station);

        bgmScript.LowerVolume();
    }

    private void ChangePitch(float pitch)
    {
        foreach (var station in stations)
        {
            station.pitch = pitch;
        }
    } 
    private void HandleStartFastForwardTime()
    {
        ChangePitch(timeSettings.fastForwardTimeScale);
    }

    private void HandleStopFastForwardTime()
    {
        ChangePitch(1f);
    }

    private void HandleChangeChannelEvent()
    {
        if (numSongChanges == 0)
        {
            stations[0] = melody3;
            stations[1] = melody1;
            stations[2] = melody2;
        }

        if (numSongChanges == 1)
        {
            stations[0] = melody2;
            stations[1] = melody3;
            stations[2] = melody1;
        }
        PlayStation(_station);

        numSongChanges++;
    }
}
