using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class  Common
{
    private static GameObject model;

    public static GameObject GetModel()
    {
        return model;
    }

    public static void LoadARScene(GameObject modelToCreate)
    {
        model = modelToCreate;
        SceneManager.LoadScene("ARScene");
    }

    public static void LoadInitialScene()
    {
        SceneManager.LoadScene("InitialScene");
    }
}
