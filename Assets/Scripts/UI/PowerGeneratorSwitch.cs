using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PowerGeneratorSwitch : MonoBehaviour
{
    public UnityEngine.UI.Image On;
    public UnityEngine.UI.Image Off;
    int index = 0; // 0 for OFF, 1 for ON
    public GameEvent PowerSwitchOnEvent;
    public GameEvent PowerSwitchOffEvent;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (index == 0)
        {
            // update power generator
            // i.e. powerGenerator.gameObject.SetActive(false);
        }
        else
        {
            // update power generator
            // i.e. powerGenerator.gameObject.SetActive(true);
        }
    }

    public void ON()
    {
        index = 0;
        On.gameObject.SetActive(false);
        Off.gameObject.SetActive(true);
        PowerSwitchOffEvent.TriggerEvent();
    }

    public void OFF()
    {
        index = 1;
        On.gameObject.SetActive(true);
        Off.gameObject.SetActive(false);
        PowerSwitchOnEvent.TriggerEvent();
    }

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int i)
    {
        index = i;
    }
}
