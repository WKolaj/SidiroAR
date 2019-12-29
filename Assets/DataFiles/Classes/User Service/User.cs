using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User
{
    /// <summary>
    /// Class representing user
    /// </summary> 
    /// <param name="id">
    /// id of user
    /// </param>
    /// <param name="name">
    /// name of user
    /// </param>
    /// <param name="jwt">
    /// jwt object used to authorize user on backend
    /// </param>
    /// <param name="models">
    /// All models owned by user
    /// </param>
    /// <param name="loader">
    /// User loader used to create this user
    /// </param>
    public User(string id, string name, string jwt, List<AssetModelLoader> models, UserLoader loader)
    {
        Init(id,name,jwt,models,loader);
    }

    /// <summary>
    /// Method for initializing user object
    /// </summary>
    /// <param name="id">
    /// id of user
    /// </param>
    /// <param name="name">
    /// name of user
    /// </param>
    /// <param name="jwt">
    /// jwt object used to authorize user on backend
    /// </param>
    /// <param name="models">
    /// All models owned by user
    /// </param>
    /// <param name="loader">
    /// User loader used to create this user
    /// </param>
    private void Init(string id, string name, string jwt, List<AssetModelLoader> models, UserLoader loader)
    {
        CreateUserDirIfNotExists();
        this._userLoader = loader;
        SetData(id, name, jwt, models);
    }


    /// <summary>
    /// User loader used to generate this user object
    /// </summary>
    private UserLoader _userLoader;
    public UserLoader UserLoader
    {
        get
        {
            return _userLoader;
        }

    }

    private string _id;
    /// <summary>
    /// ID of user
    /// </summary>
    public string ID
    {
        get
        {
            return _id;
        }

    }

    private string _name;
    /// <summary>
    /// Name of user
    /// </summary>
    public string Name
    {
        get
        {
            return _name;
        }

    }

    private string _jwt;
    /// <summary>
    /// JWT of user
    /// </summary>
    public string JWT
    {
        get
        {
            return _jwt;
        }

    }

    /// <summary>
    /// Directory path of user
    /// </summary>
    public string DirectoryPath
    {
        get
        {
            return Path.Combine(Common.ModelsDirPath, this.ID);
        }
    }

    /// <summary>
    /// List of all models owned by user
    /// </summary>
    private List<AssetModelLoader> _modelList = null;
    public List<AssetModelLoader> ModelList
    {
        get
        {
            return _modelList;
        }
    }

    /// <summary>
    /// Method for creating user directory if not exists
    /// </summary>
    private void CreateUserDirIfNotExists()
    {
        if(!Directory.Exists(this.DirectoryPath))
        {
            Directory.CreateDirectory(this.DirectoryPath);
        }
    }

    /// <summary>
    /// Method for setting new data of user
    /// </summary>
    /// <param name="id">
    /// id of user
    /// </param>
    /// <param name="name">
    /// name of user
    /// </param>
    /// <param name="jwt">
    /// jwt object used to authorize user on backend
    /// </param>
    /// <param name="models">
    /// All models owned by user
    /// </param>
    public void SetData(string id, string name, string jwt, List<AssetModelLoader> models)
    {
        //Initialzing properties
        this._id = id;
        this._name = name;
        this._jwt = jwt;
        this._modelList = models;
    }

    /// <summary>
    /// Method for refreshing user data from server
    /// </summary>
    public void RefreshDataFromServer()
    {
        this.UserLoader.RefreshUsersDataFromServer(this);
    }

    /// <summary>
    /// Method for loggin out a user
    /// </summary>
    public void LogOut()
    {
        if (this.UserLoader.LoggedUser == this)
            this.UserLoader.LogoutUser();
    }

    /// <summary>
    /// Method for loggin in a user
    /// </summary>
    public void LogIn()
    {
        this.UserLoader.LoginUser(this);
    }

}
