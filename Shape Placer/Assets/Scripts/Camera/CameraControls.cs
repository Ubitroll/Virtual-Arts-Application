using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    // Variables
    public bool _buildModeActive = false;

    [SerializeField]
    private float _mouseSensitivity = 3.0f;

    private float _rotationX;
    private float _rotationY;

    [SerializeField]
    private Transform _rotatePoint;

    [SerializeField]
    private float _distanceFromPoint = 9.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    // Called at the start of the program
    private void Start()
    {
        // Sets cursor to be not visible since game doesn't begin in build mode
        Cursor.visible = false;
        // Makes the cursor only able to move within the confines of the game window.
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        // If build mode is not engaged then allow camera to rotate
        if (_buildModeActive == false)
        {
            // Take in mouse inputs 
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            // Apply mouse movements to rotation
            _rotationY += mouseX;
            _rotationX += mouseY;

            //Clamp rotations so they can't over rotate on the x axis
            _rotationX = Mathf.Clamp(_rotationX, 0, 50);

            // Apply rotation with smoothening for neat camera control
            Vector3 nextRotation = new Vector3(_rotationX, _rotationY);
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;

            //Update camera transform so it always stays the same distance from rotation point
            transform.position = _rotatePoint.position - transform.forward * _distanceFromPoint;
        }
        
        // If B is pressed activate build mode to stop mouse moving the camera
        if (Input.GetKeyDown(KeyCode.B) == true)
        {
            // If build mode is already active then deactivate it else activate build mode
            // Set cursor visibility so it can be used for build mode
            if (_buildModeActive == true)
            {
                _buildModeActive = false;
                Cursor.visible = false;
            }
            else 
            { 
                _buildModeActive = true;
                Cursor.visible = true;
            }
        }    
    }
}