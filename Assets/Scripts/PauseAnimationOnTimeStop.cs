using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimationOnTimeStop : MonoBehaviour
{
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        timeStopStartEvent.AddListener(DisableAnimator);
        timeStopEndEvent.AddListener(EnableAnimator);
    }

    private void OnDisable()
    {
        timeStopStartEvent.RemoveListener(DisableAnimator);
        timeStopEndEvent.RemoveListener(EnableAnimator);
    }

    private void EnableAnimator()
    {
        _animator.enabled = true;
    }
    
    private void DisableAnimator()
    {
        _animator.enabled = false;
    }
}
