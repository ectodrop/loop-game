using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleOnEnter : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.SendMessage("BounceBack");
        }
    }
}
