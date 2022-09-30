using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPlacement : MonoBehaviour
{
    // Variables
    BuildingManager _buildingManager;
    
    // Start is called before the first frame update
    void Start()
    {
        // Reference the building manager gameobject so we can use information from it
        _buildingManager = GameObject.Find("Building Manager").GetComponent<BuildingManager>();
    }

    // Trigger to see if the placement overlaps another.
    private void OnTriggerEnter(Collider other)
    {
        // If it's colliding with a gameobject with the tag object
        if(other.gameObject.CompareTag("Object"))
        {
            // Tell the building manager it can't place it here
            _buildingManager._canPlace = false;
        }
    }

    // Trigger exit to let us know the object is placeable now.
    private void OnTriggerExit(Collider other)
    {
        // If it's exiting a collision with a gameobject with the tag object
        if (other.gameObject.CompareTag("Object"))
        {
            // Tell the building manager it can place it here
            _buildingManager._canPlace = true;
        }
    }
}
