using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectSpawner : MonoBehaviour
{
    public GameObject testObject;
    public ScheduleEventScriptableObject testEvent;

    public void OnEnable()
    {
        testEvent.onScheduleTrigger.AddListener(SpawnCube);
    }
    
    public void OnDisable()
    {
        testEvent.onScheduleTrigger.RemoveListener(SpawnCube);
    }

    private void SpawnCube()
    {
        Instantiate(testObject, transform.position, Quaternion.identity);
    }
}
