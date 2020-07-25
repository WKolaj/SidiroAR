using System;
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
    private FileDownloader _fileDownloader;

    private bool _fileForModelExists = false;

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

        this._fileDownloader = this.GetComponentInChildren<FileDownloader>();
    }

    private void Start()
    {
        //Assinging width to button
        _stopDownloadButton.SetWidth(this.rectTransform.rect.width - _buttonContainerRectTrans.offsetMin.x);
    }

    /// <summary>
    /// Method called to assing model to model item
    /// </summary>
    /// <param name="model"></param>
    public void AssignModel(AssetModelLoader model)
    {
        this._model = model;

        this._model.OnDownloadCanceled = HandleDownloadCanceled;
        this._model.OnDownloadCompleted = HandleDownloadCompleted;
        this._model.OnDownloadFailure = HandleDownloadFailure;
        this._model.OnProgressChanged = HandleDownloadProgressChanged;
        this._model.OnDownloadStarted = HandleDownloadStart;

        this._model.AssignFileDownloader(this._fileDownloader);
    }

    /// <summary>
    /// MUST BE IMPLEMENTED IN ORDER TO FIT SCROLL VIEW WIDTH
    /// </summary>
    private void InitResizeMechanism()
    {
        rectTransform = GetComponent<RectTransform>();
        ScrollRect scrollRect = GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
            scrollRectRectTransform = scrollRect.GetComponent<RectTransform>();
    }

    /// <summary>
    /// MUST BE IMPLEMENTED IN ORDER TO FIT SCROLL VIEW WIDTH
    /// </summary>
    protected void OnEnable()
    {
        UpdateWidth();
    }

    /// <summary>
    /// MUST BE IMPLEMENTED IN ORDER TO FIT SCROLL VIEW WIDTH
    /// </summary>
    protected void OnRectTransformDimensionsChange()
    {
        UpdateWidth(); // Update every time if parent changed
    }

    /// <summary>
    /// MUST BE IMPLEMENTED IN ORDER TO FIT SCROLL VIEW WIDTH
    /// </summary>
    private void UpdateWidth()
    {
        if (rectTransform)
            rectTransform.sizeDelta = new Vector2(scrollRectRectTransform.rect.size.x, rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// Method used for refreshing model item 
    /// </summary>
    public void RefreshDataDisplay()
    {
        _refreshDataForUI();

        _refreshUI();

    }

    /// <summary>
    /// method for refreshing data based on which, ui is being rendered
    /// </summary>
    private void _refreshDataForUI()
    {
        _fileForModelExists = Model.CheckIfModelFileExists();
    }

    /// <summary>
    /// Method for rendering ui based on model's data
    /// </summary>
    private void _refreshUI()
    {
        this._modeNameLabel.text = Model.ModelName;

        //Depending on platform smld or ismdl file are being downloaded

        #region PLATFORM_DEPENDED_CODE

        Common.RunplatformDependendCode(
            () => {
                //Android Code

                //Refreshing buttons visibility
                if (!Model.FileExists)
                {
                    this._startDownloadButtonGO.SetActive(false);
                    this._stopDownloadButtonGO.SetActive(false);
                    this._deleteFileButtonGO.SetActive(false);
                    this._showButtonGO.SetActive(false);
                }
                else if (_fileForModelExists)
                {
                    this._startDownloadButtonGO.SetActive(false);
                    this._stopDownloadButtonGO.SetActive(false);
                    this._deleteFileButtonGO.SetActive(true);
                    this._showButtonGO.SetActive(true);
                }
                else if (_model.IsDownloading)
                {
                    this._startDownloadButtonGO.SetActive(false);
                    this._stopDownloadButtonGO.SetActive(true);
                    this._deleteFileButtonGO.SetActive(false);
                    this._showButtonGO.SetActive(false);
                }
                else
                {
                    this._startDownloadButtonGO.SetActive(true);
                    this._stopDownloadButtonGO.SetActive(false);
                    this._deleteFileButtonGO.SetActive(false);
                    this._showButtonGO.SetActive(false);
                }

                return null;
            }, () =>
            {
                //IOS Code

                //Refreshing buttons visibility
                if (!Model.IOSFileExists)
                {
                    this._startDownloadButtonGO.SetActive(false);
                    this._stopDownloadButtonGO.SetActive(false);
                    this._deleteFileButtonGO.SetActive(false);
                    this._showButtonGO.SetActive(false);
                }
                else if (_fileForModelExists)
                {
                    this._startDownloadButtonGO.SetActive(false);
                    this._stopDownloadButtonGO.SetActive(false);
                    this._deleteFileButtonGO.SetActive(true);
                    this._showButtonGO.SetActive(true);
                }
                else if (_model.IsDownloading)
                {
                    this._startDownloadButtonGO.SetActive(false);
                    this._stopDownloadButtonGO.SetActive(true);
                    this._deleteFileButtonGO.SetActive(false);
                    this._showButtonGO.SetActive(false);
                }
                else
                {
                    this._startDownloadButtonGO.SetActive(true);
                    this._stopDownloadButtonGO.SetActive(false);
                    this._deleteFileButtonGO.SetActive(false);
                    this._showButtonGO.SetActive(false);
                }
                return null;
            });

        #endregion PLATFORM_DEPENDED_CODE

    }

    /// <summary>
    /// Method for handling start download of model file
    /// </summary>
    public void HandleStartDownloadButtonClicked()
    {
        Model.StartDownload();
    }

    /// <summary>
    /// Method for handling delete file button clicked
    /// </summary>
    public void HandleDeleteButtonClicked()
    {
        MainCanvas.ShowDeleteModelFileWindow(this);
    }

    /// <summary>
    /// Method for handling showing model in AR
    /// </summary>
    public void HandleShowButtonClicked()
    {
        if (Model != null && Model.CheckIfModelFileExists())
        {
            //Assigning current model path and starting ar scene
            Common.ModelPath = this.Model.BundleFilePath;

            //Loading ar scene
            Common.LoadARScene();
        }

    }

    /// <summary>
    /// Method for handling stop download clicked
    /// </summary>
    public void HandleStopDownloadButtonClicked()
    {
        MainCanvas.ShowStopModelDownloadingWindow(this);
    }

    private void Update()
    {
        //Making translations
        _startDownloadButton.SetText(Translator.GetTranslation("ModelItem.StartDownloadFileButtonText"));
        _stopDownloadButton.SetText(Translator.GetTranslation("ModelItem.StopDownloadFileButtonText"));
        _deleteFileButton.SetText(Translator.GetTranslation("ModelItem.RemoveFileButtonText"));
        _showButton.SetText(Translator.GetTranslation("ModelItem.ShowModelButtonText"));
    }

    /// <summary>
    /// Method for handling download completed
    /// </summary>
    /// <param name="progress"></param>
    private void HandleDownloadCompleted()
    {
        RefreshDataDisplay();
    }

    /// <summary>
    /// Method for handling download progress changed
    /// </summary>
    /// <param name="progress"></param>
    private void HandleDownloadProgressChanged(int progress)
    {
        this._stopDownloadButton.SetBarPercentage(Convert.ToSingle(progress));
    }

    /// <summary>
    /// Method for handling download start
    /// </summary>
    /// <param name="progress"></param>
    private void HandleDownloadStart()
    {
        RefreshDataDisplay();
    }

    /// <summary>
    /// Method for handling download cancel
    /// </summary>
    /// <param name="progress"></param>
    private void HandleDownloadCanceled()
    {
        RefreshDataDisplay();
    }

    /// <summary>
    /// Method for handling download failure
    /// </summary>
    private void HandleDownloadFailure(long errorCode, string errorMessage)
    {
        MainCanvas.ShowDownloadingErrorWindow();
        RefreshDataDisplay();
    }


    //Stopping downloading if it is in progress but item is being destroyed
    private void OnDestroy()
    {
        if (Model != null && Model.CheckIfModelFileExists() && Model.IsDownloading)
        {
            Model.StopDownload();
        }
    }
}
