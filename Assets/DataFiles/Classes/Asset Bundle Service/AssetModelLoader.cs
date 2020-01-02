using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class AssetModelLoader
{
    /// <summary>
    /// Class representing loader of asset models for loading it from internet or file
    /// </summary>
    /// <param name="id">
    /// Id of model
    /// </param>
    /// <param name="modelName">
    /// name of model
    /// </param>
    /// <param name="user">
    /// user who owns this model
    /// </param>
    public AssetModelLoader(string id, string modelName, User user)
    {
        this.Init(id, modelName, user);
    }

    /// <summary>
    /// Method for initializing asset model loader
    /// </summary>
    /// <param name="id">
    /// Id of model
    /// </param>
    /// <param name="modelName">
    /// name of model
    /// </param>
    /// <param name="user">
    /// user who owns this model
    /// </param>
    private void Init(string id, string modelName, User user)
    {
        this._id = id;
        this._modelName = modelName;
        this._user = user;
    }

    private string _id;
    /// <summary>
    /// ID of model
    /// </summary>
    public string ID
    {
        get
        {
            return _id;
        }

        private set
        {
            _id = value;
        }
    }

    private User _user;
    /// <summary>
    /// User of model
    /// </summary>
    public User User
    {
        get
        {
            return _user;
        }

        private set
        {
            _user = value;
        }
    }

    private string _modelName;
    /// <summary>
    /// Name of model
    /// </summary>
    public string ModelName
    {
        get
        {
            return _modelName;
        }

        private set
        {
            _modelName = value;
        }
    }

    private string _bundleFilePath;
    /// <summary>
    /// ID of model
    /// </summary>
    public string BundleFilePath
    {
        get
        {
            return Path.Combine(User.DirectoryPath, String.Format("{0}.{1}", this.ID, "smdl"));
        }

    }

    /// <summary>
    /// Asset model creator
    /// </summary>
    private AssetModelCreator _modelCreator = null;
    public AssetModelCreator ModelCreator
    {
        get
        {
            return _modelCreator;
        }
    }

    /// <summary>
    /// Is asset model during downloading model form server
    /// </summary>
    public bool IsDownloading
    {
        get
        {
            return this.downloadHandler != null;
        }
    }

    /// <summary>
    /// Handler for download action
    /// </summary>
    private WebClient downloadHandler = null;

    /// <summary>
    /// Progress of download action
    /// </summary>
    private float _downloadProgress = 0.0f;
    public float DownloadProgress
    {
        get
        {
            return _downloadProgress;
        }
    }

    /// <summary>
    /// Event called when download is started
    /// </summary>
    public event Action OnDownloadStarted;

    /// <summary>
    /// Event called when progress changed
    /// </summary>
    public event Action<float> OnProgressChanged;

    /// <summary>
    /// Event called when download has been completed
    /// </summary>
    public event Action OnDownloadCompleted;

    /// <summary>
    /// Event called when download has been canceled
    /// </summary>
    public event Action OnDownloadCanceled;

    /// <summary>
    /// Event called when download has failed
    /// </summary>
    public event Action<string> OnDownloadFailure;

    /// <summary>
    /// Method for checking if model file has already been downloaded
    /// </summary>
    public bool CheckIfModelFileExists()
    {
        return File.Exists(BundleFilePath);
    }

    /// <summary>
    /// Method for handling hange progress event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        SetProgress(e.ProgressPercentage);
    }

    /// <summary>
    /// Method for setting progress
    /// </summary>
    /// <param name="progress">
    /// new progress
    /// </param>
    private void SetProgress(float progress)
    {
        this._downloadProgress = progress;

        if (OnProgressChanged != null) OnProgressChanged(progress);
    }

    /// <summary>
    /// Method for handling download completed action
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            if (OnDownloadCanceled != null)
            {
                OnDownloadCanceled();
            }

            SetProgress(0);
            RemoveDownloadedModel();
        }
        else if (e.Error != null)
        {
            if(OnDownloadFailure!= null)
            {
                OnDownloadFailure(e.Error.Message);
            }

            SetProgress(0);
            RemoveDownloadedModel();
        }
        else
        {
            if(OnDownloadCompleted != null)
            {
                OnDownloadCompleted();
            }

            SetProgress(100);
        }

        //Removing handler from model
        this.downloadHandler = null;
    }

    /// <summary>
    /// Method for downloading model from server
    /// </summary>
    public void DownloadModelFromServer()
    {
        if (this.downloadHandler != null && this.downloadHandler.IsBusy)
            StopDownloading();

        downloadHandler = AssetModelService.DownloadAssembly(this.User.ID, this.ID, this.BundleFilePath);
        downloadHandler.DownloadProgressChanged += HandleProgressChanged;
        downloadHandler.DownloadFileCompleted += HandleDownloadCompleted;

        if (OnDownloadStarted != null) OnDownloadStarted();

    }



    /// <summary>
    /// Method for downloading model from server
    /// </summary>
    public void StopDownloading()
    {
        if (this.downloadHandler != null && this.downloadHandler.IsBusy)
        {
            this.downloadHandler.CancelAsync();
            this.downloadHandler = null;
        }

    }

    /// <summary>
    /// Method for downloading model from server
    /// </summary>
    public void RemoveDownloadedModel()
    {
        if(this.CheckIfModelFileExists())
        {
            File.Delete(this.BundleFilePath);
        }
    }

    /// <summary>
    /// Method for creating and assigning model creator to loader object
    /// </summary>
    private void CreateAssetModelCreator()
    {
        //Constructor throws in case there is no such file!!
        this._modelCreator = new AssetModelCreator(this.BundleFilePath,this.ModelName);
    }
}
