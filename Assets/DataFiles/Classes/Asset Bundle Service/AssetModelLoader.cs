using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetModelLoader
{
    /// <summary>
    /// Class representing loader of asset models for loading it from internet or file
    /// </summary>
    /// <param name="id">
    /// Id of model
    /// </param>
    /// <param name="modelName">
    /// name of model
    /// </param>
    /// <param name="user">
    /// user who owns this model
    /// </param>
    public AssetModelLoader(string id, string modelName, User user)
    {
        this.Init(id, modelName, user);
    }

    /// <summary>
    /// Method for initializing asset model loader
    /// </summary>
    /// <param name="id">
    /// Id of model
    /// </param>
    /// <param name="modelName">
    /// name of model
    /// </param>
    /// <param name="user">
    /// user who owns this model
    /// </param>
    private void Init(string id, string modelName, User user)
    {
        this._id = id;
        this._modelName = modelName;
        this._user = user;
    }

    private string _id;
    /// <summary>
    /// ID of model
    /// </summary>
    public string ID
    {
        get
        {
            return _id;
        }

        private set
        {
            _id = value;
        }
    }

    private User _user;
    /// <summary>
    /// User of model
    /// </summary>
    public User User
    {
        get
        {
            return _user;
        }

        private set
        {
            _user = value;
        }
    }

    private string _modelName;
    /// <summary>
    /// Name of model
    /// </summary>
    public string ModelName
    {
        get
        {
            return _modelName;
        }

        private set
        {
            _modelName = value;
        }
    }

    private string _bundleFilePath;
    /// <summary>
    /// ID of model
    /// </summary>
    public string BundleFilePath
    {
        get
        {
            return Path.Combine(User.DirectoryPath, String.Format("{0}.{1}", this.ID, "smdl"));
        }

    }

    /// <summary>
    /// Asset model creator
    /// </summary>
    private AssetModelCreator _modelCreator = null;
    public AssetModelCreator ModelCreator
    {
        get
        {
            return _modelCreator;
        }
    }

    /// <summary>
    /// Method for checking if model file has already been downloaded
    /// </summary>
    public bool CheckIfModelFileExists()
    {
        return File.Exists(BundleFilePath);
    }

    /// <summary>
    /// Method for downloading model from server
    /// </summary>
    public void DownloadModelFromServer()
    {
        //TO DO LATER
        Debug.Log("Downloading model from server..");
    }

    /// <summary>
    /// Method for downloading model from server
    /// </summary>
    public void RemoveDownloadedModel()
    {
        if(this.CheckIfModelFileExists())
        {
            File.Delete(this.BundleFilePath);
        }
    }

    /// <summary>
    /// Method for creating and assigning model creator to loader object
    /// </summary>
    private void CreateAssetModelCreator()
    {
        //Constructor throws in case there is no such file!!
        this._modelCreator = new AssetModelCreator(this.BundleFilePath,this.ModelName);
    }
}
