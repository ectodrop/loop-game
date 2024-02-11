using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BatteryHolder : MonoBehaviour
{
    public GameObject battery;
    public TextMeshPro textMeshPro;
    Collider _holder, _batteryCollier;
    Rigidbody _batteryRb;
    private int _holdLayer;
    private bool _holding = false;
    private Battery _batteryScript;

    private int _drainRate = 1;
    private bool _draining = false;
    private bool _emergencyPower = false;

    // Switch
    public GameObject switchObj;
    private PowerModeSwitch _switchScript;


    // Start is called before the first frame update
    void Start()
    {
        _holder = gameObject.GetComponent<Collider>();
        _batteryCollier = battery.GetComponent<Collider>();
        _holdLayer = LayerMask.NameToLayer("holdLayer");
        _batteryRb = battery.GetComponent<Rigidbody>();
        _batteryScript = battery.GetComponent<Battery>();
        Debug.Log(_batteryScript.GetBatteryLevel().ToString());

        _switchScript = switchObj.GetComponent<PowerModeSwitch>();
    }

    private bool IsPlayerHolding()
    {
        return battery.layer == _holdLayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_holder.bounds.Intersects(_batteryCollier.bounds) && !IsPlayerHolding() && !_holding)
        {
            Debug.Log("Battery Inserted.");
            battery.transform.SetParent(gameObject.transform);
            battery.transform.position = gameObject.transform.position;
            _batteryRb.isKinematic = true;


            // If switch is already on (using emergency power, start draining battery)
            if (_switchScript.UsingEmergencyPower())
            {
                StartBatteryDrain();
            }

            _holding = true;
        }
    }

    public void StartBatteryDrain()
    {
        AllowDrain();
        StartCoroutine(DrainBattery());
    }

    private IEnumerator DrainBattery()
    {
        // Drain battery until empty
        while (_batteryScript.GetBatteryLevel() != 0 && _draining)
        {
            textMeshPro.text = _batteryScript.GetBatteryLevel().ToString();
            _batteryScript.DecreaseBattery(1);
            yield return new WaitForSeconds(_drainRate);
        }
    }

    public bool HasBattery()
    {
        return _holding;
    }

    public void AllowDrain()
    {
        _draining = true;
    }

    public void StopDrain()
    {
        _draining = false;
    }
}