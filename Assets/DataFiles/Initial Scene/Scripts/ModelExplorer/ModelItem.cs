﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModelItem : MonoBehaviour
{
    private GameObject nameLabel;
    private GameObject eyeButton;

    /// <summary>
    /// Instance of model creator
    /// </summary>
    private AssetModelCreator _modeCreator;
    public AssetModelCreator ModelCreator
    {
        get
        {
            return _modeCreator;
        }
    }

    /// <summary>
    /// Method for setting model creator
    /// </summary>
    /// <param name="modelCreator">
    /// Model creator to set
    /// </param>
    public void AssignModelCreator(AssetModelCreator modelCreator)
    {
        this._modeCreator = modelCreator;
        SetLabelText(this.ModelCreator.ModelName);

    }

    /// <summary>
    /// Method for setting text of name label
    /// </summary>
    /// <param name="text">
    /// Text to set
    /// </param>
    public void SetLabelText(string text)
    {
        nameLabel.GetComponent<TextMeshProUGUI>().SetText(text);
    }

    private RectTransform rectTransform, scrollRectRectTransform;

    protected void Awake()
    {
        InitResizeMechanism();
        InitComponents();
    }

    private void InitResizeMechanism()
    {
        rectTransform = GetComponent<RectTransform>();
        ScrollRect scrollRect = GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
            scrollRectRectTransform = scrollRect.GetComponent<RectTransform>();
    }

    protected void OnEnable()
    {
        UpdateWidth();
    }

    protected void OnRectTransformDimensionsChange()
    {
        UpdateWidth(); // Update every time if parent changed
    }

    private void UpdateWidth()
    {
        if (rectTransform)
            rectTransform.sizeDelta = new Vector2(scrollRectRectTransform.rect.size.x, rectTransform.sizeDelta.y);
    }

    private void InitComponents()
    {
        nameLabel = transform.Find("NameLabel").gameObject;
        eyeButton = transform.Find("EyeButton").gameObject;
    }

    /// <summary>
    /// Method for loading AR Scene based on path from game object creator
    /// </summary>
    public void LoadARSceneWithGameObjectCreator()
    {
        Common.ModelPath = ModelCreator.BundleFilePath;
        Common.LoadARScene();
    }

}
