using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class User
{

    /// <summary>
    /// Class representing user
    /// </summary> 
    /// <param name="jsonData">
    /// json data of user
    /// </param>
    /// <param name="loader">
    /// User loader used to create this user
    /// </param>
    public User(UserJSONData jsonData, UserLoader loader)
    {
        Init(jsonData, loader);
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
    private void Init(UserJSONData jsonData, UserLoader loader)
    {
        this._userLoader = loader;
        SetData(jsonData);
        //Has to be invoked after set data - require setting of id!
        CreateUserDirIfNotExists();
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

    private string _email;
    /// <summary>
    /// Email of user
    /// </summary>
    public string Email
    {
        get
        {
            return _email;
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

    private Int32 _permissions;
    /// <summary>
    /// Permissions of user
    /// </summary>
    public Int32 Permissions
    {
        get
        {
            return _permissions;
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
        if (!Directory.Exists(this.DirectoryPath))
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
    }

    /// <summary>
    /// Method for setting new data of user
    /// </summary>
    /// <param name="jsonData">
    /// users jsonData
    /// </param>
    public void SetData(UserJSONData jsonData)
    {
        //Initialzing properties
        if(jsonData._id != null) this._id = jsonData._id;
        if (jsonData.name != null) this._name = jsonData.name;
        if (jsonData.jwt != null) this._jwt = jsonData.jwt;
        if (jsonData.email != null) this._email = jsonData.email;
        if (jsonData.permissions != -1) this._permissions = jsonData.permissions;
        if (jsonData.modelIds != null && jsonData.modelNames != null) this._modelList = UserLoader.GenerateAssetModelLoaders(this, jsonData.modelIds, jsonData.modelNames);

    }

    /// <summary>
    /// Method for refreshing user data from server
    /// </summary>
    public async Task RefreshDataFromServer()
    {
        await this.UserLoader.RefreshUsersDataFromServer(this);
    }

    /// <summary>
    /// Method for loggin out a user
    /// </summary>
    public void LogOut()
    {
        if (UserLoader.LoggedUser == this)
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
