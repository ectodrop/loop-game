using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleOnEnter : MonoBehaviour
{
    public GameEventVector3 PlayerEnterPuddleEvent;
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("collision");
        if (hit.collider.tag == "Player")
        {
            Debug.Log("EnterPuddle");
            PlayerEnterPuddleEvent.TriggerEvent(new Vector3(1,1,1));  
        }
    }
}
