using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public SoundEffect station1;
    public SoundEffect station2;
    public SoundEffect station3;
    
    [Header("Listening To")]
    public GameEvent powerOn;
    public GameEvent powerOff;
    public GameEvent radioButtonClick;

    [Header("Triggers")]
    public GameEvent startGrow;
    public GameEvent stopGrow;

    private float _interval = 20f;
    // State of the radio
    private bool _hasPower = false;
    private bool _timerOn = false;
    private int _station = 1; // Stations 0, 1, 2
    private SoundEffect currentStation;

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
        PlayStation(_station);
    }

    private void HandlePowerOff()
    {
        _hasPower = false;
    }

    private void HandleRadioButton()
    {
        currentStation.Stop();
        PlayNextStation();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
        currentStation = station1;
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
                break;
            case 2:
                currentStation = station2;
                break;
            case 3:
                currentStation = station3;
                break;
            default:
                currentStation = station1;
                break;
        }
        currentStation.Play();
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
        float elapsedTime = 0f;

        while (elapsedTime < _interval)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Play next radio station
        PlayNextStation();

        // Restart
        _timerOn = false;
        StartTimer();
    }
}
