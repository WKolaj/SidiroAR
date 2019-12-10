using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Common 
{
    private static string _modelPath = string.Empty;
    public static string ModelPath
    {
        get
        {
            return _modelPath;
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
}
