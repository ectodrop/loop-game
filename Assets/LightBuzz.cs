using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBuzz : MonoBehaviour
{
    public SoundEffect flickerSFX;
    public GameEvent lightFlickerEvent;
    public GameEvent powerOffEvent;
    
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        lightFlickerEvent.AddListener(PlayFlickerSFX);
        powerOffEvent.AddListener(StopBuzz);
    }

    private void OnDisable()
    {
        lightFlickerEvent.RemoveListener(PlayFlickerSFX);
        powerOffEvent.RemoveListener(StopBuzz);
    }

    private void StopBuzz()
    {
        _audioSource.Stop();
    }
    
    private void PlayFlickerSFX()
    {
        StartCoroutine(PlayFlickerSound());
    }
    
    private IEnumerator PlayFlickerSound()
    {
        flickerSFX.Play();
        _audioSource.Pause();
        yield return new WaitForSeconds(flickerSFX.GetAudioClip().length / flickerSFX.pitch);
        _audioSource.UnPause();
    }
}
