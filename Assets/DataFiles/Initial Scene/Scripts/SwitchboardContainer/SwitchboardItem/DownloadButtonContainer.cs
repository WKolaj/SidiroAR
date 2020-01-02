using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownloadButtonContainer : MonoBehaviour
{
    public GameObject radialProgressGO;
    public GameObject downloadButtonGO;

    private RadialProgress radialProgress;


    private bool _downloadEnable = false;
    /// <summary>
    /// Can button be used to download file
    /// </summary>
    public bool DownloadEnable
    {
        get
        {
            return _downloadEnable;
        }
    }

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

    /// <summary>
    /// Method for enabling download possibility
    /// </summary>
    public void EnableDownload()
    {
        this._downloadEnable = true;
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Method for disabling download possibility
    /// </summary>
    public void DisableDownload()
    {
        this._downloadEnable = false;
        this.gameObject.SetActive(false);
    }
}
