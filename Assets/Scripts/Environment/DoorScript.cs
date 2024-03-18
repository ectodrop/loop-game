using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float timeToCompleteSeconds;
    public float moveDistance;
    
    public void OpenDoor()
    {
        StartCoroutine(MoveRoutine(transform.position + new Vector3(0, moveDistance, 0)));
    }

    public void CloseDoor()
    {
        StartCoroutine(MoveRoutine(transform.position - new Vector3(0, moveDistance, 0)));
    }

    private IEnumerator MoveRoutine(Vector3 target)
    {
        float t = 0;
        Vector3 start = transform.position;
        while (t <= timeToCompleteSeconds)
        {
            transform.position = Vector3.Lerp(start, target, t/timeToCompleteSeconds);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }
}
