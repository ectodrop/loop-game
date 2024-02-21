using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;

    private bool stopped;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!stopped)
            {
                stopped = true;
                timeStopStartEvent.TriggerEvent();
            }
            else
            {
                stopped = false;
                timeStopEndEvent.TriggerEvent();
            }
        }
    }
}
