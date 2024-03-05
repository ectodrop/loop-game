using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : MonoBehaviour
{
    public GameObject fluidParent;
    public GameObject bigFluid;

    [Header("Listening To")]
    public GameEvent OnFluidDrain;

    public bool drained;

    private void OnEnable()
    {
        OnFluidDrain.AddListener(DrainCoolant);
    }

    private void OnDisable()
    {
        OnFluidDrain.RemoveListener(DrainCoolant);
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
            bigFluid.transform.position -= new Vector3(0, 0.5f, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
