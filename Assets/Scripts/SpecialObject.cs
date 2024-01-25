using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObject : MonoBehaviour
{
    [SerializeField] TimeIncreaser timeIncreaser;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("hit");
            timeIncreaser.increaseTimeLimit();
        }
    }
}
