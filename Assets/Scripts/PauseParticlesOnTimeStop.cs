using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseParticlesOnTimeStop : MonoBehaviour
{
    public GameEvent timeStopStart;
    public GameEvent timeStopEnd;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        timeStopStart.AddListener(PauseParticles);
        timeStopEnd.AddListener(ResumeParticles);
    }

    private void OnDisable()
    {
        timeStopStart.RemoveListener(PauseParticles);
        timeStopEnd.RemoveListener(ResumeParticles);
    }

    private void PauseParticles()
    {
        _particleSystem.Pause();
    }

    private void ResumeParticles()
    {
        _particleSystem.Play();
    }
}
