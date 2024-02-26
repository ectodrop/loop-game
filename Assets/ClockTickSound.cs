using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTickSound : MonoBehaviour
{
    public TimeSettings timeSettings;
    public GameEvent timestopStartEvent;
    public GameEvent timestopEndEvent;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        timestopStartEvent.AddListener(PauseAudio);
        timestopEndEvent.AddListener(PlayAudio);
    }

    private void OnDisable()
    {
        timestopStartEvent.RemoveListener(PauseAudio);
        timestopEndEvent.RemoveListener(PlayAudio);
    }

    private void PauseAudio()
    {
        _audioSource.Pause();
    }
    
    private void PlayAudio()
    {
        _audioSource.Play();
    }
}
