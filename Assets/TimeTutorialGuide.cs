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

    public GameObject QuestionUI;
    public TMP_InputField answerInputField;
    public Button ConfirmButton;
    [Header("Dialogues")]
    public DialogueNode dialogueFirstTime;
    public DialogueNode dialogueNotFirstTime;
    public DialogueNode dialogueSuccess;
    public DialogueNode dialogueFail;
    public DialogueNode dialogueReset;
    [Header("Triggers")]
    public GameEvent resetLoopEvent;
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;
    public GameEvent LookAtJarvis;
    public GameEvent LookFree;
    public GameEvent LightFlicker;
    public GameEvent HUDEnableEvent;
    public GameEvent HUDDisableEvent;
    [Header("Listening To")]    
    public GameEvent DialogueStop;
    public GameEvent AnswerCorrect;
    public GameEvent AnswerWrong;
    private const float CountDownDuration = 10f;
    private float CountDown;
    private bool _result;
    private string _answer = "12:03";
    private bool _passed = false;

    private enum CurrentDialogue
    {
        FirstTime,
        NotFirstTime,
        Success,
        Fail,
        Reset
    }
    private CurrentDialogue _currentDialogue;

    private void OnEnable()
    {
        DialogueStop.AddListener(StartObservation);
        ConfirmButton.onClick.AddListener(QuestionOff);
        AnswerCorrect.AddListener(Passed);
        AnswerWrong.AddListener(Failed);
    }
    private void OnDisable()
    {
        DialogueStop.RemoveListener(StartObservation);
        ConfirmButton.onClick.RemoveListener(QuestionOff);
        AnswerCorrect.RemoveListener(Passed);
        AnswerWrong.RemoveListener(Failed);
    }
    void Start()
    {
        firstTime.DefaultValue = true;
        Debug.Log(firstTime.GetValue());
        _timeLoopController.StopTime();
        CountDown = CountDownDuration;
        if (firstTime.GetValue())
        {
            _currentDialogue = CurrentDialogue.FirstTime;
            _dialogueController.StartDialogue(dialogueFirstTime, DialogueOptions.STOP_TIME);
        }
        else
        {
            _currentDialogue = CurrentDialogue.NotFirstTime;
            _dialogueController.StartDialogue(dialogueNotFirstTime, DialogueOptions.STOP_TIME);
        }
        
        LookAtJarvis.TriggerEvent();
    }
    void StartObservation()
    {
        if  (_currentDialogue == CurrentDialogue.FirstTime | _currentDialogue == CurrentDialogue.NotFirstTime)
        {
            _timeLoopController.ResumeTime();
            LookFree.TriggerEvent();
            CountDown = CountDownDuration;
        }
    }
    private void Update()
    {
        if (!_dialogueController.IsShowingDialogue())
        {
            CountDown -= Time.deltaTime;
        }
        if (CountDown <= 7f)
        {
            LightFlicker.TriggerEvent();
        }
        if (CountDown <= 0f & (_currentDialogue == CurrentDialogue.FirstTime | _currentDialogue == CurrentDialogue.NotFirstTime))
        {
            _timeLoopController.StopTime();
            Ask();
        }
        if (!_dialogueController.IsShowingDialogue() & _passed)
        {
            Debug.Log("tutorial passed");
        }
        if (!_dialogueController.IsShowingDialogue() & !_passed & _currentDialogue == CurrentDialogue.Fail)
        {
            _currentDialogue = CurrentDialogue.Reset;
            _dialogueController.StartDialogue(dialogueSuccess, DialogueOptions.NO_INPUT | DialogueOptions.STOP_TIME);
        }
    }

    void Ask()
    {
        LookAtJarvis.TriggerEvent();
        QuestionOn();
        answerInputField.onValueChanged.AddListener(delegate { VerifyPassword(); });
    }
    void VerifyPassword()
    {
        string enteredPassword = answerInputField.text;
        if (enteredPassword == _answer)
        {
            _result = true;
        }
        else
        {
            _result = false;
        }
    }
    void QuestionOn()
    {
        HUDDisableEvent.TriggerEvent();
        QuestionUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void QuestionOff()
    {
        if (_result)
        {
            AnswerCorrect.TriggerEvent();
        }
        else
        {
            AnswerWrong.TriggerEvent();
        }
        HUDEnableEvent.TriggerEvent();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        QuestionUI.SetActive(false);
    }
    void Passed()
    {
        _currentDialogue = CurrentDialogue.Success;
        _dialogueController.StartDialogue(dialogueSuccess, DialogueOptions.STOP_TIME);
        _passed = true;
    }
    void Failed()
    {
        _dialogueController.StartDialogue(dialogueFail, DialogueOptions.STOP_TIME);
        _currentDialogue = CurrentDialogue.Fail;
        firstTime.SetValue(false);
    }
}
