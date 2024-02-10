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

        if (timeCounter > 0) {
            timeCounter -= Time.deltaTime;
            InvokeNextEvent();
            var parts = timeCounter.ToString("N2").Split(".");
            timerText.text = string.Format("{0}:{1}", parts[0], parts[1]);
        } else
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
            nextEvent.TriggerEvent();
            currentEvent++;
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
}
