using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : MonoBehaviour, IInteractable
{
    public GameObject fluidParent;

    public GameEvent OnFluidDrain;

    private void OnEnable()
    {
        OnFluidDrain.AddListener(DrainCoolant);
    }

    public string DisplayText { get => "Power Generator"; }
    public void Interact()
    {
        Debug.Log("Interacted");
        // StartCoroutine(DrainCoolantCoroutine());
    }

    public bool CanInteract()
    {
        return true;
    }

    private void DrainCoolant()
    {
        StartCoroutine(DrainCoolantCoroutine());
    }

    private IEnumerator DrainCoolantCoroutine()
    {
        while (fluidParent.transform.localScale.y > 0)
        {
            fluidParent.transform.localScale -= new Vector3(0, 0.05f, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
