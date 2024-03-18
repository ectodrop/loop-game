using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    public float secondsPerCharacter = 1 / 60f;
    // a sound effect will be played for every "charactersPerSFX" characters
    public int charactersPerSFX = 3;
    public GameObject dialogueBox;
    public DialogueChoiceBoxController choiceBox;
    public TextMeshProUGUI dialogueControlsHint;
    public TextMeshProUGUI dialogueSpeaker;
    public TextMeshProUGUI dialogueBody;
    public GameControls gameControls;
    public SoundEffect typewriterSFX;
    public SharedBool timeStoppedFlag;
    public GameEvent DialogueStop;


    private IEnumerator _currentDialogueNodeCoroutine;
    private IEnumerator _currentDialogueCoroutine;
    private IEnumerator _currentChoiceCoroutine;
    private DialogueOptions _currentFlags;
    
    private bool _dialogueShowing;
    private bool _skipScrawl;
    private bool _cancelDialogue;
    private bool _nextDialogue;

    public void Awake()
    {
        dialogueBox.SetActive(false);
    }
    
    /// <summary>
    /// Open the dialogue box from a starting dialogueNode
    /// </summary>
    /// <param name="dialogueNode">starting node</param>
    /// <param name="options">
    /// options that change the behaviour of the dialogue box, combine multiple options with bitwise OR
    /// <list type="bullet">
    /// <item>INTERRUPTING = interrupt the current dialogue box display</item>
    /// <item>NO_INPUT = dialogue cannot be progressed by pressing a key, this means dialogue will be progressed in your script</item>
    /// <item>ALLOW_MOVEMENT = allows the player to move and look around while the dialogue is playing</item>
    /// <item>STOP_TIME = sets the timeStopped shared bool scriptableobject</item>
    /// </list>
    /// </param>
    /// <param name="finishedCallback">calls this function at the very end of the dialogue after it has closed</param>
    /// <param name="choiceCallback">calls this function when a choice is made in the same location as finishedCallback</param>
    public void StartDialogue(
        DialogueNode dialogueNode,
        DialogueOptions options = 0,
        Action finishedCallback = null,
        Action<string> choiceCallback = null)
    {
        if (options.HasFlag(DialogueOptions.INTERRUPTING))
        {
            CancelCurrentDialogue();
        }
        
        if (!_dialogueShowing)
        {
            _currentFlags = options;
            _currentDialogueNodeCoroutine = AnimateDialogueNode(dialogueNode, options, finishedCallback, choiceCallback);
            StartCoroutine(_currentDialogueNodeCoroutine);
        }
    }
    
    public void OnEnable()
    {
        gameControls.Wrapper.UI.DialogueAction.performed += SkipScrawl;
    }

    public void OnDisable()
    {
        gameControls.Wrapper.UI.DialogueAction.performed -= SkipScrawl;
    }

    /// <summary>
    /// Forces the current dialogue to progress to the next dialogue, you will want to call this if you're using the
    /// "NO_INPUT" flag
    /// </summary>
    /// <param name="skip">skips the current dialogue text animation, otherwise we wait for the animation</param>
    public void ProgressDialogue(bool skip = false)
    {
        if (skip)
            _skipScrawl = true;
        _nextDialogue = true;
    }

    /// <summary>
    /// Forcibly closes out of the current dialogue that is playing and does the respective cleanup
    /// </summary>
    public void CancelCurrentDialogue()
    {
        if (_dialogueShowing)
        {
            if (_currentDialogueNodeCoroutine != null)
                StopCoroutine(_currentDialogueNodeCoroutine);
            if (_currentDialogueCoroutine != null)
                StopCoroutine(_currentDialogueCoroutine);
            if (_currentChoiceCoroutine != null)
                StopCoroutine(_currentChoiceCoroutine);
            
            CleanupDialogueBox();
        }
    }
    
    private void SkipScrawl(InputAction.CallbackContext _)
    {
        if (_dialogueShowing && !_currentFlags.HasFlag(DialogueOptions.NO_INPUT))
        {
            _skipScrawl = true;
        }
    }

    private void SetHintText(string text)
    {
        if (!_currentFlags.HasFlag(DialogueOptions.NO_INPUT))
            dialogueControlsHint.text = text;
        else
            dialogueControlsHint.text = "";
    }

    private void InitDialogueBox()
    {
        _dialogueShowing = true;
        dialogueBox.SetActive(true);
        if (_currentFlags.HasFlag(DialogueOptions.STOP_TIME))
        {
            timeStoppedFlag.SetValue(true);
        }
        
        if (!_currentFlags.HasFlag(DialogueOptions.ALLOW_MOVEMENT))
        {
            gameControls.Wrapper.Player.Move.Disable();
            gameControls.Wrapper.Player.Look.Disable();
            gameControls.Wrapper.Player.Sprint.Disable();
            gameControls.Wrapper.Player.Jump.Disable();
            gameControls.Wrapper.Player.Interact.Disable();
            gameControls.Wrapper.Player.TimeStop.Disable();
            gameControls.Wrapper.Player.FastForward.Disable();
        }
    }

    private void CleanupDialogueBox()
    {
        if (_currentFlags.HasFlag(DialogueOptions.STOP_TIME))
        {
            timeStoppedFlag.SetValue(false);
        }
        if (!_currentFlags.HasFlag(DialogueOptions.ALLOW_MOVEMENT))
        {
            gameControls.Wrapper.Player.Look.Enable();
            gameControls.Wrapper.Player.Move.Enable();
            gameControls.Wrapper.Player.Sprint.Enable();
            gameControls.Wrapper.Player.Jump.Enable();
            gameControls.Wrapper.Player.Interact.Enable();
            gameControls.Wrapper.Player.TimeStop.Enable();
            gameControls.Wrapper.Player.FastForward.Enable();
        }
        dialogueBox.SetActive(false);
        _dialogueShowing = false;
        _currentDialogueCoroutine = null;
        _currentDialogueNodeCoroutine = null;
        DialogueStop.TriggerEvent();
    }
    
    private IEnumerator AnimateDialogueNode(
        DialogueNode dialogueNode,
        DialogueOptions options,
        Action finishedCallback,
        Action<string> choiceCallback)
    {
        InitDialogueBox();
        string choice = "";
        for (int i = 0; i < dialogueNode.sentences.Length; i++)
        {
            SetHintText("Skip [E]");
            // blocks and waits for the text to finish animating
            if (i == dialogueNode.sentences.Length - 1 && dialogueNode.HasChoices())
            {
                choiceBox.gameObject.SetActive(true);
                choiceBox.SetChoices(dialogueNode.choices);
            }
            _currentDialogueCoroutine = AnimateDialogue(dialogueNode.sentences[i]); 
            yield return StartCoroutine(_currentDialogueCoroutine);
            _currentDialogueCoroutine = null;

            if (i == dialogueNode.sentences.Length - 1)
            {
                if (dialogueNode.HasChoices())
                {
                    SetHintText("Select [E]");
                    _currentChoiceCoroutine = WaitForChoice();
                    yield return StartCoroutine(_currentChoiceCoroutine);

                    choice = choiceBox.CurrentChoice();
                    choiceBox.gameObject.SetActive(false);
                }
                else
                    SetHintText("Close [E]");
            }
            else
                SetHintText("Next [E]");

            while (!_nextDialogue && !(gameControls.Wrapper.UI.DialogueAction.WasPerformedThisFrame() && !options.HasFlag(DialogueOptions.NO_INPUT)))
            {
                yield return null;
            }

            _nextDialogue = false;
        }
        CleanupDialogueBox();
        
        finishedCallback?.Invoke();
        if (dialogueNode.HasChoices())
            choiceCallback?.Invoke(choice);
    }
    
    private IEnumerator AnimateDialogue(Dialogue dialogue)
    {
        dialogueSpeaker.text = dialogue.Speaker;
        dialogueBody.text = "";
        yield return null;
        _skipScrawl = false;
        for (int i = 0; i < dialogue.Body.Length; i++)
        {
            if (_skipScrawl)
            {
                _skipScrawl = false;
                break;
            }
            // tag detection
            if (dialogue.Body[i] == '<')
            {
                // advance pointer until end of tag
                while (i < dialogue.Body.Length && dialogue.Body[i] != '>')
                {
                    dialogueBody.text += dialogue.Body[i];
                    i++;
                }

                if (i >= dialogue.Body.Length)
                    break;
            }
            dialogueBody.text += dialogue.Body[i];
            if (i % charactersPerSFX == 0)
                typewriterSFX.Play();
            if (".,!?".Contains(dialogue.Body[i]))
                yield return new WaitForSeconds(secondsPerCharacter * 20);
            else
                yield return new WaitForSeconds(secondsPerCharacter);
        }
        
        dialogueBody.text = dialogue.Body; 
        yield return new WaitForSeconds(0.1f);
    }
    public bool IsShowingDialogue()
    {
        return _dialogueShowing;
    }

    private IEnumerator WaitForChoice()
    {
        Vector2 nav;
        // forced pause so they don't accidently miss the choice being made
        yield return new WaitForSeconds(0.2f);
        while (!gameControls.Wrapper.UI.DialogueAction.WasPerformedThisFrame())
        {
            if (gameControls.Wrapper.UI.Navigate.WasPerformedThisFrame())
            {
                nav = gameControls.Wrapper.UI.Navigate.ReadValue<Vector2>();
                if (nav.y > 0)
                {
                    choiceBox.MoveUp();
                }
                else if (nav.y < 0)
                {
                    choiceBox.MoveDown();
                }
            }
            yield return null;
        }
        
    }
}
