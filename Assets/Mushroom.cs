using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public GameEvent startGrow;
    public GameEvent stopGrow;
    private Rigidbody rb;
    public float GrowSpeed = 0.01f;
    private float growHeightLimit = 4.0f;
    private bool _growing = false;
    private void Start()
    {
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
