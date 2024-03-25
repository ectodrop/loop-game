using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicScript : MonoBehaviour
{
    public AudioSource normalAudioSource;
    public AudioSource pausedAudioSource;
    public AudioSource timestopAudioSource;

    [Header("Listening To")]
    public GameEvent timeStopStart;
    public GameEvent timeStopEnd;
    public GameEvent gamePausedEvent;
    public GameEvent gameUnPausedEvent;


    private bool timeStopped = false;

    private void Awake()
    {
        PlayAllSources();
        PauseAllSources();
        normalAudioSource.UnPause();
    }

    public void LowerVolume()
    {
        normalAudioSource.volume = 0.05f;
    }

    public void RaiseVolume()
    {
        normalAudioSource.volume = 0.2f;
    }

    private void OnEnable()
    {
        timeStopStart.AddListener(HandleTimeStopStart);
        timeStopEnd.AddListener(HandleTimeStopEnd);
        gamePausedEvent.AddListener(HandlePauseGame);
        gameUnPausedEvent.AddListener(HandleUnPausedGame);
    }

    private void OnDisable()
    {
        timeStopStart.RemoveListener(HandleTimeStopStart);
        timeStopEnd.RemoveListener(HandleTimeStopEnd);
        gamePausedEvent.RemoveListener(HandlePauseGame);
        gameUnPausedEvent.RemoveListener(HandleUnPausedGame);
    }

    private void PlayAllSources()
    {
        normalAudioSource.Play();
        pausedAudioSource.Play();
        timestopAudioSource.Play();
    }
    
    private void PauseAllSources()
    {
        normalAudioSource.Pause();
        pausedAudioSource.Pause();
        timestopAudioSource.Pause();
    }
    
    private void HandleTimeStopStart()
    {
        PauseAllSources();
        timestopAudioSource.UnPause();
        timeStopped = true;
    }
    
    private void HandleTimeStopEnd()
    {
        PauseAllSources();
        normalAudioSource.UnPause();
        timeStopped = false;
    }

    private void HandlePauseGame()
    {
        PauseAllSources();
        pausedAudioSource.UnPause();
    }

    private void HandleUnPausedGame()
    {
        PauseAllSources();
        if (timeStopped)
        {
            timestopAudioSource.UnPause();
        }
        else
        {
            normalAudioSource.UnPause();
        }
    }
}
