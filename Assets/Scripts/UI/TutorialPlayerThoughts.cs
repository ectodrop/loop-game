using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayerThoughts : MonoBehaviour
{
    public TextMeshProUGUI playerThoughts;
    private int _num_bounces = 0;

  
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("puddle"))
        {
            _num_bounces += 1;

            if (_num_bounces > 6)
            {
                playerThoughts.text = "The power generator needs to be turned off.";
            }
            else if (_num_bounces > 3)
            {
                playerThoughts.text = "Maybe the control panel will be helpful.";
            }
            else
            {
                playerThoughts.text = "Ouch. The water is electrified by the power generator.";
            }
        }
    }
}
