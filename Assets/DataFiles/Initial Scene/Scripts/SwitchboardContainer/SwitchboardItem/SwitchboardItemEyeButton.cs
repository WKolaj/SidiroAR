using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchboardItemEyeButton : MonoBehaviour
{
    private bool _viewEnabled = false;
    /// <summary>
    /// Method for checking if view of file is enabled
    /// </summary>
    public bool ViewEnabled
    {
        get
        {
            return _viewEnabled;
        }
    }

    private Button eyeButtonButton;
    private Image eyeButtonImage;
    public Sprite eyeButtonEnabledIcon;
    public Sprite eyeButtonDisabledIcon;

    private void Awake()
    {
        InitializeComponents();
        DisableView();
    }

    private void InitializeComponents()
    {
        eyeButtonImage = this.GetComponent<Image>();
        eyeButtonButton = this.GetComponent<Button>();
    }

    public void EnableView()
    {
        this.eyeButtonImage.sprite = eyeButtonEnabledIcon;
        this._viewEnabled = true;
        eyeButtonButton.enabled = true;
    }

    public void DisableView()
    {
        this.eyeButtonImage.sprite = eyeButtonDisabledIcon;
        this._viewEnabled = false;
        eyeButtonButton.enabled = false;
    }
}
