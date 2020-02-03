using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class ApplicationInitializer : MonoBehaviour
{

    private bool _appIninitalied = false;
    /// <summary>
    /// Property indicating whether application has already been initialized
    /// </summary>
    public bool AppInitialized
    {
        get
        {
            return _appIninitalied;
        }
    }

    [SerializeField]
    private GameObject _mainCanvasGO;
    /// <summary>
    /// Main canvas object
    /// </summary>
    public GameObject MainCanvasGO
    {
        get
        {
            return _mainCanvasGO;
        }

        set
        {
            _mainCanvasGO = value;
        }
    }

    private MainCanvas mainCanvas;

    [SerializeField]
    private UserLoader _userLoader;
    /// <summary>
    /// Object of user loader - for creating and managing users
    /// </summary>
    public UserLoader Loader
    {
        get
        {
            return _userLoader;
        }
        set
        {
            _userLoader = value;
        }
    }


    private void Awake()
    {
        InitializeComponents();
        InitApp();
    }

    private void InitializeComponents()
    {
        mainCanvas = MainCanvasGO.GetComponent<MainCanvas>();
        mainCanvas.InitializeComponents(this);
    }

    /// <summary>
    /// Method for initializing application
    /// </summary>
    private void InitApp()
    {
        //Create directories if not exist
        if (!Directory.Exists(Common.AppDirPath)) Directory.CreateDirectory(Common.AppDirPath);

        //Create directories if not exist
        if (!Directory.Exists(Common.ModelsDirPath)) Directory.CreateDirectory(Common.ModelsDirPath);

        //Create directories if not exist
        if (!Directory.Exists(Common.DefaultUserDirPath)) Directory.CreateDirectory(Common.DefaultUserDirPath);

        InitUserLoader();

        _appIninitalied = true;
    }

    /// <summary>
    /// Method for initializing user loader
    /// </summary>
    private void InitUserLoader()
    {
        //Trying loading user from prefs
        Loader.LoginUserFromPlayerPrefs();

        if(UserLoader.LoggedUser != null)
        {
            RefreshDataAccordingToNewUser(UserLoader.LoggedUser);
        }
        else
        {
            mainCanvas.ShowLoginWindow();
        }

        mainCanvas.RefreshUserDisplay(UserLoader.LoggedUser);

    }

    /// <summary>
    /// Method for setting up ui based on new user object
    /// </summary>
    /// <param name="user">
    /// New user object
    /// </param>
    private void RefreshDataAccordingToNewUser(User user)
    {
        mainCanvas.RefreshUserDataFromServer();
    }
}
