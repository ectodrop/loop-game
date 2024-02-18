using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : MonoBehaviour, IInteractable, ILabel
{
    public GameObject fluidParent;
    public GameObject glassObject;
    public HoldableItemScriptableObject hammer;

    [Header("Listening To")]
    public GameEvent OnFluidDrain;

    private AudioSource glassBreakSound;
    private Collider generatorCollider;
    private bool drained;

    private void Start()
    {
        glassBreakSound = GetComponent<AudioSource>();
        generatorCollider = GetComponent<BoxCollider>();
    }

    private bool IsHoldingHammer()
    {
        return PickUpHoldScript.heldItemIdentifier == hammer;
    }
    public string GetLabel()
    {
        if (IsHoldingHammer() && drained)
        {
            return "Break Glass (E)";
        }
        return "Can't Break";
    }
    
    private void OnEnable()
    {
        OnFluidDrain.AddListener(DrainCoolant);
    }

    public void Interact()
    {
        if (drained && IsHoldingHammer())
        {
            glassObject.SetActive(false);
            generatorCollider.enabled = false;
            glassBreakSound.Play();
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    private void DrainCoolant()
    {
        drained = true;
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
