using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectLooper : MonoBehaviour
{
    public SoundEffect soundEffect;
    [Tooltip("if the track is looping")]
    public bool isLooping = true;
    public float repeatingIntervalSeconds = 1f;
    public float initialDelaySeconds = 0f;

    private AudioSource _audioSource;
    private void Start()
    {
        InvokeRepeating("PlaySoundEffect", initialDelaySeconds, repeatingIntervalSeconds);
        _audioSource = GetComponent<AudioSource>();
    }

    private void PlaySoundEffect()
    {
        soundEffect.Play(_audioSource);
    }
}
