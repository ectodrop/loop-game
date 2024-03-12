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

    private void OnDisable()
    {
        gameControls.Wrapper.Player.Move.performed -= HandleMove;
        gameControls.Wrapper.Player.Look.performed -= HandleLook;
        gameControls.Wrapper.Player.Sprint.performed -= HandleSprinted;
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
            player.TeleportTo(transform.position);
            StartCoroutine(FreezeMovement());
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
