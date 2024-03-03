using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class BatteryHolder : MonoBehaviour, IInteractable, ILabel
{
    public string HoldingText;
    public string EmptyText;
    public GameObject battery;
    public TextMeshPro textMeshPro;
    public GameObject switchObj;
    // Game Events

    [Header("Triggers")]
    public GameEvent batteryDraining;
    public GameEvent batteryStopDraining;
    public GameEvent playerDropHeldEvent;
    
    // Switch
    private PowerModeSwitch _switchScript;

    Collider _holder, _batteryCollier;
    Rigidbody _batteryRb;
    private int _holdLayer;
    private bool _holding = false;
    private Battery _batteryScript;

    private int _drainRate = 1;
    private bool _draining = false;


    // Offsets to place battery perfectly
    private readonly float _yOffset = 0.2f;
    private readonly float _zOffset = 0.2f;

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

    public string GetLabel()
    {
        if (_holding)
            return "";
        
        if (!_holding && !IsPlayerHolding())
            return "Missing Battery";
        return IsPlayerHolding() ? HoldingText : EmptyText;
    }

    // Update is called once per frame
    public void Interact()
    {
        if (IsPlayerHolding() && !_holding)
        {
            playerDropHeldEvent.TriggerEvent();
            Debug.Log("Battery Inserted.");
            textMeshPro.text = _batteryScript.GetBatteryLevel().ToString();
            GetComponent<BoxCollider>().enabled = false;
            battery.transform.tag = "Untagged";
            battery.transform.SetParent(gameObject.transform);
            battery.transform.position = gameObject.transform.position;
            battery.transform.localPosition += new Vector3(0, _yOffset, _zOffset);
            battery.transform.rotation = gameObject.transform.rotation;
            battery.layer = LayerMask.GetMask("Default");
            _batteryRb.isKinematic = true;


            // If switch is already on (using emergency power, start draining battery)
            if (_switchScript.UsingEmergencyPower())
            {
                StartBatteryDrain();
            }

            _holding = true;
        }
    }

    public bool CanInteract()
    {
        return true;
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
            _batteryScript.DecreaseBattery(10);
            textMeshPro.text = _batteryScript.GetBatteryLevel().ToString();
            yield return new WaitForSeconds(_drainRate);
        }

        // Battery is empty stop draining
        if (_batteryScript.GetBatteryLevel() <= 0)
        {
            StopDrain();
        }
    }

    public bool HasBattery()
    {
        return _holding;
    }

    private void AllowDrain()
    {
        batteryDraining.TriggerEvent();
        _draining = true;
    }

    public void StopDrain()
    {
        batteryStopDraining.TriggerEvent();
        _draining = false;
    }

    public bool IsBatteryEmpty()
    {
        return _batteryScript.GetBatteryLevel() <= 0;
    }
}
