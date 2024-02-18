using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeLoopController : MonoBehaviour, ITimer
{
    public bool debugMode = false;
    public TimeLimitControllerScriptableObject timeLimitController;
    public ScheduleControllerScriptableObject scheduleController;
    public TextMeshProUGUI timerText;
    private float timeCounter;

    private int currentEvent = 0;

    private readonly string _powerOutageName = "PowerOutageEvent";

    // Handle battery usage
    private bool _usingBattery = false;
    
    [Header("Listening To")]
    public GameEvent batteryDraining;
    public GameEvent batteryStopDraining;
    public GameEvent resetLoopEvent;
    public GameEventFloat timeExtendedEvent;

    private void OnEnable()
    {
        batteryDraining.AddListener(HandleBatteryDraining);
        batteryStopDraining.AddListener(HandleBatteryStoppedDraining);
        resetLoopEvent.AddListener(ResetScene);
        timeExtendedEvent.AddListener(HandleTimeExtension);
    }

    private void OnDisable()
    {
        batteryDraining.RemoveListener(HandleBatteryDraining);
        batteryStopDraining.RemoveListener(HandleBatteryStoppedDraining);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeCounter = timeLimitController.currentMaxTime;
        Invoke("ResetScene", timeCounter);
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {
            return;
        }

        if (timeCounter > 0)
        {
            timeCounter -= Time.deltaTime;
            InvokeNextEvent();
            var parts = timeCounter.ToString("N2").Split(".");
            timerText.text = string.Format("{0}:{1}", parts[0], parts[1]);
        }
        else
        {
            timeCounter = 0;
        }
    }

    private void InvokeNextEvent()
    {
        if (currentEvent >= scheduleController.scheduledEvents.Length)
            return;

        var nextEvent = scheduleController.scheduledEvents[currentEvent];
        if (nextEvent.time < timeLimitController.currentMaxTime - timeCounter)
        {
            currentEvent++;

            // If battery is already being used do not trigger the power outage
            if (nextEvent.name == _powerOutageName && _usingBattery)
            {
                return;
            }

            nextEvent.TriggerEvent();
        }
    }

    public void ResetScene()
    {
        if (!debugMode)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public float GetTime()
    {
        return timeLimitController.currentMaxTime;
    }

    public void SetTime(float newTimeLimit)
    {
        timeLimitController.currentMaxTime = newTimeLimit;
    }

    private void HandleTimeExtension(float addedTime)
    {
        SetTime(GetTime() + addedTime);
        ResetScene();
    }

    private void HandleBatteryDraining()
    {
        _usingBattery = true;
    }

    private void HandleBatteryStoppedDraining()
    {
        _usingBattery = false;
    }
}