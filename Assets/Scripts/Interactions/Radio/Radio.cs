using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public BatteryPackScript radioBatteryPack;
    public TimeSettings timeSettings;
    public AudioSource melody1;
    public AudioSource melody2;
    public AudioSource melody3;
    public DialogueNode[] station1Dialogues;
    public DialogueNode[] station2Dialogues;
    public DialogueNode[] station3Dialogues;
    public GameObject bgmInstrumentals;
    private DialogueNode[][] stationDialogues = new DialogueNode[3][];
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

    private AudioSource[] stations = new AudioSource[3];
    // Listen to these for the mushroom growth
    [Header("Triggers")]
    public GameEvent startGrow;
    public GameEvent stopGrow;
    // State of the radio
    private bool _hasPower = false;
    private bool _timerOn = false;
    private int _station = 0; // Stations 0, 1, 2
    private int numSongChanges = 0;
    private DialogueController _dialogueController;

    private void Start()
    {
        _dialogueController = FindObjectOfType<DialogueController>();
        stations[0] = melody1;
        stations[1] = melody2;
        stations[2] = melody3;
        stationDialogues[0] = station1Dialogues;
        stationDialogues[1] = station2Dialogues;
        stationDialogues[2] = station3Dialogues;
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
        stopGrow.TriggerEvent();
    }

    private void HandleRadioButton()
    {
        _dialogueController.StartDialogue(stationDialogues[_station][numSongChanges], options: DialogueOptions.STOP_TIME,
            choiceCallback: HandleStationChoice);
    }

    private void HandleStationChoice(string choice)
    {
        if (choice == "Station 1")
        {
            _station = 0;
        }
        if (choice == "Station 2")
        {
            _station = 1;
        }
        if (choice == "Station 3")
        {
            _station = 2;
        }
        PlayStation(_station);
    }
    
    void MuteAllStations()
    {
        foreach (var station in stations)
        {
            station.mute = true;
        }

        foreach (var audiosource in bgmInstrumentals.GetComponents<AudioSource>())
        {
            audiosource.volume = 0.2f;
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
        if (stations[station] == correctMelody && _hasPower)
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
        
        if (_hasPower)
        {
            MuteAllStations();
            UnMuteStation(station);
            foreach (var audiosource in bgmInstrumentals.GetComponents<AudioSource>())
            {
                audiosource.volume = 0;
            }
        }
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
            stations[0] = melody2;
            stations[1] = melody3;
            stations[2] = melody1;
        }

        if (numSongChanges == 1)
        {
            stations[0] = melody3;
            stations[1] = melody1;
            stations[2] = melody2;
        }
        PlayStation(_station);

        numSongChanges++;
    }
}
