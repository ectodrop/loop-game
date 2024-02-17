using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Interactor : MonoBehaviour
{
    public float InteractableDistance = 2.0f;
    public GameEventString onHoverReadableObject;

    private Camera mainCamera;
    private IRayHoverable curRayHoverableObj;
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
        var prevRayHoverableObj = curRayHoverableObj;
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, InteractableDistance, rayLayerMask))
        {
            curRayHoverableObj = hit.transform.GetComponent<IRayHoverable>();

            if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable) && interactable.CanInteract())
            {
                if (Input.GetKeyDown("e"))
                    interactable.Interact();
            }
        }
        else
        {
            curRayHoverableObj = null;
        }

        // check if the current interactable we are hovering on has changed, call the corresponding interface methods if yes
        if (curRayHoverableObj != prevRayHoverableObj)
        {
            if (curRayHoverableObj == null)
            {
                onHoverReadableObject.TriggerEvent("");
            }
            else
            {
                onHoverReadableObject.TriggerEvent("Interact (E)");
            }
            prevRayHoverableObj?.OnHoverExit();
            curRayHoverableObj?.OnHoverEnter();
        }
    }
}
