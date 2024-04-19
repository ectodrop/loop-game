using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PauseAudioSourceOnTimeStop : MonoBehaviour
{
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;

    public bool pauseOnGamePause;
    public GameEvent gamePausedEvent;
    public GameEvent gameUnPausedEvent;
    
    private AudioSource[] _audioSources;

    private void Start()
    {
        _audioSources = GetComponents<AudioSource>();
    }

    private void OnEnable()
    {
        timeStopStartEvent.AddListener(PauseAudio);
        timeStopEndEvent.AddListener(ResumeAudio);
        if (pauseOnGamePause)
        {
            gamePausedEvent.AddListener(PauseAudio);
            gameUnPausedEvent.AddListener(ResumeAudio);
        }
    }

    private void OnDisable()
    {
        timeStopStartEvent.RemoveListener(PauseAudio);
        timeStopEndEvent.RemoveListener(ResumeAudio);
        if (pauseOnGamePause)
        {
            gamePausedEvent.RemoveListener(PauseAudio);
            gameUnPausedEvent.RemoveListener(ResumeAudio);
        }
    }

    private void PauseAudio()
    {
        foreach (var audioSource in GetComponents<AudioSource>())
        {
            audioSource.Pause();
        }
    }

    private void ResumeAudio()
    {
        foreach (var audioSource in GetComponents<AudioSource>())
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            audioSource.UnPause();
        }
    }
}
