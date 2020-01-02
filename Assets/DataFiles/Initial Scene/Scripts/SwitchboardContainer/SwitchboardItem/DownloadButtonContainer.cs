using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadButtonContainer : MonoBehaviour
{
    public GameObject radialProgressGO;
    private RadialProgress radialProgress;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        InitializeComponents();
        radialProgress.Hide();
    }

    private void InitializeComponents()
    {
        radialProgress = radialProgressGO.GetComponent<RadialProgress>();
    }

    public void SetProgress(float progress)
    {
        this.radialProgress.SetProgress(progress);
    }

    public void HideRadialProgress()
    {
        this.radialProgress.Hide();
    }

    public void ShowRadialProgress()
    {
        this.radialProgress.Show();
    }
}
