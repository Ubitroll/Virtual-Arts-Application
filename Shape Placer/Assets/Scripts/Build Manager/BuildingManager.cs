using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    // Variables
    public GameObject[] _listOfObjects;
    public GameObject _cameraObject;
    public GameObject _pendingObject;
    public GameObject _loadedObject;

    [SerializeField] private Material[] _canPlaceMaterials;
    [SerializeField] public Material[] _placedMaterials;
    public Material _currentMaterial;

    private Vector3 _aimedPosition;
    private RaycastHit _hit;

    [SerializeField] private LayerMask _layerMask;

    public float _rotateAmount;
    public float _gridSize;
    bool _gridOn = true;
    public bool _canPlace;
    [SerializeField] private Toggle _gridToggle;

    // Update is called once per frame
    void Update()
    {
        // If in build mode
        if (_cameraObject.GetComponent<CameraControls>()._buildModeActive == true)
        {
            // Check if there is a pending object
            if (_pendingObject != null)
            {
                // If grid snap is on
                if (_gridOn)
                {
                    // Use all the positions of the moise to find the nearest snap in the X, Y and Z planes
                    _pendingObject.transform.position = new Vector3(
                        RoundToNearestGrid(_aimedPosition.x),
                        RoundToNearestGrid(_aimedPosition.y),
                        RoundToNearestGrid(_aimedPosition.z)
                        );
                }
                else
                {
                    // Moves the pending object around with where the cursor is pointing.
                    // Uses an ofset so object isn't entirely in the ground.
                    _pendingObject.transform.position = _aimedPosition + new Vector3(0, 0.5f, 0);
                }

                

                // If the player clicks the left mouse button
                if (Input.GetMouseButtonDown(0) && _canPlace)
                {
                    //place the object
                    PlaceObject();
                }

                // If player presses E rotate clockwise
                if (Input.GetKeyDown(KeyCode.E))
                {
                    RotateObject(_rotateAmount);
                }
                // Else if player pressedQ then rotate anticlockwise
                else if(Input.GetKeyDown(KeyCode.Q))
                {
                    RotateObject(-_rotateAmount);
                }

                // If player presses escape then cancel pending object
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    DestroyObject();
                }

                // Set materials to update
                UpdateMaterials();
            }
        }
    }

    // FixedUpdate updates at the end of a frame and is more efficient for physics updates
    private void FixedUpdate()
    {
        // If in build mode
        if (_cameraObject.GetComponent<CameraControls>()._buildModeActive == true)
        {
            // Make a raycast from camera to where curser is pointing
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // If the raycast hits something and it is within the correct layer
            // Set the point as the aimed position
            if (Physics.Raycast(_ray, out _hit, 1000, _layerMask))
            {
                _aimedPosition = _hit.point;
                // Debug to ensure the aimed position is being updated properly.
                //Debug.Log(_aimedPosition);
            }
        }
    }

    // Method to update materials to give visual representation of when you can and can't place
    void UpdateMaterials()
    {
        // If the pending object exists
        if(_pendingObject != null)
        {
            // If you can place the object then set to first material
            if (_canPlace)
            {
                _pendingObject.GetComponent<MeshRenderer>().material = _canPlaceMaterials[0];
            }
            // Else show other material when can't place
            else
            {
                _pendingObject.GetComponent<MeshRenderer>().material = _canPlaceMaterials[1];
            }
        }
    }

    // Method to allow you to pick an object
    public void SelectObject(int _index)
    {
        // Instantiates a copy of the desired prefab
        _pendingObject = Instantiate(_listOfObjects[_index], _aimedPosition, transform.rotation);
    }

    // Method to load object
    public void LoadObject(int _shapeIndex, int _materialIndex, Vector3 _position, Quaternion _rotation)
    {
        // Instantiate the loaded oject with desired prefab
        _loadedObject = Instantiate(_listOfObjects[_shapeIndex], _position, _rotation);
        _loadedObject.GetComponent<MeshRenderer>().material = _placedMaterials[_materialIndex];
    }

    // Method to get find out what object type it is
    public int GetShape(GameObject _selectedObject)
    {
        // If the object is a capsule
        if(_selectedObject.name.ToString() == "Capsule(Clone)")
        {
            return 0;
        }
        // If the object is a capsule
        if (_selectedObject.name.ToString() == "Cube(Clone)")
        {
            return 1;
        }
        // If the object is a capsule
        if (_selectedObject.name.ToString() == "Cylinder(Clone)")
        {
            return 2;
        }
        // If the object is a capsule
        if (_selectedObject.name.ToString() == "Sphere(Clone)")
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }

    // Method to get material type
    public int GetMaterial(GameObject _selectedObject)
    {
        // If value is 0 then object is to be Blue
        if (_selectedObject.GetComponent<MeshRenderer>().material.ToString() == "Blue (Instance) (UnityEngine.Material)")
        {
            return 0;
        }
        // Else if value is 1 then object is to be Green
        else if (_selectedObject.GetComponent<MeshRenderer>().material.ToString() == "Green (Instance) (UnityEngine.Material)")
        {
            return 1;
        }
        // Else if value is 2 then object is to be Pink
        else if (_selectedObject.GetComponent<MeshRenderer>().material.ToString() == "Pink (Instance) (UnityEngine.Material)")
        {
            return 2;
        }
        // Else if value is 3 then object is to be Red
        else if (_selectedObject.GetComponent<MeshRenderer>().material.ToString() == "Red (Instance) (UnityEngine.Material)")
        {
            return 3;
        }
        // Else if value is 4 then object is to be Yellow
        else if (_selectedObject.GetComponent<MeshRenderer>().material.ToString() == "Yellow (Instance) (UnityEngine.Material)")
        {
            return 4;
        }
        else
        {
            
            return 0;
            
        }
        
    }

    // Method to place objects
    public void PlaceObject()
    {
        // If the object already had a colour and was just moved
        if (_currentMaterial != null)
        {
            // Set the material to what it was before it moved
            _pendingObject.GetComponent<MeshRenderer>().material = _currentMaterial;
            _currentMaterial = null;
        }
        // else if its a new object then
        else
        {
            _pendingObject.GetComponent<MeshRenderer>().material = _placedMaterials[0];
        }
        
        _pendingObject = null;
    }

    // Method to destroy objects
    public void DestroyObject()
    {
        Destroy(_pendingObject);
    }

    // Method to rotate object
    public void RotateObject(float _rotate)
    {
        _pendingObject.transform.Rotate(Vector3.up, _rotate);
    }

    // Method to toggle grid snapping for building
    public void ToggleGrid()
    {
        // If toggle is on turn on grid
        if (_gridToggle.isOn)
        {
            _gridOn = true;
        }
        else
        {
            _gridOn = false;
        }
    }

    // Method that rounds the aimed position to the nearest grid position
    private float RoundToNearestGrid(float _position)
    {
        // Works out which posiition the grid is closest to and returns it
        float _xDifference = _position % _gridSize;
        _position -= _xDifference;

        if (_xDifference > (_gridSize/2))
        {
            _position += _gridSize;
        }
        return _position;
    }

}