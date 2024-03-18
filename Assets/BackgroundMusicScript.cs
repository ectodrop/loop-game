using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip normalBGM;
    public AudioClip timestopBGM;

    [Header("Listening To")]
    public GameEvent timeStopStart;
    public GameEvent timeStopEnd;
    

    private void OnEnable()
    {
        timeStopStart.AddListener(HandleTimeStopStart);
        timeStopEnd.AddListener(HandleTimeStopEnd);
    }

    private void OnDisable()
    {
        timeStopStart.RemoveListener(HandleTimeStopStart);
        timeStopEnd.RemoveListener(HandleTimeStopEnd);
    }
    
    private void HandleTimeStopStart()
    {
        audioSource.Stop();
        audioSource.clip = timestopBGM;
        audioSource.Play();
    }
    
    private void HandleTimeStopEnd()
    {
        audioSource.Stop();
        audioSource.clip = normalBGM;
        audioSource.Play();
    }
}
