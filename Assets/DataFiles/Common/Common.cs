using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Common 
{

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


    /// <summary>
    /// Path to applicaton files directory
    /// </summary>
    public static string AppDirPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, dataDirName);
        }
    }

    /// <summary>
    /// Path to models directory
    /// </summary>
    public static string ModelsDirPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, dataDirName, modelsDirName);
        }
    }

    /// <summary>
    /// Path to default user directory
    /// </summary>
    public static string DefaultUserDirPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, dataDirName, modelsDirName, defaultUserDirName);
        }
    }

    /// <summary>
    /// Path to currently loaded model
    /// </summary>
    private static string _modelPath = string.Empty;
    public static string ModelPath
    {
        get
        {
            return _modelPath;
        }

        set
        {
            _modelPath = value;
        }
    }


    public static void LoadARScene()
    {
        SceneManager.LoadScene("ARScene");
    }

    public static void LoadInitialScene()
    {
        SceneManager.LoadScene("InitialScene");
    }

    public static void QuitApp()
    {
        Application.Quit();
    }

    /// <summary>
    /// Method for generating random file name for download
    /// </summary>
    /// <returns></returns>
    public static string GenerateRandomTemporaryFilePath()
    {
        return Path.Combine(Application.temporaryCachePath,Path.GetRandomFileName());
    }

    public static void DispatchInMainThread(Action action)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(action);
    }
    
    /// <summary>
    /// Method for running code
    /// </summary>
    /// <param name="androidCode"></param>
    /// <param name="iosCode"></param>
    /// <returns></returns>
    public static object RunplatformDependendCode(Func<object> androidCode, Func<object> iosCode )
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    return androidCode();
                }

            case RuntimePlatform.IPhonePlayer:
                {
                    return iosCode();
                }

            case RuntimePlatform.WindowsEditor:
                {
                    return androidCode();
                }

            case RuntimePlatform.LinuxEditor:
                {
                    return androidCode();
                }

            case RuntimePlatform.OSXEditor:
                {
                    return androidCode();
                }

            default:
                {
                    throw new InvalidProgramException(String.Format("Unsupported platform :{0}", Application.platform));
                }
        }

    }
}

