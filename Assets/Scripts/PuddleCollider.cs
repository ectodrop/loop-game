using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleCollider : MonoBehaviour
{
    public GameEvent PowerOn;
    public GameEvent PowerOff;
    void OnEnable()
    {
        PowerOn.AddListener(PuddleOn);
        PowerOff.AddListener(PuddleOff);
    }
    void PuddleOn()
    {
        Debug.Log("Collider On");
        this.gameObject.SetActive(true);
    }
    void PuddleOff()
    {
        Debug.Log("Collider Off");
        this.gameObject.SetActive(false);
    }
}
