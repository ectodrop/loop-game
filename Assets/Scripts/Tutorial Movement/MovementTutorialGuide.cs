using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

public class MovementTutorialGuide : MonoBehaviour
{
    public CharacterMovement player;
    public Transform phone;
    public GameObject lookObject;
    public GameObject movementObject;
    public GameObject sprintObject;
    public GameObject jarvisObject;
    public GameObject timeDoor;
    public GameObject powerDoor;
    public GameControls gameControls;
    public DialogueNode dialogueJarvisExplanation;

    [Header("Triggers")] public GameEventVector3 lookAtEvent;

    private DialogueController _dialogueController;
    private bool _hasLooked;
    private bool _hasMoved;
    private bool _hasSprinted;

    private void Start()
    {
        _dialogueController = FindObjectOfType<DialogueController>();
    }

    private void OnEnable()
    {
        lookObject.SetActive(true);
        gameControls.Wrapper.Player.Move.performed += HandleMove;
        gameControls.Wrapper.Player.Look.performed += HandleLook;
        gameControls.Wrapper.Player.Sprint.performed += HandleSprinted;
    }

    private void OnDisable()
    {
        gameControls.Wrapper.Player.Move.performed -= HandleMove;
        gameControls.Wrapper.Player.Look.performed -= HandleLook;
        gameControls.Wrapper.Player.Sprint.performed -= HandleSprinted;
    }

    private void HandleLook(InputAction.CallbackContext _)
    {
        if (!_hasLooked)
            lookAtEvent.TriggerEvent(phone.position);
        lookObject.SetActive(false);
        _hasLooked = true;
        if (!_hasMoved)
            movementObject.SetActive(true);
    }
    
    private void HandleMove(InputAction.CallbackContext _)
    {
        _hasMoved = true;
        movementObject.SetActive(false);
        if (!_hasSprinted)
            sprintObject.SetActive(true);
    }
    
    private void HandleSprinted(InputAction.CallbackContext _)
    {
        _hasSprinted = true;
        sprintObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, phone.position) < 5f)
        {
            player.TeleportTo(transform.position);
            StartCoroutine(FreezeMovement());
        }
    }

    public void StartJarvisFirstDialogue()
    {
        lookAtEvent.TriggerEvent(jarvisObject.transform.position);
        _dialogueController.StartDialogue(dialogueJarvisExplanation, finishedCallback: () => StartCoroutine(OpenDoor(timeDoor.transform)));
    }


    public void OpenPowerDoor()
    {
        StartCoroutine(OpenDoor(powerDoor.transform));
    }


    public IEnumerator OpenDoor(Transform door)
    {
        lookAtEvent.TriggerEvent(door.position);
        float y = 0;
        while (y < 5f)
        {
            door.position += new Vector3(0, Time.deltaTime, 0);
            y += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FreezeMovement()
    {
        gameControls.Wrapper.Player.Move.Disable();
        gameControls.Wrapper.Player.Look.Disable();
        yield return new WaitForSeconds(0.5f);
        gameControls.Wrapper.Player.Move.Enable();
        gameControls.Wrapper.Player.Look.Enable();
    }
}
