using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleTrigger : MonoBehaviour
{
    public GameEvent PowerOn;
    public GameEvent PowerOff;
    float DeathCountDown = 1.0f;

    private Material _material;

    private void Start()
    {
        _material = GetComponentInParent<MeshRenderer>().material;
    }

    void OnEnable()
    {
        PowerOn.AddListener(PuddleOn);
        PowerOff.AddListener(PuddleOff);
    }
    void PuddleOn()
    {
        _material.SetInt("_PowerOn", 1);
        this.gameObject.SetActive(true);
    }
    void PuddleOff()
    {
        _material.SetInt("_PowerOn", 0);
        this.gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            DeathCountDown = 3.0f;
        }
    }
    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            DeathCountDown -= Time.deltaTime;
            if (DeathCountDown <= 0f)
            {
                Debug.Log("Die");
            }
        }
    }
}
