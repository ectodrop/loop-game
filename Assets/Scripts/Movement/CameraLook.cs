using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public GameControls gameControls;
    public float sens = 400;
    public GameObject player;
    [Header("Listening To")]
    public GameEventVector3 lookAtEvent;
    
    // Used for vertical viewing limits
    private float _minAngle = -45.0f;
    private float _maxAngle = 70.0f;
    private float _currentRotationX = 0.0f;
    private bool _fixlook = false;
    private float _lookAtSpeed = 3.0f;

    private IEnumerator _lookAtRoutine;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        lookAtEvent.AddListener(LookAt);
    }

    private void LookAt(Vector3 targetPos)
    {
        _lookAtRoutine = LookAtRoutine(targetPos);
        StartCoroutine(_lookAtRoutine);
    }

    private float GetShortestRotationTarget(float source, float target)
    {
        if (Mathf.Abs(target - source) < Mathf.Abs(source - (target - 360)))
        {
            return target;
        }
        else
        {
            return target - 360;
        }
    }

    private IEnumerator LookAtRoutine(Vector3 targetPos)
    {
        _fixlook = true;
        float t = 0.0f;
        
        Vector3 targetRotation = Quaternion.LookRotation(targetPos - this.transform.position).eulerAngles;
        Vector3 sourceRotation = new Vector3(transform.localRotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, 0);

        float targetXRotation = GetShortestRotationTarget(sourceRotation.x, targetRotation.x);
        float targetYRotation = GetShortestRotationTarget(sourceRotation.y, targetRotation.y);
        while (t < 1.0f)
        {
            this.transform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(sourceRotation.x, 0, 0), new Vector3(targetXRotation, 0, 0), t));
            player.transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, sourceRotation.y, 0), new Vector3(0, targetYRotation, 0), t));
            t += Time.deltaTime * _lookAtSpeed;
            yield return null;
        }

        _currentRotationX = targetXRotation;
        _fixlook = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_fixlook && gameControls.Wrapper.Player.Look.enabled) 
        {
            // Mouse movement rotate along X axis, look up and down, limited range of motion
            Vector2 look = Time.deltaTime * sens * gameControls.Wrapper.Player.Look.ReadValue<Vector2>();
            float mouseY = look.y;
            float mouseX = look.x;
            _currentRotationX -= mouseY;
            _currentRotationX = Mathf.Clamp(_currentRotationX, _minAngle, _maxAngle);
            transform.localRotation = Quaternion.Euler(_currentRotationX, 0, 0);

            // Should affect the player object, which will in turn rotate the camera since it is attached
            player.transform.Rotate(0.0f, mouseX, 0.0f, Space.World); 
        }
    }
}