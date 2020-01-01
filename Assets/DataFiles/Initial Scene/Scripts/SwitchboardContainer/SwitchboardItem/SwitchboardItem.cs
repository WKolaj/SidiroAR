using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchboardItem : MonoBehaviour
{
    private AssetModelLoader modelLoader = null;

    private RectTransform rectTransform, scrollRectRectTransform;

    private TextMeshProUGUI nameLabel = null;

    protected void Awake()
    {
        InitResizeMechanism();
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
        if(rectTransform)
        rectTransform.sizeDelta = new Vector2(scrollRectRectTransform.rect.size.x, rectTransform.sizeDelta.y);
    }


    /// <summary>
    /// Method for initialzing item
    /// </summary>
    /// <param name="loader">
    /// model loader to assign
    /// </param>
    public void Init(AssetModelLoader loader)
    {
        InitializeComponents();
        this.modelLoader = loader;
        this.nameLabel.text = loader.ModelName;
    }

    /// <summary>
    /// Method for initializing components
    /// </summary>
    private void InitializeComponents()
    {
        var nameLabelGO = this.transform.Find("NameLabel").gameObject;
        nameLabel = nameLabelGO.GetComponent<TextMeshProUGUI>();
    }
}
