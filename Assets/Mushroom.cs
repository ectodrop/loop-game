using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public Transform mushroom;
    public Transform head;
    public Rigidbody movingPlatform;
    public GameEvent startGrow;
    public GameEvent stopGrow;
    public float GrowSpeed = 0.01f;
    private float growHeightLimit = 4.0f;
    private bool _growing = false;
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
                mushroom.localScale += new Vector3(GrowSpeed, GrowSpeed, GrowSpeed);
                movingPlatform.transform.localScale += new Vector3(GrowSpeed, 0, GrowSpeed) * 2;
                movingPlatform.MovePosition(head.position);
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
