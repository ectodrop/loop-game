using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public PC PCObj;
    [SerializeField] TextMeshProUGUI timerText;
    float timer;
    void Start()
    {
        timer = PCObj.GetLoadingCountDown();
    }


    void Update()
    {
        timer = PCObj.GetLoadingCountDown();
        int minutes = Mathf.RoundToInt(timer / 60);
        int seconds = Mathf.RoundToInt(timer % 60);
        timerText.text = "Loading PC\n" + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
