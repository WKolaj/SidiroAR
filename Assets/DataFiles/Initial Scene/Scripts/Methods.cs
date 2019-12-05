using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Methods : MonoBehaviour
{
    public GameObject model;
    public void LoadARScene()
    {
        Common.LoadARScene(model);
    }


    public void QuitApp()
    {
        Application.Quit();
    }
}
