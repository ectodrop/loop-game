using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPuzzle : MonoBehaviour
{
    public string Solution = "123";
    private string combination = "";
    public void Add(string character)
    {
        combination += character;
        if (combination.Length == Solution.Length)
        {
            if (combination == Solution)
                GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            else
                GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        }
    }
}
