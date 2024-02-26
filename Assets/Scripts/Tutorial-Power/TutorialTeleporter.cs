using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTeleporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Exiting Power Tutorial.");
        
        // Return to Main Menu
    }
}
