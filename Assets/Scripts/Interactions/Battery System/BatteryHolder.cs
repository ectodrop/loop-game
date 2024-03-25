using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Serialization;

public class BatteryHolder : MonoBehaviour, IInteractable, ILabel
{
    public string HoldingText;
    public string EmptyText;
    public GameObject battery;
    public GameObject switchObj;
    public SoundEffect emergencyPowerOnSFX;
    
    // Battery UI
    private TextMeshPro _batteryPercentageText;
    private GameObject _batteryPercentageBar;
    private float _originalBatteryScale;
    private Vector3 _originalBatteryPos;

    // Game Events

    public SharedBool timeStoppedFlag;
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
    
    [Header("Battery Drain Parameters")]
    public int drainRate = 5;
    public int drainIntervalSeconds = 1;
    
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

        // Get objects from battery
        _batteryPercentageText = battery.transform.Find("PercentText").GetComponent<TextMeshPro>();
        _batteryPercentageBar = battery.transform.Find("PercentBar").GameObject();
        _originalBatteryScale = _batteryPercentageBar.transform.localScale.x;
        _originalBatteryPos = _batteryPercentageBar.transform.localPosition;
    }

    private bool IsPlayerHolding()
    {
        return battery.layer == _holdLayer;
    }

    public string GetLabel()
    {
        if (IsPlayerHolding() && !_holding)
            return HoldingText;

        return "";
    }

    // Update is called once per frame
    public void Interact()
    {
        if (IsPlayerHolding() && !_holding)
        {
            playerDropHeldEvent.TriggerEvent();
            Debug.Log("Battery Inserted.");
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
        emergencyPowerOnSFX.Play();
        AllowDrain();
        StartCoroutine(DrainBattery());
    }

    private IEnumerator DrainBattery()
    {
        // Drain battery until empty
        while (_batteryScript.GetBatteryLevel() != 0 && _draining)
        {
            if (!timeStoppedFlag.GetValue())
            {
                _batteryScript.DecreaseBattery(drainRate);
                _batteryPercentageText.text = _batteryScript.GetBatteryLevel().ToString();
                AnimateBar();
            }

            yield return new WaitForSeconds(drainIntervalSeconds);
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

    private float CalculateScale(int percentage)
    {
        return percentage * 0.01f * _originalBatteryScale;
    }

    private void AnimateBar()
    {
        float scale = CalculateScale(_batteryScript.GetBatteryLevel());

        _batteryPercentageBar.transform.localScale = new Vector3(scale,
            _batteryPercentageBar.transform.localScale.y, _batteryPercentageBar.transform.localScale.z);

        // Offset the position so the bottom of the bar stays fixed
        _batteryPercentageBar.transform.localPosition = _originalBatteryPos - new Vector3(0,
            1 - _batteryScript.GetBatteryLevel() * 0.01f,
            0);
    }
}
