using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectSpawner : MonoBehaviour
{
    public GameObject testObject;
    public GameEvent spawnCubeEvent;

    public void OnEnable()
    {
        spawnCubeEvent?.AddListener(SpawnCube);
    }
    
    public void OnDisable()
    {
        spawnCubeEvent?.RemoveListener(SpawnCube);
    }

    private void SpawnCube()
    {
        Instantiate(testObject, transform.position, Quaternion.identity);
    }
}
