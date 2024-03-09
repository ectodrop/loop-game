using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleTrigger : MonoBehaviour
{
    public GameObject lasers;
    public GameEvent PowerOn;
    public GameEvent PowerOff;
    float DeathCountDown = 1.0f;


    void OnEnable()
    {
        PowerOn.AddListener(PuddleOn);
        PowerOff.AddListener(PuddleOff);
    }
    void PuddleOn()
    {
        lasers.SetActive(true);
        this.gameObject.SetActive(true);
    }
    void PuddleOff()
    {
        lasers.SetActive(false);
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
