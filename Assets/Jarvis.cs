using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jarvis : MonoBehaviour
{
    /*private enum CurrentDialogue
    {
        FirstTime, // 0
        NotFirstTime, // 1
        Success, // 2
        Fail, // 3
        Reset // 4
    }*/
    public TimeTutorialGuide _tutorialGuide;
    public GameObject Panel;
    public Sprite[] Images;
    private int _check;
    private void Update()
    {
        _check = _tutorialGuide.GetCurrentDialogue();
        if (_check == 0 || _check == 1 || _check == 2)
        {
            Panel.GetComponent<Image>().sprite = Images[0];
        }
        else if (_check == 3)
        {
            Panel.GetComponent<Image>().sprite = Images[1];
        }
        else if (_check == 4)
        {
            Panel.GetComponent<Image>().sprite = Images[2];
        }
    }
}
