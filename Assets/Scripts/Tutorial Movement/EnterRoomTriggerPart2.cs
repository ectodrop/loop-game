using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    public DoorScript powerDoor;
    [Header("Triggers")]
    public GameEventVector3 lookAtEvent;

    private BoxCollider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lookAtEvent.TriggerEvent(powerDoor.transform.position);
            powerDoor.OpenDoor();
            _collider.enabled = false;
        }
    }

}
