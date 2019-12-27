using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OBJModelCreator
{
    /// <summary>
    /// Instance of created model
    /// </summary>
    private OBJModel _model = null;
    public OBJModel Model
    {
        get
        {
            return _model;
        }

        set
        {
            _model = value;
        }
    }

    /// <summary>
    /// Name for created model
    /// </summary>
    private string _modelName = null;
    public string ModelName
    {
        get
        {
            return _modelName;
        }
    }

    /// <summary>
    /// OBJ File path
    /// </summary>
    private string _objFilePath = null;
    public string OBJFilePath
    {
        get
        {
            return _objFilePath;
        }
    }

    /// <summary>
    /// MTL File path
    /// </summary>
    private string _mtlFilePath = null;
    public string MTLFilePath
    {
        get
        {
            return _mtlFilePath;
        }
    }

    /// <summary>
    /// Is MTL File avaiable
    /// </summary>
    public bool MTLFileAvailable
    {
        get
        {
            return MTLFilePath != null;
        }
    }

    /// <summary>
    /// Method for checking obj file path
    /// </summary>
    /// <param name="objFilePath">
    /// File path to check
    /// </param>
    /// <returns>
    /// null - if path is ok
    /// string with message otherwise
    /// </returns>
    private static string CheckOBJFilePath(string objFilePath)
    {
        //Checking if file exists
        if (!File.Exists(objFilePath)) return string.Format("File not found: {0}", objFilePath);

        //Checking file extension
        var extension = Path.GetExtension(objFilePath);
        if (extension != ".obj") return string.Format("OBJ file has invalid extension: {0}", extension);

        //Returning null if path is ok
        return null;
    }

    /// <summary>
    /// Method for getting name based on obj file path
    /// </summary>
    /// <param name="objFilePath">
    /// OBJ file path
    /// </param>
    /// <returns>
    /// Name for model
    /// </returns>
    private static string GetModelName(string objFilePath)
    {
        return Path.GetFileNameWithoutExtension(objFilePath);
    }

    /// <summary>
    /// Method for getting mtl file path based on obj file path
    /// </summary>
    /// <param name="objFilePath">
    /// obj file path
    /// </param>
    /// <returns>
    /// mtl file path
    /// </returns>
    private static string GetMTLFilePath(string objFilePath)
    {
        return objFilePath.Replace(".obj", ".mtl");
    }

    /// <summary>
    /// Creator of OBJ models
    /// </summary>
    /// <param name="objFilePath">
    /// Path for obj file
    /// </param>
    public OBJModelCreator(string objFilePath)
    {
        //Checking obj file path
        var pathCheckResult = CheckOBJFilePath(objFilePath);
        if (pathCheckResult != null) throw new InvalidDataException(pathCheckResult);

        //Assigning obj file path
        this._objFilePath = objFilePath;

        //Assigning model name
        this._modelName = OBJModelCreator.GetModelName(objFilePath);

        //Assigning mtl file path if mtl file exists
        var mtlFilePath = OBJModelCreator.GetMTLFilePath(objFilePath);
        if (File.Exists(mtlFilePath)) this._mtlFilePath = mtlFilePath;
    }

    /// <summary>
    /// Creator of OBJ models - for testing loading prefabs directly, without initialization
    /// </summary>
    public OBJModelCreator()
    {
    }

    /// <summary>
    /// Method for creating new obj model
    /// </summary>
    /// <param name="parentGO">
    /// Parent game object for created model
    /// </param>
    /// <param name="modelPrefab">
    /// Optional parameter to load prefab instead of filePath
    /// </param>
    /// <returns>
    /// New obj model
    /// </returns>
    public OBJModel CreateModel(GameObject parentGO, GameObject modelPrefab = null)
    {
        GameObject modelGO = null;

        //Creating prefab of model from file if modelprefab does not exist
        if (modelPrefab == null)
        {
            modelGO = MTLFileAvailable ?
                new OBJLoader().Load(OBJFilePath, MTLFilePath) :
                new OBJLoader().Load(OBJFilePath);
        }
        else
        {
            modelGO = GameObject.Instantiate(modelPrefab);
        }

        modelGO.transform.SetParent(parentGO.transform);

        //Creating new OBJ Model script
        modelGO.AddComponent<OBJModel>();

        //getting and returning obj model script
        _model = modelGO.GetComponent<OBJModel>();
        Model.Init(this);

        return Model;
    }

    /// <summary>
    /// Method for creating OBJModelCreators for all files in directory
    /// </summary>
    /// <param name="dirPath">
    /// Directory to check
    /// </param>
    /// <returns>
    /// All possible creators
    /// </returns>
    public static List<OBJModelCreator> GetCreatorsFromDirectory(string dirPath)
    {
        List<OBJModelCreator> listToReturn = new List<OBJModelCreator>();

        var allFilePaths = Directory.GetFiles(dirPath);

        foreach(var filePath in allFilePaths)
        {
            //Creating and adding new obj model creator if path is ok
            if(OBJModelCreator.CheckOBJFilePath(filePath) == null)
                listToReturn.Add(new OBJModelCreator(filePath));
        }

        return listToReturn;
    }
}
