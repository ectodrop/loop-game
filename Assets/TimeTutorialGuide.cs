using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeTutorialGuide : MonoBehaviour
{
    public DialogueController _dialogueController;
    public TimeLoopController _timeLoopController;
    public SharedBool firstTime;

    public GameObject jarvisObject;
    [Header("Dialogues")]
    public DialogueNode dialogueFirstTime;
    public DialogueNode dialogueNotFirstTime;
    public DialogueNode dialogueSuccess;
    public DialogueNode dialogueFail;
    public DialogueNode dialogueReset;
    public DialogueNode dialogueQuiz;
    [Header("Triggers")]
    public GameEventVector3 lookAtEvent;
    [Header("Listening To")]    
    public GameEvent DialogueStop;

    public ScheduleEvent studyTimeOverEvent;
    
    readonly string _answer = "12:03pm";
    private bool _passed = false;

    private enum CurrentDialogue
    {
        FirstTime, // 0
        NotFirstTime, // 1
        Success, // 2
        Fail, // 3
        Reset // 4
    }
    private CurrentDialogue _currentDialogue;

    private void OnEnable()
    {
        DialogueStop.AddListener(StartObservation);
        studyTimeOverEvent.AddListener(Ask);
    }
    private void OnDisable()
    {
        DialogueStop.RemoveListener(StartObservation);
        studyTimeOverEvent.RemoveListener(Ask);
    }
    void Start()
    {
        Debug.Log(firstTime.GetValue());
        _timeLoopController.StopTime();
        if (firstTime.GetValue())
        {
            _currentDialogue = CurrentDialogue.FirstTime;
            _dialogueController.StartDialogue(dialogueFirstTime, options: DialogueOptions.STOP_TIME);
        }
        else
        {
            _currentDialogue = CurrentDialogue.NotFirstTime;
            _dialogueController.StartDialogue(dialogueNotFirstTime, options: DialogueOptions.STOP_TIME);
        }
        
        lookAtEvent.TriggerEvent(jarvisObject.transform.position);
    }
    void StartObservation()
    {
        if  (_currentDialogue == CurrentDialogue.FirstTime | _currentDialogue == CurrentDialogue.NotFirstTime)
        {
            _timeLoopController.ResumeTime();
        }
    }
    void Ask()
    {
        lookAtEvent.TriggerEvent(jarvisObject.transform.position);
        _dialogueController.StartDialogue(dialogueQuiz, options: DialogueOptions.STOP_TIME, choiceCallback: VerifyPassword);
    }
    
    void VerifyPassword(string enteredPassword)
    {
        if (enteredPassword == _answer)
        {
            _currentDialogue = CurrentDialogue.Success;
            _timeLoopController.StopTime();
            _dialogueController.StartDialogue(dialogueSuccess);
            _passed = true;
        }
        else
        {
            _dialogueController.StartDialogue(dialogueFail, DialogueOptions.STOP_TIME, finishedCallback: ShowResetDialogue);
            _currentDialogue = CurrentDialogue.Fail;
            firstTime.SetValue(false);
        }
    }

    private void ShowResetDialogue()
    {
        _currentDialogue = CurrentDialogue.Reset;
        _dialogueController.StartDialogue(dialogueReset, options: DialogueOptions.NO_INPUT | DialogueOptions.STOP_TIME);
    }
    
    public int GetCurrentDialogue()
    {
        switch (_currentDialogue)
        {
            case CurrentDialogue.FirstTime:
                return 0;
            case CurrentDialogue.NotFirstTime:
                return 1;
            case CurrentDialogue.Success:
                return 2;
            case CurrentDialogue.Fail:
                return 3;
            default:
                return 4;
        }
    }
}
