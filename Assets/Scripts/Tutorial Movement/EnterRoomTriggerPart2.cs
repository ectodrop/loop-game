using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    public GameObject powerDoor;
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
            StartCoroutine(OpenDoor());
        }
    }
    public IEnumerator OpenDoor()
    {
        lookAtEvent.TriggerEvent(powerDoor.transform.position);
        _collider.enabled = false;
        float y = 0;
        while (y < 5f)
        {
            powerDoor.transform.position += new Vector3(0, Time.deltaTime, 0);
            y += Time.deltaTime;
            yield return null;
        }
    }

}
