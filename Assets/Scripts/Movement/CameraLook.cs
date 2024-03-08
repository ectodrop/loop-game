using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public GameControls gameControls;
    public float sens = 400;
    public GameObject player;
    
    // Used for vertical viewing limits
    private float _minAngle = -45.0f;
    private float _maxAngle = 70.0f;
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