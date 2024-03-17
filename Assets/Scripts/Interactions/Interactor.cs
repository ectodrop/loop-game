using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public GameControls gameControls;
    public float InteractableDistance = 2.0f;
    public GameEventString onHoverReadableObject;
    public SharedBool timeStoppedFlag;
    public PickUpHoldScript pickupScript;

    [Header("Listening To")] public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;

    [Header("Triggers")]
    public GameEventString setWarningTextEvent;
    
    public GameEvent flashWarningTextEvent;
    public GameEvent pickedUp;
    
    private Camera mainCamera;
    private IRayHoverable curRayHoverableObj;
    private GameObject curHoverObject;
    private int rayLayerMask;

    private string curHoverString = "";
    private bool _timeStopAbilityActive;

    private ILabel[] curLabels;

    private RaycastHit interactHit;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rayLayerMask = LayerMask.GetMask("Interactable") | LayerMask.GetMask("Default");

    }

    private void OnEnable()
    {
        gameControls.Wrapper.Player.Interact.performed += HandleInteract;
        timeStopStartEvent.AddListener(HandleTimeStopStart);
        timeStopEndEvent.AddListener(HandleTimeStopEnd);
    }

    private void OnDisable()
    {
        gameControls.Wrapper.Player.Interact.performed -= HandleInteract;
        timeStopStartEvent.RemoveListener(HandleTimeStopStart);
        timeStopEndEvent.RemoveListener(HandleTimeStopEnd);
    }

    public void HandleTimeStopStart() {
        _timeStopAbilityActive = true;
    }

    public void HandleTimeStopEnd() {
        _timeStopAbilityActive = false;
    }
    
// Update is called once per frame
    void Update()
    {
        var prevHoverObject = curHoverObject;
        var prevHoverString = curHoverString;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out interactHit, InteractableDistance, rayLayerMask))
        {
            curHoverObject = interactHit.transform.gameObject;
            curLabels = curHoverObject.GetComponents<ILabel>();
        }
        else
        {
            curHoverObject = null;
            curLabels = null;
        }

        curHoverString = "";
        if (curLabels != null)
        {
            for (int i = 0; i < curLabels.Length; i++)
            {
                if (curLabels[i].GetLabel() != "")
                    curHoverString += curLabels[i].GetLabel() + "\n";
            }
        }

        // if (!string.IsNullOrWhiteSpace(curHoverString) && _timeStopAbilityActive)
        //     curHoverString = "Start time to interact (R)";
            
        
        // check if the current interactable we are hovering on has changed, call the corresponding interface methods if yes
        if (curHoverObject != prevHoverObject)
        {
            // call the enter exit methods if they exist
            if (curHoverObject != null && curHoverObject.TryGetComponent(out IRayHoverable curRayHoverableObj))
                curRayHoverableObj?.OnHoverEnter();
            
            if (prevHoverObject != null && prevHoverObject.TryGetComponent(out IRayHoverable prevRayHoverableObj))
                prevRayHoverableObj?.OnHoverExit();
        }
        
        if (curHoverString != prevHoverString)
            onHoverReadableObject.TriggerEvent(curHoverString);
    }

    private void HandleInteract(InputAction.CallbackContext _)
    {
        if (curHoverObject != null && !pickupScript.IsHolding())
        {
            if (curHoverObject.CompareTag("canPickUp"))
            {
                pickupScript.PickUpObject(curHoverObject);
                pickedUp.TriggerEvent();
            }
        }
        else if (curHoverObject == null && pickupScript.IsHolding())
        {
            pickupScript.DropObject();
        }
        
        if (curHoverObject != null &&
            curHoverObject.TryGetComponent<IInteractable>(out IInteractable interactable) && interactable.CanInteract())
        {
            if (!timeStoppedFlag.GetValue())
            {
                interactable.Interact();
            }
            else
            {
                setWarningTextEvent.TriggerEvent("Resume time to Interact (R)");
                flashWarningTextEvent.TriggerEvent();
            }
        }
    }
}
