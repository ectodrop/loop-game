using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persisting : MonoBehaviour
{
    private static Persisting instance;
    
    // add this component to any object you want to persist between loads
    // (ie. not reset after a time loop reset)
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
