using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private void GetShortestRotationTarget(float source, float target, out float bestSource, out float bestTarget)
    {
        source = mod(source, 360);
        target = mod(target, 360);

        float invertedSource = source - 360;
        float invertedTarget = target - 360;
        // figure out if we should invert target or source
        if (target < source)
        {
            bestTarget = target;
            if (source - target < target - invertedSource)
            {
                bestSource = source;
                return;
            }
            else
            {
                bestSource = invertedSource;
                return;
            }
        }
        else
        {
            bestSource = source;
            if (target - source < source - invertedTarget)
            {
                bestTarget = target;
                return;
            }
            else
            {
                bestTarget = invertedTarget;
                return;
            }
        }


    }

    private IEnumerator LookAtRoutine(Vector3 targetPos)
    {
        _fixlook = true;
        float t = 0.0f;
        
        Vector3 targetRotation = Quaternion.LookRotation(targetPos - this.transform.position).eulerAngles;
        Vector3 sourceRotation = new Vector3(
            mod(transform.localRotation.eulerAngles.x,360), 
            mod(player.transform.rotation.eulerAngles.y, 360), 
            0);

        float sourceXRotation, targetXRotation;
        float sourceYRotation, targetYRotation;
        GetShortestRotationTarget(sourceRotation.x, targetRotation.x, out sourceXRotation, out targetXRotation);
        GetShortestRotationTarget(sourceRotation.y, targetRotation.y, out sourceYRotation, out targetYRotation);
        while (t < 1.0f)
        {
            this.transform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(sourceXRotation, 0, 0), new Vector3(targetXRotation, 0, 0), t));
            player.transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, sourceYRotation, 0), new Vector3(0, targetYRotation, 0), t));
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
    // need a custom mod here because modulus operator in c# is not a real modulus operator
    // this mod works with negative numbers
    private float mod(float x, float m)
    {
        float r = x % m;
        return r < 0 ? r+m : r;
    }
}