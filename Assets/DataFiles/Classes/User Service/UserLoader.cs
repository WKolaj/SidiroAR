using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLoader 
{
    /// <summary>
    /// Currently logged user - null if there is no user logged
    /// </summary>
    private User _loggedUser = null;
    public User LoggedUser
    {
        get
        {
            return _loggedUser;
        }
    }

    /// <summary>
    /// Class for loading users from server
    /// </summary>
    public UserLoader()
    {
        Init();
    }

    /// <summary>
    /// Method for initializing loader
    /// </summary>
    private void Init()
    {
        //TO DO LATER
    }

    /// <summary>
    /// Method for loggining in a user - set as currently logged
    /// </summary>
    /// <param name="user">
    /// user to log
    /// </param>
    public void LoginUser(User user)
    {
        this._loggedUser = user;
    }

    /// <summary>
    /// Method for loggining out a user - deleting it as currently logged
    /// </summary>
    /// <param name="user">
    /// user to log
    /// </param>
    public void LogoutUser()
    {
        this._loggedUser = null;
    }

    /// <summary>
    /// Method for getting data about user from server and adjusting it in user object
    /// </summary>
    /// <param name="user">
    /// User object
    /// </param>
    public void RefreshUsersDataFromServer(User user)
    {
        //TO DO LATER!!
    }
}
