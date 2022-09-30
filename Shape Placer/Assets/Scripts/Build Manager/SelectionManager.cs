using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    // Variables
    public GameObject _selectedObject;
    public TextMeshProUGUI _objectNameText;

    private BuildingManager _buildingManager;
    public GameObject _selectedUI;
    public TMP_Dropdown _colourDropdown;

    // Start is called before the first frame update
    void Start()
    {
        // Reference the building manager gameobject so we can use information from it
        _buildingManager = GameObject.Find("Building Manager").GetComponent<BuildingManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
        // If the user left clicks
        if (Input.GetMouseButtonDown(0))
        {
            // Send a raycast to see if an object was hit
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;
            
            // If the raycast hits an object
            if (Physics.Raycast(_ray, out _hit, 1000))
            {
                // If the hit object has tag object
                if (_hit.collider.gameObject.CompareTag("Object"))
                {
                    // Slect the game object
                    Select(_hit.collider.gameObject);
                }
            }   
        }

        // If the player Right clicks then deselect
        if (Input.GetMouseButtonDown(1) && _selectedObject != null)
        {
            Deselect();
        }
    }

    // Method to select gameobject
    private void Select(GameObject _object)
    {
        // Don't run through everything if object has already been selected
        if (_object == _selectedObject)
        {
            return;
        }

        // If we already have an object selected
        if (_selectedObject != null)
        {
            // Deselect the object
            Deselect();
        }        

        // Get the objects outline 
        Outline _outline = _object.GetComponent<Outline>();
        // If it doesn't then add one
        if (_outline == null)
        {
            _object.AddComponent<Outline>();
        }    
        // If it does the enable it
        else
        {
            _outline.enabled = true;
        }

        // Get Selected Object Name
        _objectNameText.text = _object.name;

        // Set the selected object 
        _selectedObject = _object;

        // Turn on UI
        _selectedUI.SetActive(true);
    }

    // Method to deselect Object
    public void Deselect()
    {
        // If selected isn't null
        if (_selectedObject != null)
        {
            // Turn off UI
            _selectedUI.SetActive(false);

            // Disable the outline
            _selectedObject.GetComponent<Outline>().enabled = false;

            // Set select to null
            _selectedObject = null;

            // Set dropdown back to default
            _colourDropdown.value = 0;
        }        
    }

    // Method that allows selected object to be moved
    public void Move()
    {
        // Set what the objects current material is
        _buildingManager._currentMaterial = _selectedObject.GetComponent<MeshRenderer>().material;
        // Sets the selected object to be the pending object
        _buildingManager._pendingObject = _selectedObject;
    }
    
    // Method that allows selected object to be destroyed
    public void Delete()
    {
        // Set the object that has to be destroyed
        GameObject _objectToDestroy = _selectedObject;

        // Deselect the object
        Deselect();

        // Destroy it 
        Destroy(_objectToDestroy);
    }    

    // Method to handle selected colour drop down
    public void HandleColourDropdown(int _val)
    {
        // If an object has been selected
        if (_selectedObject != null)
        {
            // If value is 0 then object is to be Blue
            if (_val == 0)
            {
                _selectedObject.GetComponent<MeshRenderer>().material = _buildingManager._placedMaterials[0];
            }
            // Else if value is 1 then object is to be Green
            else if (_val == 1)
            {
                _selectedObject.GetComponent<MeshRenderer>().material = _buildingManager._placedMaterials[1];
            }
            // Else if value is 2 then object is to be Pink
            else if (_val == 2)
            {
                _selectedObject.GetComponent<MeshRenderer>().material = _buildingManager._placedMaterials[2];
            }
            // Else if value is 3 then object is to be Red
            else if (_val == 3)
            {
                _selectedObject.GetComponent<MeshRenderer>().material = _buildingManager._placedMaterials[3];
            }
            // Else if value is 4 then object is to be Yellow
            else if (_val == 4)
            {
                _selectedObject.GetComponent<MeshRenderer>().material = _buildingManager._placedMaterials[4];
            }
        }        
    }
}
