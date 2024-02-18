using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelScript : MonoBehaviour, ILabel
{
    public string Label;

    public string GetLabel()
    {
        return Label;
    }
}
