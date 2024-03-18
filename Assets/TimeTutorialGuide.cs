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

    public GameEventInt changeExpressionEvent;

    public GameEvent powerOffEvent;

    public ScheduleEvent studyTimeOverEvent;
    
    readonly string _answer = "12:03pm";
    private bool _passed = false;

    private void OnEnable()
    {
        studyTimeOverEvent.AddListener(Ask);
    }
    
    private void OnDisable()
    {
        studyTimeOverEvent.RemoveListener(Ask);
        _timeLoopController.ResumeTime();
    }
    
    void Start()
    {
        _timeLoopController.StopTime();
        if (firstTime.GetValue())
        {
            _dialogueController.StartDialogue(
                dialogueFirstTime,
                options: DialogueOptions.STOP_TIME,
                finishedCallback: () => changeExpressionEvent.TriggerEvent((int)JarvisExpression.Empty));
        }
        else
        {
            _dialogueController.StartDialogue(dialogueNotFirstTime,
                options: DialogueOptions.STOP_TIME,
                finishedCallback: () => changeExpressionEvent.TriggerEvent((int)JarvisExpression.Empty));
        }
        
        lookAtEvent.TriggerEvent(jarvisObject.transform.position);
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
            _timeLoopController.StopTime();
            _dialogueController.StartDialogue(dialogueSuccess, finishedCallback: () => StartCoroutine(ShowPlayerThoughts()));
            _passed = true;
        }
        else
        {
            _dialogueController.StartDialogue(dialogueFail, DialogueOptions.STOP_TIME, finishedCallback: ShowResetDialogue);
            firstTime.SetValue(false);
        }
    }

    private IEnumerator ShowPlayerThoughts()
    {
        powerOffSFX.Play();
        powerOffEvent.TriggerEvent();
        
        changeExpressionEvent.TriggerEvent((int)JarvisExpression.Empty);
        yield return new WaitForSeconds(3f);
        _dialogueController.StartDialogue(dialoguePlayerPowerOutageThoughts, options: DialogueOptions.ALLOW_MOVEMENT | DialogueOptions.NO_INPUT);
    }

    private void ShowResetDialogue()
    {
        _dialogueController.StartDialogue(dialogueReset, options: DialogueOptions.NO_INPUT | DialogueOptions.STOP_TIME);
    }
}
