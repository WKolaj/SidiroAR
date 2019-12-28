using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetModelCreator
{
    /// <summary>
    /// Instance of created model
    /// </summary>
    private AssetModel _model = null;
    public AssetModel Model
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
    /// Bundle File path
    /// </summary>
    private string _bundleFilePath = null;
    public string BundleFilePath
    {
        get
        {
            return _bundleFilePath;
        }
    }


    /// <summary>
    /// Method for checking bundle file path
    /// </summary>
    /// <param name="smdlFilePath">
    /// File path to check
    /// </param>
    /// <returns>
    /// null - if path is ok
    /// string with message otherwise
    /// </returns>
    private static string CheckSMDLFilePath(string smdlFilePath)
    {
        //Checking if file exists
        if (!File.Exists(smdlFilePath)) return string.Format("File not found: {0}", smdlFilePath);

        //Checking file extension
        var extension = Path.GetExtension(smdlFilePath);
        if (extension != ".smdl") return string.Format("SMDL file has invalid extension: {0}", extension);

        //Returning null if path is ok
        return null;
    }

    /// <summary>
    /// Method for getting name based on smdl file path
    /// </summary>
    /// <param name="smdlFilePath">
    /// SMDL file path
    /// </param>
    /// <returns>
    /// Name for model
    /// </returns>
    private static string GetModelName(string smdlFilePath)
    {
        return Path.GetFileNameWithoutExtension(smdlFilePath);
    }

    /// <summary>
    /// Creator of Asset models
    /// </summary>
    /// <param name="smdlFilePath">
    /// Path for smdl file
    /// </param>
    public AssetModelCreator(string smdlFilePath)
    {
        //Checking obj file path
        var pathCheckResult = CheckSMDLFilePath(smdlFilePath);
        if (pathCheckResult != null) throw new InvalidDataException(pathCheckResult);

        //Assigning obj file path
        this._bundleFilePath = smdlFilePath;

        //Assigning model name
        this._modelName = AssetModelCreator.GetModelName(smdlFilePath);

    }

    /// <summary>
    /// Method for creating new asset model
    /// </summary>
    /// <param name="parentGO">
    /// Parent game object for created model
    /// </param>
    /// <returns>
    /// New asset model
    /// </returns>
    public AssetModel CreateModel(GameObject parentGO)
    {
        GameObject modelPrefab = AssetBundleLoader.LoadBundle(this.BundleFilePath);

        var modelGO = GameObject.Instantiate(modelPrefab);

        modelGO.transform.SetParent(parentGO.transform);

        //Creating new OBJ Model script
        modelGO.AddComponent<AssetModel>();

        //getting and returning obj model script
        _model = modelGO.GetComponent<AssetModel>();
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
    public static List<AssetModelCreator> GetCreatorsFromDirectory(string dirPath)
    {
        List<AssetModelCreator> listToReturn = new List<AssetModelCreator>();

        var allFilePaths = Directory.GetFiles(dirPath);

        foreach(var filePath in allFilePaths)
        {
            //Creating and adding new obj model creator if path is ok
            if(AssetModelCreator.CheckSMDLFilePath(filePath) == null)
                listToReturn.Add(new AssetModelCreator(filePath));
        }

        return listToReturn;
    }
}
