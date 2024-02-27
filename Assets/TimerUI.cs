using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public GameObject clockhand;

    public TextMeshProUGUI timerText;

    public TimeSettings timeSettings;

    [Header("Listens To")]
    public GameEvent HUDDisableEvent;
    public GameEvent HUDEnableEvent;

    private void OnEnable()
    {
        HUDDisableEvent.AddListener(DisableChildren);
        HUDEnableEvent.AddListener(EnableChildren);
    }

    private void OnDisable()
    {
        HUDDisableEvent.RemoveListener(DisableChildren);
        HUDEnableEvent.RemoveListener(EnableChildren);
    }

    private void DisableChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void EnableChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        int lastIncrement = (int)timeSettings.CurrentMaxTimeSeconds() / (int)timeSettings.SecondsPerIncrement();
        // a percentage value between 0-1
        float progress = timeSettings.SecondsIntoIncrement() / timeSettings.SecondsPerIncrement();
        // make the text flash if on the last increment
        if (timeSettings.NextIncrement() >= lastIncrement)
        {
            timerText.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        }
        
        timerText.text = timeSettings.currentTimestamp.ToStringAMPM();
        clockhand.transform.localEulerAngles = new Vector3(0,0,-360 * progress);
    }
}
