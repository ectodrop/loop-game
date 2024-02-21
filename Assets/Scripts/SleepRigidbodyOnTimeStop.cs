using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FixedUpdate = UnityEngine.PlayerLoop.FixedUpdate;

public class SleepRigidbodyOnTimeStop : MonoBehaviour
{
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;
    private Rigidbody rb;
    private bool sleeping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        timeStopStartEvent.AddListener(SleepRigidbody);
        timeStopEndEvent.AddListener(WakeRigidbody);
    }

    private void OnDisable()
    {
        timeStopStartEvent.RemoveListener(SleepRigidbody);
        timeStopEndEvent.RemoveListener(WakeRigidbody);
    }

    private void SleepRigidbody()
    {
        sleeping = true;
    }

    private void WakeRigidbody()
    {
        sleeping = false;
    }

    private void FixedUpdate()
    {
        if (gameObject.layer != LayerMask.NameToLayer("holdLayer"))
            rb.isKinematic = sleeping;
    }
}
