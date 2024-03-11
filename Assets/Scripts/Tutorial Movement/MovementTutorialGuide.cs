using System;
using System.Linq;
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
    public GameControls gameControls;


    private bool _hasLooked;
    private bool _hasMoved;
    private bool _hasSprinted;

    private void OnEnable()
    {
        lookObject.SetActive(true);
        gameControls.Wrapper.Player.Move.performed += HandleMove;
        gameControls.Wrapper.Player.Look.performed += HandleLook;
        gameControls.Wrapper.Player.Sprint.performed += HandleSprinted;
    }

    private void HandleLook(InputAction.CallbackContext _)
    {
        _hasLooked = true;
        lookObject.SetActive(false);
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
            Debug.Log(player.transform.position);
            Debug.Log(transform.position);
            player.TeleportTo(transform.position);
        }
    }
}
