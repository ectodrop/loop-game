using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public GameEvent startGrow;
    public GameEvent stopGrow;
    private Rigidbody rb;
    public float GrowSpeed = 0.001f;
    private float growHeightLimit = 4.0f;
    private bool _growing = false;
    private Vector3 startPos;
    private Vector3 endPos;
    private void Start()
    {
        startPos = gameObject.transform.position;
        endPos = new Vector3(startPos.x, growHeightLimit, startPos.z);
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        startGrow.AddListener(Grow);
        stopGrow.AddListener(StopGrow);
    }
    private void OnDisable()
    {
        startGrow.RemoveListener(Grow);
        stopGrow.RemoveListener(StopGrow);
    }
    private void FixedUpdate()
    {
        if (_growing)
        {
            if (gameObject.transform.position.y >= growHeightLimit)
            {
                StopGrow();
            }
            else
            {
                //gameObject.transform.position = gameObject.transform.position + new Vector3(0, GrowSpeed, 0);
                rb.MovePosition(rb.position + new Vector3(0, GrowSpeed, 0));
            }
        }
    }
    void Grow()
    {
        _growing = true;
    }
    void StopGrow()
    {
        _growing = false;
    }
}
