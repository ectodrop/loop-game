using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class EmergencyPowerSwitch : MonoBehaviour
{
    public UnityEngine.UI.Image On;
    public UnityEngine.UI.Image Off;
    int index = 0;
    public PowerGeneratorSwitch powerGeneratorSwitch; // Reference to the PowerGeneratorSwitch script
    public GameEvent EmergencyPowerOnEvent;
    public GameEvent EmergencyPowerOffEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (index == 1)
        {
            // update emergency power
            // i.e. emergencyPower.gameObject.SetActive(false);
        }
        else
        {
            // update emergency power
            // i.e. emergencyPower.gameObject.SetActive(true);
        }
    }

    public void ON()
    {
        index = 0;
        On.gameObject.SetActive(false);
        Off.gameObject.SetActive(true);
        EmergencyPowerOffEvent.TriggerEvent();
    }

    public void OFF()
    {
        Debug.Log(powerGeneratorSwitch.GetIndex());
        if (powerGeneratorSwitch.GetIndex() == 1)
        {
            powerGeneratorSwitch.ON();
        }
        index = 1;
        On.gameObject.SetActive(true);
        Off.gameObject.SetActive(false);
        EmergencyPowerOnEvent.TriggerEvent();
    }

    public int GetIndex()
    {
        return index;
    }
}
