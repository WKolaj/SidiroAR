using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchboardItem : MonoBehaviour
{
    private AssetModelLoader _modelLoader = null;
    public AssetModelLoader ModelLoader
    {
        get
        {
            return _modelLoader;
        }
    }

    private RectTransform rectTransform, scrollRectRectTransform;

    private TextMeshProUGUI nameLabel = null;

    private MainCanvas mainCanvas = null;

    private DownloadButtonContainer downloadButtonContainer = null;

    private bool fileExists = false;
    private GameObject eyeButtonGO = null;
    private GameObject removeButtonGO = null;
    private SwitchboardItemEyeButton eyebutton = null;

    private void RefreshFileExistance()
    {
        if(_modelLoader != null)
        {
            fileExists = _modelLoader.CheckIfModelFileExists();
        }
        else
        {
            fileExists = false;
        }
    }

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

    private void Update()
    {
        RefreshButtonsVisibility();
    }

    private void RefreshButtonsVisibility()
    {
        if(this.fileExists && !this._modelLoader.IsDownloading)
        {
            this.downloadButtonContainer.DisableDownload();
            this.removeButtonGO.SetActive(true);
            this.eyebutton.EnableView();
        }
        else
        {
            this.downloadButtonContainer.EnableDownload();
            this.removeButtonGO.SetActive(false);
            this.eyebutton.DisableView();
        }
    }


    /// <summary>
    /// Method for initialzing item
    /// </summary>
    /// <param name="loader">
    /// model loader to assign
    /// </param>
    public void Init(AssetModelLoader loader, MainCanvas canvas)
    {
        InitializeComponents();
        this.mainCanvas = canvas;
        this._modelLoader = loader;

        this._modelLoader.OnDownloadCanceled += HandleDownloadCanceled;
        this._modelLoader.OnDownloadCompleted += HandleDownloadCompleted;
        this._modelLoader.OnDownloadFailure += HandleDownloadFailure;
        this._modelLoader.OnProgressChanged += HandleDownloadProgressChanged;
        this._modelLoader.OnDownloadStarted += HandleDownloadStart;


        this.nameLabel.text = loader.ModelName;

        RefreshFileExistance();
    }

    /// <summary>
    /// Method for initializing components
    /// </summary>
    private void InitializeComponents()
    {
        var nameLabelGO = this.transform.Find("NameLabel").gameObject;
        nameLabel = nameLabelGO.GetComponent<TextMeshProUGUI>();

        var buttonsContainer = this.transform.Find("ButtonsContainer").gameObject;
        var downloadButtonContainerGO = buttonsContainer.transform.Find("DownloadButtonContainer").gameObject;
        this.downloadButtonContainer = downloadButtonContainerGO.GetComponent<DownloadButtonContainer>();

        this.eyeButtonGO = buttonsContainer.transform.Find("EyeButton").gameObject;
        this.removeButtonGO = buttonsContainer.transform.Find("RemoveButton").gameObject;
        this.eyebutton = eyeButtonGO.GetComponent<SwitchboardItemEyeButton>();
    }

    /// <summary>
    /// Method for handling download completed
    /// </summary>
    /// <param name="progress"></param>
    private void HandleDownloadCompleted()
    {
        Common.DispatchInMainThread(new Action(() =>
        {
            this.downloadButtonContainer.HideRadialProgress();

            RefreshFileExistance();

        }));
    }

    /// <summary>
    /// Method for handling download progress changed
    /// </summary>
    /// <param name="progress"></param>
    private void HandleDownloadProgressChanged(float progress)
    {
        Common.DispatchInMainThread(new Action(() =>
        {
            this.downloadButtonContainer.SetProgress(progress);

        }));
    }

    /// <summary>
    /// Method for handling download start
    /// </summary>
    /// <param name="progress"></param>
    private void HandleDownloadStart()
    {
        Common.DispatchInMainThread(new Action(() =>
        {
            this.downloadButtonContainer.ShowRadialProgress();

            RefreshFileExistance();
        }));
    }

    /// <summary>
    /// Method for handling download cancel
    /// </summary>
    /// <param name="progress"></param>
    private void HandleDownloadCanceled()
    {
        Common.DispatchInMainThread(new Action(() =>
        {
            this.downloadButtonContainer.HideRadialProgress();

            RefreshFileExistance();
        }));
    }

    /// <summary>
    /// Method for handling download failure
    /// </summary>
    /// <param name="progress"></param>
    private void HandleDownloadFailure(string errorMessage)
    {
        Common.DispatchInMainThread(new Action(() =>
        {
            this.downloadButtonContainer.HideRadialProgress();
            mainCanvas.ShowDialogBox("Błąd pobierania", errorMessage, DialogBoxMode.Warning, DialogBoxType.Ok);
            RefreshFileExistance();
        }));
    }

    /// <summary>
    /// Method called when remove button is clicked
    /// </summary>
    public void HandleRemoveButtonClick()
    {
        var dialogBox = mainCanvas.ShowDialogBox("Usuwanie", string.Format("Czy na pewno usunąć model {0} z dysku?", this._modelLoader.ModelName), DialogBoxMode.Question, DialogBoxType.YesNo);
        dialogBox.onYesClicked += new System.Action(() =>
        {
            _modelLoader.DeleteModelFileIfExists();

            RefreshFileExistance();
        });
    }

    /// <summary>
    /// Method called when download button is clicked
    /// </summary>
    public void HandleDownloadButtonClicked()
    {
        if (_modelLoader.IsDownloading)
        {
            var dialogBox = mainCanvas.ShowDialogBox("Zatrzymanie", string.Format("Czy na pewno przerwać pobieranie {0}?", this._modelLoader.ModelName), DialogBoxMode.Question, DialogBoxType.YesNo);
            dialogBox.onYesClicked += new System.Action(() =>
            {
                _modelLoader.StopDownload();

            });
        }
        else
        {
            _modelLoader.StartDownload();

        }

        RefreshFileExistance();
    }

    /// <summary>
    /// Method called when eye button is clicked
    /// </summary>
    public void HandleEyeButtonClicked()
    {
        if(_modelLoader!=null && _modelLoader.CheckIfModelFileExists())
        {
            //Assigning current model path and starting ar scene
            Common.ModelPath = this._modelLoader.BundleFilePath;
            Common.LoadARScene();
        }
    }

    //Stopping downloading if it is in progress but item is being destroyed
    private void OnDestroy()
    {
        if(_modelLoader != null && _modelLoader.CheckIfModelFileExists() && _modelLoader.IsDownloading)
        {
            _modelLoader.StopDownload();
        }
    }

}
