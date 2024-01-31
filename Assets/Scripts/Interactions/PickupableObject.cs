using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableObject : MonoBehaviour
{
    [SerializeField] private HoldableItemScriptableObject identifier;
    public HoldableItemScriptableObject PickupIdentifier()
    {
        return identifier;
    }
}
