using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PowerOutageAudioEvent : MonoBehaviour
{
    [Header("Listening To")]
    public GameEvent powerOutageEvent;
    private AudioSource powerOutageSource;

    private void Start()
    {
        powerOutageSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        powerOutageEvent.AddListener(PlaySound);
    }

    private void OnDisable()
    {
        powerOutageEvent.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        powerOutageSource.Play();
    }
}
