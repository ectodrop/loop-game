using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public GameEvent powerOn;
    public GameEvent powerOff;
    public GameEvent radioButton;

    public SoundEffect station1;
    public SoundEffect station2;
    public SoundEffect station3;

    private float _interval = 20f;
    // State of the radio
    private bool _hasPower = false;
    private bool _timerOn = false;
    private int _station = 0; // Stations 0, 1, 2
    
    
    private void OnEnable()
    {
        powerOn.AddListener(HandlePowerOn);
        powerOff.AddListener(HandlePowerOff);
        radioButton.AddListener(HandleRadioButton);
    }

    private void OnDisable()
    {
        powerOn.RemoveListener(HandlePowerOn);
        powerOff.RemoveListener(HandlePowerOff);
        radioButton.RemoveListener(HandleRadioButton);
    }

    private void HandlePowerOn()
    {
        _hasPower = true;
        // Start by playing station 1
        PlayStation(1);
    }

    private void HandlePowerOff()
    {
        _hasPower = false;
    }

    private void HandleRadioButton()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
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
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
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
        float elapsedTime = 0f;

        while (elapsedTime < _interval)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Play next radio station
        

        // Restart
        _timerOn = false;
        StartTimer();
    }
}
