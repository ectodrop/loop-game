using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeLoopController : MonoBehaviour, ITimer
{
    public bool debugMode = false;
    public float timeLimit = 10f;
    private float timeCounter;

    private bool resetting = false;

    // Start is called before the first frame update
    void Start()
    {
        timeCounter = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {

        if (debugMode || resetting)
        {
            return;
        }

        if (timeCounter > 0) { 
            // decrement time limit
            timeCounter -= Time.deltaTime;

            //Debug.Log(timeCounter);
        } else
        {
            // time limit reached, resetting loop
            timeCounter = timeLimit;
            resetting = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            resetting = false;
            //Debug.Log("Resetting");
        }
    }

    public float GetTime()
    {
        return timeLimit;
    }

    public void SetTime(float newTimeLimit)
    {
        timeLimit = newTimeLimit;
    }
}
