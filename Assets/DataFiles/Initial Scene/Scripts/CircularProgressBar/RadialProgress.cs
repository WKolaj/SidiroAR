using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    private Image loadingBar;
    float currentValue;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        var loadingBarGO = this.transform.Find("LoadingBar").gameObject;
        this.loadingBar = loadingBarGO.GetComponent<Image>();
    }

    /// <summary>
    /// Method for setting progress
    /// </summary>
    /// <param name="value"></param>
    public void SetProgress(float value)
    {
        //Controlling max/min
        if (value >= 100) value = 100;
        if (value <= 0) value = 0;

        this.currentValue = value;
    }

    /// <summary>
    /// Method for showing radial progress
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Method for hiding radial progress
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        loadingBar.fillAmount = currentValue / 100;
    }
}
