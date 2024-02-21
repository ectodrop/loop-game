using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Interactor : MonoBehaviour
{
    public float InteractableDistance = 2.0f;
    public GameEventString onHoverReadableObject;
    public SharedBool timeStoppedFlag;

    private Camera mainCamera;
    private IRayHoverable curRayHoverableObj;
    private GameObject curHoverObject;
    private int rayLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rayLayerMask = LayerMask.GetMask("Interactable") | LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        var prevHoverObject = curHoverObject;
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, InteractableDistance, rayLayerMask))
        {
            curHoverObject = hit.transform.gameObject;

            if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable) && interactable.CanInteract())
            {
                if (!timeStoppedFlag.GetValue() && Input.GetKeyDown("e"))
                    interactable.Interact();
            }
        }
        else
        {
            curHoverObject = null;
        }

        // check if the current interactable we are hovering on has changed, call the corresponding interface methods if yes
        if (curHoverObject != prevHoverObject)
        {
            // check for labels
            if (curHoverObject == null)
            {
                onHoverReadableObject.TriggerEvent("");
            }
            else
            {
                string text = "";
                foreach (var label in curHoverObject.GetComponents<ILabel>())
                {
                    text += label.GetLabel() + "\n";
                }

                if (timeStoppedFlag.GetValue())
                    text = "Resume time to interact";
                onHoverReadableObject.TriggerEvent(text);
            }

            // call the enter exit methods if they exist
            if (curHoverObject != null && curHoverObject.TryGetComponent(out IRayHoverable curRayHoverableObj))
                curRayHoverableObj?.OnHoverEnter();
            
            if (prevHoverObject != null && prevHoverObject.TryGetComponent(out IRayHoverable prevRayHoverableObj))
                prevRayHoverableObj?.OnHoverExit();
        }
    }
}
