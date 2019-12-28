using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ApplicationInitializer : MonoBehaviour
{

    private List<String> _modelsPaths = new List<string>();
    /// <summary>
    /// Paths to all models
    /// </summary>
    public List<String> ModelsPath
    {
        get
        {
            return _modelsPaths;
        }
    }

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

    /// <summary>
    /// Name of directory for app data
    /// </summary>
    private static string dataDirName = "data";

    /// <summary>
    /// Name for directory inside app data dir to store models
    /// </summary>
    private static string modelsDirName = "models";

    /// <summary>
    /// Name for directory inside models directory to store models for default user
    /// </summary>
    private static string defaultUserDirName = "_default";

    public string AppDirPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, dataDirName);
        }
    }

    public string ModelsDirPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, dataDirName, modelsDirName);
        }
    }

    public string DefaultUserDirPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, dataDirName, modelsDirName, defaultUserDirName);
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
        if (!Directory.Exists(AppDirPath)) Directory.CreateDirectory(AppDirPath);

        //Create directories if not exist
        if (!Directory.Exists(ModelsDirPath)) Directory.CreateDirectory(ModelsDirPath);

        //Create directories if not exist
        if (!Directory.Exists(DefaultUserDirPath)) Directory.CreateDirectory(DefaultUserDirPath);

        //Initializing all models path
        this.ModelsPath.Clear();

        var allFileNames = Directory.GetFiles(ModelsDirPath, "*.obj");

        foreach(var name in allFileNames)
        {
            this.ModelsPath.Add(Path.Combine(ModelsDirPath, name));
        }


        _appIninitalied = true;
    }
}
