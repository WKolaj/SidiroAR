using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModelItem : MonoBehaviour
{
    private AssetModelLoader _model = null;
    public AssetModelLoader Model
    {
        get
        {
            return _model;
        }

    }

    private RectTransform rectTransform, scrollRectRectTransform;

    private GameObject _modelNameLabelGO;

    private GameObject _buttonContainerGO;
    private GameObject _startDownloadButtonGO;
    private GameObject _deleteFileButtonGO;
    private GameObject _showButtonGO;
    private GameObject _stopDownloadButtonGO;

    private RectTransform _buttonContainerRectTrans;

    private TextMeshProUGUI _modeNameLabel;
    private ModelItemButton _startDownloadButton;
    private ModelItemButton _deleteFileButton;
    private ModelItemButton _showButton;
    private StopDownloadButton _stopDownloadButton;

    protected void Awake()
    {
        InitResizeMechanism();

        _buttonContainerGO = transform.Find("ButtonContainer").gameObject;

        this._modelNameLabelGO = transform.Find("NameLabel").gameObject;

        this._buttonContainerRectTrans = _buttonContainerGO.GetComponent<RectTransform>();

        this._startDownloadButtonGO = _buttonContainerGO.transform.Find("StartDownloadButton").gameObject;
        this._deleteFileButtonGO = _buttonContainerGO.transform.Find("DeleteButton").gameObject;
        this._showButtonGO = _buttonContainerGO.transform.Find("ShowButton").gameObject;
        this._stopDownloadButtonGO = _buttonContainerGO.transform.Find("StopDownloadButton").gameObject;

        this._modeNameLabel = _modelNameLabelGO.GetComponent<TextMeshProUGUI>();

        this._startDownloadButton = _startDownloadButtonGO.GetComponent<ModelItemButton>();
        this._deleteFileButton = _deleteFileButtonGO.GetComponent<ModelItemButton>();
        this._showButton = _showButtonGO.GetComponent<ModelItemButton>();
        this._stopDownloadButton = _stopDownloadButtonGO.GetComponent<StopDownloadButton>();

    }

    private void Start()
    {
        //Assinging width to button
        _stopDownloadButton.SetWidth(this.rectTransform.rect.width - _buttonContainerRectTrans.offsetMin.x);
    }

    private void Update()
    {
        if(Model != null)
        {
            this._modeNameLabel.text = Model.ModelName;
        }
    }

    public void AssignModel(AssetModelLoader model)
    {
        this._model = model;
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

    public void HandleStartDownloadButtonClicked()
    {

    }

    public void HandleDeleteButtonClicked()
    {

    }

    public void HandleShowButtonClicked()
    {

    }

    public void HandleStopDownloadButtonClicked()
    {

    }

}
