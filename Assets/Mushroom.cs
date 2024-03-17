using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public GameEvent batteryDraining;
    public GameEvent batteryStopDraining;
    public float GrowSpeed = 0.1f;
    private float t = 0.0f;
    private void OnEnable()
    {
        batteryDraining.AddListener(OnBatteryDraining);
        batteryStopDraining.AddListener(OnBatteryStopDraining);
    }
    private void OnDisable()
    {
        batteryDraining.RemoveListener(OnBatteryDraining);
        batteryStopDraining.RemoveListener(OnBatteryStopDraining);
    }

    void OnBatteryDraining()
    {
        StartCoroutine(Grow());
    }
    void OnBatteryStopDraining()
    {
        StopCoroutine(Grow());
    }
    IEnumerator Grow()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("grow");
        Vector3 startSize = transform.localScale;
        Vector3 endSize = new Vector3(startSize.x, 8.0f, startSize.z);
        do
        {
            Debug.Log("loop");
            transform.localScale = new Vector3(startSize.x, Mathf.Lerp(startSize.y, endSize.y, t), startSize.z);
            t += GrowSpeed * Time.deltaTime;
            yield return null;
        }
        while (true);
    }
}
