using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PauseAudioSourceOnTimeStop : MonoBehaviour
{
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;
    
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        timeStopStartEvent.AddListener(PauseAudio);
        timeStopEndEvent.AddListener(ResumeAudio);
    }

    private void OnDisable()
    {
        timeStopStartEvent.RemoveListener(PauseAudio);
        timeStopEndEvent.RemoveListener(ResumeAudio);
    }

    private void PauseAudio()
    {
        _audioSource.Pause();
    }

    private void ResumeAudio()
    {
        _audioSource.UnPause();
    }
}
