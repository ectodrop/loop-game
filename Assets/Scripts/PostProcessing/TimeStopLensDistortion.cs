using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeStopLensDistortion : MonoBehaviour
{
    public AnimationCurve distortionCurve;
    [Header("Listens To")]
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;
    
    private float animationSpeed = 1/60f;
    
    private VolumeProfile profile;

    private LensDistortion lensOverride;
    private ColorCurves colorOverride;
    private ColorAdjustments colorAdjustmentsOverride;
    private Bloom bloomOverride;
    
    // Start is called before the first frame update
    void Awake()
    {
        profile = GetComponent<Volume>().profile;
        profile.TryGet<LensDistortion>(out lensOverride);
        profile.TryGet<ColorCurves>(out colorOverride);
        profile.TryGet<ColorAdjustments>(out colorAdjustmentsOverride);
        profile.TryGet<Bloom>(out bloomOverride);
    }

    private void OnEnable()
    {
        timeStopStartEvent.AddListener(PlayAnimation);
        timeStopEndEvent.AddListener(DeactivateTimeStop);
    }

    private void OnDisable()
    {
        timeStopStartEvent.RemoveListener(PlayAnimation);
        timeStopEndEvent.RemoveListener(DeactivateTimeStop);
    }

    private void DeactivateTimeStop()
    {
        StartCoroutine(DistortionAnimation(false));
    }

    private void PlayAnimation()
    {
        StartCoroutine(DistortionAnimation(true));
    }

    private IEnumerator DistortionAnimation(bool stopping)
    {
        float time = 0;
        while (time < 1.0f)
        {
            if (time > 0.5f)
            {
                colorOverride.active = stopping;
                bloomOverride.active = !stopping;
                colorAdjustmentsOverride.active = stopping;
            }
            lensOverride.intensity.value = distortionCurve.Evaluate(time);
            time += animationSpeed;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
