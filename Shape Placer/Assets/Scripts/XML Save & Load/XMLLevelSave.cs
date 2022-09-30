using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;
using System.IO;

public class XMLLevelSave : MonoBehaviour
{
    // Variables
    BuildingManager _buildingManager;
    SelectionManager _selectionManager;
    public GameObject _levelUI;
    public TMP_InputField _inputField;
    public TMP_Text _fileListText;
    private bool _isLevelUIActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // Reference the building manager gameobject so we can use information from it
        _buildingManager = GameObject.Find("Building Manager").GetComponent<BuildingManager>();

        // Reference the selection manager so we can use infromation from it
        _selectionManager = GameObject.Find("Select Manager").GetComponent<SelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // If player presses F1 then bring up level save UI
        if(Input.GetKeyDown(KeyCode.F1))
        {
            // If not already active then activate
            if(!_isLevelUIActive)
            {
                _isLevelUIActive = true;
                _levelUI.SetActive(true);
            }    
            else
            {
                _isLevelUIActive = false;
                _levelUI.SetActive(false);
            }
            
        }
    }

    // Method to check if an XML file exists at the supplied location
    public bool CheckIfFileExists(string _filename)
    {
        // If the file exists return true
        if (System.IO.File.Exists(_filename + ".xml"))
        {
            return true;
        }
        // Else return false
        else
        {
            return false;
        }
    }

    // Method to find all file names
    public void FindFiles()
    {
        // Create array of files with filetype .xml
        string[] _files = Directory.GetFiles(".", "*.xml");

        // Empty text box
        _fileListText.text = "";

        // loop through all files present and add them to the text box
        for (int i = 0; i < _files.Length; i++)
        {
            _fileListText.text = _fileListText.text + " " + Path.GetFileNameWithoutExtension(_files[i]);
        }
    }

    // Method to Delete Files
    public void DeleteFile()
    {
        string _filename = _inputField.textComponent.text;

        // Delete file
        File.Delete(_filename + ".xml");
    }

    // Write objects to XML file
    public void SaveObjectsToXMLFile()
    {
        // find all objects and add them to array
        GameObject[] _objects = GameObject.FindGameObjectsWithTag("Object");
        string _filename = _inputField.textComponent.text;

        // Set up writer
        XmlWriterSettings _writerSettings = new XmlWriterSettings();
        _writerSettings.Indent = true;

        // Creating a writing instance
        XmlWriter _xmlWriter = XmlWriter.Create(_filename + ".xml", _writerSettings);

        // Write beginning of document
        _xmlWriter.WriteStartDocument();

        // Create root element
        _xmlWriter.WriteStartElement("Objects");

        // Save Object Positions, Colour and rotation
        for(int i = 0; i< _objects.Length; i++)
        {
            // Create a single object element
            _xmlWriter.WriteStartElement("Object");

            // Create attributes to store information
            _xmlWriter.WriteAttributeString("posX", _objects[i].gameObject.transform.position.x.ToString());
            _xmlWriter.WriteAttributeString("posY", _objects[i].gameObject.transform.position.y.ToString());
            _xmlWriter.WriteAttributeString("posZ", _objects[i].gameObject.transform.position.z.ToString());
            _xmlWriter.WriteAttributeString("rotX", _objects[i].gameObject.transform.rotation.x.ToString());
            _xmlWriter.WriteAttributeString("rotY", _objects[i].gameObject.transform.rotation.y.ToString());
            _xmlWriter.WriteAttributeString("rotZ", _objects[i].gameObject.transform.rotation.z.ToString());
            _xmlWriter.WriteAttributeString("Material", _buildingManager.GetMaterial(_objects[i]).ToString());
            _xmlWriter.WriteAttributeString("Shape", _buildingManager.GetShape(_objects[i]).ToString());

            // End the object element
            _xmlWriter.WriteEndElement();
        }

        // End the root element
        _xmlWriter.WriteEndElement();

        // Write end of the document
        _xmlWriter.WriteEndDocument();

        // Close to save
        _xmlWriter.Close();
    }

    // Read Objects from XML
    public void LoadObjectsFromXMLFile()
    {
        // Bring in file name
        string _filename = _inputField.textComponent.text;

        if(CheckIfFileExists(_filename))
        {
            // Creat an XML Reader with the file wanted
            XmlReader _xmlReader = XmlReader.Create(_filename + ".xml");

            // Destroy all objects with tag
            GameObject[] _objects = GameObject.FindGameObjectsWithTag("Object");

            // destroy all game objects in field currently
            for (int i = 0; i < _objects.Length; i++)
            {
                Destroy(_objects[i]);
            }

            // Deselect any selected item
            _selectionManager.Deselect();

            // Iterate through and read every line in the xml file
            while (_xmlReader.Read())
            {
                // Debug
                Debug.Log("Reading Objects");

                // Gather details of the saved object
                if (_xmlReader.IsStartElement("Object"))
                {
                    float _posX = float.Parse(_xmlReader["posX"]);
                    float _posY = float.Parse(_xmlReader["posY"]);
                    float _posZ = float.Parse(_xmlReader["posZ"]);
                    float _rotX = float.Parse(_xmlReader["rotX"]);
                    float _rotY = float.Parse(_xmlReader["rotY"]);
                    float _rotZ = float.Parse(_xmlReader["rotZ"]);
                    int _material = int.Parse(_xmlReader["Material"]);
                    int _shape = int.Parse(_xmlReader["Shape"]);

                    Quaternion _rotation = Quaternion.Euler(_rotX, _rotY, _rotZ);
                    Vector3 _position = new Vector3(_posX, _posY, _posZ);

                    // Instantiate Game Objects
                    _buildingManager.LoadObject(_shape, _material, _position, _rotation);
                }
            }
        }        
    }
}