﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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


    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            //Initializing app if not initialzied
            if (!AppInitialized) InitApp();
        }
        catch(Exception err)
        {
            Debug.Log(err.Message);
        }
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

        _appIninitalied = true;
    }
}
