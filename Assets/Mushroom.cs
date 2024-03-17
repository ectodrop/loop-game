using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public GameEvent batteryDraining;
    public GameEvent batteryStopDraining;
    public float GrowSpeed = 0.001f;
    private float growHeightLimit = 4.0f;
    private bool _growing = false;
    private Vector3 startPos;
    private Vector3 endPos;
    private void Start()
    {
        startPos = gameObject.transform.position;
        endPos = new Vector3(startPos.x, growHeightLimit, startPos.z);
    }
    private void OnEnable()
    {
        batteryDraining.AddListener(Grow);
        batteryStopDraining.AddListener(StopGrow);
    }
    private void OnDisable()
    {
        batteryDraining.RemoveListener(Grow);
        batteryStopDraining.RemoveListener(StopGrow);
    }
    private void FixedUpdate()
    {
        if (_growing)
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, GrowSpeed, 0);
            if (gameObject.transform.position.y >= growHeightLimit)
            {
                StopGrow();
            }
        }
    }
    void Grow()
    {
        _growing = true;
    }
    void StopGrow()
    {
        _growing = false;
    }
}
