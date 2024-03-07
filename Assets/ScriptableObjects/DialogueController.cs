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
    public TextMeshProUGUI dialogueControlsHint;
    public TextMeshProUGUI dialogueSpeaker;
    public TextMeshProUGUI dialogueBody;
    public GameControls gameControls;
    public SoundEffect typewriterSFX;


    private IEnumerator _currentDialogueNodeCoroutine;
    private IEnumerator _currentDialogueCoroutine;
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
    /// </list>
    /// </param>
    public void StartDialogue(DialogueNode dialogueNode, DialogueOptions options = 0)
    {
        if (options.HasFlag(DialogueOptions.INTERRUPTING))
        {
            CancelCurrentDialogue();
        }
        
        if (!_dialogueShowing)
        {
            _currentFlags = options;
            _currentDialogueNodeCoroutine = AnimateDialogueNode(dialogueNode, options);
            StartCoroutine(_currentDialogueNodeCoroutine);
        }
    }
    
    public void OnEnable()
    {
        gameControls.Wrapper.Player.Interact.performed += SkipScrawl;
    }

    public void OnDisable()
    {
        gameControls.Wrapper.Player.Interact.performed -= SkipScrawl;
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
            CleanupDialogueBox();
        }
    }
    
    private void SkipScrawl(InputAction.CallbackContext _)
    {
        if (_dialogueShowing && !_currentFlags.HasFlag(DialogueOptions.NO_INPUT))
            _skipScrawl = true;
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
        if (!_currentFlags.HasFlag(DialogueOptions.ALLOW_MOVEMENT))
        {
            gameControls.Wrapper.Player.Move.Disable();
            gameControls.Wrapper.Player.Look.Disable();
        }
        
    }

    private void CleanupDialogueBox()
    {
        if (!_currentFlags.HasFlag(DialogueOptions.ALLOW_MOVEMENT))
        {
            gameControls.Wrapper.Player.Look.Enable();
            gameControls.Wrapper.Player.Move.Enable();
        }
        dialogueBox.SetActive(false);
        _dialogueShowing = false;
        _currentDialogueCoroutine = null;
        _currentDialogueNodeCoroutine = null;
    }
    
    private IEnumerator AnimateDialogueNode(DialogueNode dialogueNode, DialogueOptions options)
    {
        InitDialogueBox();
        for (int i = 0; i < dialogueNode.sentences.Length; i++)
        {
            SetHintText("Skip [E]");
            // blocks and waits for the text to finish animating
            _currentDialogueCoroutine = AnimateDialogue(dialogueNode.sentences[i]); 
            yield return StartCoroutine(_currentDialogueCoroutine);
            _currentDialogueCoroutine = null;

            if (i == dialogueNode.sentences.Length - 1)
                SetHintText("Close [E]");
            else
                SetHintText("Next [E]");

            while (!_nextDialogue && !(gameControls.Wrapper.Player.Interact.WasPerformedThisFrame() && !options.HasFlag(DialogueOptions.NO_INPUT)))
            {
                yield return null;
            }

            _nextDialogue = false;
        }
        CleanupDialogueBox();
    }
    
    private IEnumerator AnimateDialogue(Dialogue dialogue)
    {
        dialogueSpeaker.text = dialogue.Speaker;
        dialogueBody.text = "";
        _skipScrawl = false;
        for (int i = 0; i < dialogue.Body.Length; i++)
        {
            if (_skipScrawl)
            {
                _skipScrawl = false;
                break;
            }
            dialogueBody.text += dialogue.Body[i];
            if (i % charactersPerSFX == 0)
                typewriterSFX.Play();
            if (".,!".Contains(dialogue.Body[i]))
                yield return new WaitForSeconds(secondsPerCharacter * 20);
            else
                yield return new WaitForSeconds(secondsPerCharacter);
        }
        
        dialogueBody.text = dialogue.Body; 
        yield return new WaitForSeconds(0.1f);
    }
}
