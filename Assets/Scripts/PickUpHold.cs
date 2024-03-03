using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHoldScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public HoldableItemScriptableObject defaultPickupId;
    [Header("Listening To")]
    public GameEvent dropHeldEvent;

    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private int LayerNumber; //layer index
    public static HoldableItemScriptableObject heldItemIdentifier;

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
        heldItemIdentifier = null;
    }

    private void OnEnable()
    {
        dropHeldEvent.AddListener(DropObject);
    }

    private void OnDisable()
    {
        dropHeldEvent.RemoveListener(DropObject);
    }


    public bool IsHolding()
    {
        return heldObj != null;
    }
    
    public void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            heldObj.transform.localRotation = Quaternion.identity;
            heldObj.transform.localPosition = Vector3.zero;
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
            //make sure object doesnt collide with player, it can cause weird bugs
            PickupableObject pickupId;
            if (pickUpObj.TryGetComponent(out pickupId))
            {
                heldItemIdentifier = pickupId.PickupIdentifier();
            }
            else
            {
                heldItemIdentifier = defaultPickupId;
            }
        }
    }

    public void DropObject()
    {
        StopClipping(); //prevents object from clipping through walls
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
        heldItemIdentifier = null;
    }

    void StopClipping() //function only called when dropping
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
