using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class SimpleBatteryHolder : MonoBehaviour, IInteractable, ILabel
{
    public GameObject battery;
    public string HoldingText = "Insert Battery (E)";
    public string EmptyText = "Missing Battery";
    public SharedBool timeStoppedFlag;

    [Header("Battery Drain Parameters")]
    public int drainRate = 5;
    public int drainIntervalSeconds = 1;

    [Header("Triggers")]
    public GameEvent powerOn;
    public GameEvent powerOff;
    public GameEvent playerDropHeldEvent;

    [Header("Listening To")]
    public GameEvent playerPickedUp;

    // Battery Object
    private TextMeshPro _batteryPercentageText;
    private GameObject _batteryPercentageBar;
    private float _originalBatteryScale;
    private Vector3 _originalBatteryPos;
    private Battery _batteryScript;
    private int _holdLayer;
    Rigidbody _batteryRb;

    // Booleans for the battery holder state
    private bool _hasBattery = false;
    private bool _isDraining = false;

    // Offsets to place battery perfectly
    private readonly float _yOffset = 0.2f;
    private readonly float _zOffset = 0.2f;

    private void OnEnable()
    {
        playerPickedUp.AddListener(HandlePlayerPickedUp);
    }

    private void OnDisable()
    {
        playerPickedUp.RemoveListener(HandlePlayerPickedUp);
    }
    private void HandlePlayerPickedUp()
    {
        if (IsPlayerHolding() && _hasBattery)
        {
            _hasBattery = false;
            StopDrain();
            GetComponent<BoxCollider>().enabled = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _holdLayer = LayerMask.NameToLayer("holdLayer");
        _batteryScript = battery.GetComponent<Battery>();
        _batteryRb = battery.GetComponent<Rigidbody>();
        // Get objects from battery
        _batteryPercentageText = battery.transform.Find("PercentText").GetComponent<TextMeshPro>();
        _batteryPercentageBar = battery.transform.Find("PercentBar").GameObject();
        _originalBatteryScale = _batteryPercentageBar.transform.localScale.x;
        _originalBatteryPos = _batteryPercentageBar.transform.localPosition;
    }

    public void Interact()
    {
        // Put battery inside
        if (IsPlayerHolding() && !_hasBattery)
        {
            playerDropHeldEvent.TriggerEvent();
            Debug.Log("Battery Inserted.");
            GetComponent<BoxCollider>().enabled = false;
            battery.transform.tag = "canPickUp";
            battery.layer = LayerMask.NameToLayer("Interactable");
            battery.transform.SetParent(gameObject.transform);
            battery.transform.position = gameObject.transform.position;
            battery.transform.localPosition += new Vector3(0, _yOffset, _zOffset);
            battery.transform.rotation = gameObject.transform.rotation;
            _batteryRb.isKinematic = true;

            _hasBattery = true;
            _isDraining = true;

            // Only trigger if battery has charge
            if (_batteryScript.GetBatteryLevel() > 0)
            {
                powerOn.TriggerEvent();
            }
            StartCoroutine(DrainBattery());
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    private bool IsPlayerHolding()
    {
        return battery.layer == _holdLayer;
    }

    public string GetLabel()
    {
        if (_hasBattery)
            return "";

        if (!_hasBattery && !IsPlayerHolding())
            return EmptyText;

        return IsPlayerHolding() ? HoldingText : EmptyText;
    }

    private IEnumerator DrainBattery()
    {
        // Drain battery until empty
        while (_batteryScript.GetBatteryLevel() != 0 && _isDraining)
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

    private void StopDrain()
    {
        powerOff.TriggerEvent();
        _isDraining = false;
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
