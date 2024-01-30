using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Interactor : MonoBehaviour
{
    public float InteractableDistance = 2.0f;
    public InteractGUIController GUIController;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var layerMask = LayerMask.GetMask("Interactable");
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * InteractableDistance, Color.red);
        GUIController.Hide();
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, InteractableDistance, layerMask))
        {
            var interactable = hit.transform.GetComponent<IInteractable>();
            if (interactable.CanInteract())
            {
                GUIController.Show(interactable.DisplayText);
                hit.transform.GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.white);
                if (Input.GetKeyDown("e"))
                    interactable.Interact();
            }
        }
    }
}
