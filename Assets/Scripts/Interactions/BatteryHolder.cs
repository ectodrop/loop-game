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
    private bool _emergencyPower = false;


    // Start is called before the first frame update
    void Start()
    {
        _holder = gameObject.GetComponent<Collider>();
        _batteryCollier = battery.GetComponent<Collider>();
        _holdLayer = LayerMask.NameToLayer("holdLayer");
        _batteryRb = battery.GetComponent<Rigidbody>();
        _batteryScript = battery.GetComponent<Battery>();
        Debug.Log(_batteryScript.GetBatteryLevel().ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (_holder.bounds.Intersects(_batteryCollier.bounds) && battery.layer != _holdLayer && !_holding)
        {
            Debug.Log("Intersection");
            battery.transform.SetParent(gameObject.transform);
            battery.transform.position = gameObject.transform.position;
            _batteryRb.isKinematic = true;
            StartCoroutine(_drainBattery());
            _holding = true;
        }
    }

    private IEnumerator _drainBattery()
    {
        // Loop indefinitely
        while (_batteryScript.GetBatteryLevel() != 0)
        {
            textMeshPro.text = _batteryScript.GetBatteryLevel().ToString();
            _batteryScript.DecreaseBattery(1);
            yield return new WaitForSeconds(_drainRate);
        }
    }
}