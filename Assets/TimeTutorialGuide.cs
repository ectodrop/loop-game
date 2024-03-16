using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeTutorialGuide : MonoBehaviour
{
    public DialogueController _dialogueController;
    public TimeLoopController _timeLoopController;
    public SharedBool firstTime;

    public SoundEffect powerOffSFX;
    public GameObject jarvisObject;
    [Header("Dialogues")]
    public DialogueNode dialogueFirstTime;
    public DialogueNode dialogueNotFirstTime;
    public DialogueNode dialogueSuccess;
    public DialogueNode dialogueFail;
    public DialogueNode dialogueReset;
    public DialogueNode dialogueQuiz;
    public DialogueNode dialoguePlayerPowerOutageThoughts;
    [Header("Triggers")]
    public GameEventVector3 lookAtEvent;

    public GameEvent powerOffEvent;
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
        Reset, // 4
        PowerOutage, // 5
    }
    private CurrentDialogue _currentDialogue;

    private void OnEnable()
    {
        // DialogueStop.AddListener(StartObservation);
        studyTimeOverEvent.AddListener(Ask);
    }
    private void OnDisable()
    {
        // DialogueStop.RemoveListener(StartObservation);
        studyTimeOverEvent.RemoveListener(Ask);
        _timeLoopController.ResumeTime();
    }
    void Start()
    {
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
            _dialogueController.StartDialogue(dialogueSuccess, finishedCallback: () => StartCoroutine(ShowPlayerThoughts()));
            _passed = true;
        }
        else
        {
            _dialogueController.StartDialogue(dialogueFail, DialogueOptions.STOP_TIME, finishedCallback: ShowResetDialogue);
            _currentDialogue = CurrentDialogue.Fail;
            firstTime.SetValue(false);
        }
    }

    private IEnumerator ShowPlayerThoughts()
    {
        powerOffSFX.Play();
        powerOffEvent.TriggerEvent();
        _currentDialogue = CurrentDialogue.PowerOutage;
        yield return new WaitForSeconds(3f);
        _dialogueController.StartDialogue(dialoguePlayerPowerOutageThoughts, options: DialogueOptions.ALLOW_MOVEMENT | DialogueOptions.NO_INPUT);
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
            case CurrentDialogue.PowerOutage:
                return 5;
            default:
                return 4;
        }
    }
}
