using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeIncreaser : MonoBehaviour
{
    TimeLoopController timeController;

    [SerializeField] float amountTimeIncrease = 10f;


    private void Start()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeLoopController>();
    }

    public void increaseTimeLimit()
    {
        float currentTime = timeController.GetTime();
        timeController.SetTime(currentTime + amountTimeIncrease);
    }
}
