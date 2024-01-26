using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float sens = 400;
    public GameObject player;
    
    // Used for vertical viewing limits
    private float _minAngle = -45.0f;
    private float _maxAngle = 45.0f;
    private float _currentRotationX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse movement rotate along X axis, look up and down, limited range of motion
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        _currentRotationX -= mouseY;
        _currentRotationX = Mathf.Clamp(_currentRotationX, _minAngle, _maxAngle);
        transform.localRotation = Quaternion.Euler(_currentRotationX, 0, 0);

        // Mouse movement rotate along Y axis, look left and right, unlimited range of motion
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        
        // Should affect the player object, which will in turn rotate the camera since it is attached
        player.transform.Rotate(0.0f, mouseX, 0.0f, Space.World);
    }
}