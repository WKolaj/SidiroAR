using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Timers;
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
        this.timeoutHandler = new Timer();
        //60s timeout
        this.timeoutHandler.Interval = 10*1000;
        this.timeoutHandler.AutoReset = false;
        this.timeoutHandler.Elapsed += HandleTimeout;
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

    private string _temporaryDownloadPath = String.Empty;
    /// <summary>
    /// Path for temporary file download
    /// </summary>
    public string TemporaryDownloadPath
    {
        get
        {
            return _temporaryDownloadPath;
        }

        set
        {
            _temporaryDownloadPath = value;
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
    /// Handler for download action
    /// </summary>
    private WebClient downloadHandler = null;

    /// <summary>
    /// Timer for handling download timer error
    /// </summary>
    private Timer timeoutHandler = null;

    /// <summary>
    /// Method for handling timeout
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleTimeout(object sender, ElapsedEventArgs e)
    {
        //Stopping timer
        StopTimeoutTimer();

        //Stopping downloading
        StopDownload();

        //Invoking handling timeout error
        HandleDownloadFailure(new Exception("Download timeout error"));
    }

    private void StartTimeoutTimer()
    {
        timeoutHandler.Start();
    }

    private void StopTimeoutTimer()
    {
        timeoutHandler.Stop();
    }

    private void ResetTimeoutTimer()
    {
        timeoutHandler.Stop();
        timeoutHandler.Start();
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
    /// Method called to check if model file exists
    /// </summary>
    /// <returns>
    /// Method for checking if model file exists
    /// </returns>
    public bool CheckIfModelFileExists()
    {
        return File.Exists(this.BundleFilePath);
    }

    /// <summary>
    /// Method called to check if temp model file exists
    /// </summary>
    /// <returns>
    /// Method for checking if temp model file exists
    /// </returns>
    public bool CheckIfTempFileExists()
    {
        if (this.TemporaryDownloadPath == null) return false;

        return File.Exists(this.TemporaryDownloadPath);
    }

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
    /// Method for downloading model from server
    /// </summary>
    public void StartDownload()
    {
        if (this.IsDownloading) StopDownload();

        //Generating new temporary path for download file
        this._temporaryDownloadPath = Common.GenerateRandomTemporaryFilePath();

        //Generating new download handler
        downloadHandler = AssetModelService.DownloadAssembly(this.User.JWT, this.ID, this.TemporaryDownloadPath);
        downloadHandler.DownloadProgressChanged += HandleDownloadProgressChanged;
        downloadHandler.DownloadFileCompleted += HandleDownloadStop;

        //Starting download timeout
        StartTimeoutTimer();

        //Firing onDownloadStarted event if it is not a null
        if (OnDownloadStarted != null) OnDownloadStarted();
    }

    /// <summary>
    /// Method for downloading model from server
    /// </summary>
    public void StopDownload()
    {
        //Stopping download timeout
        StopTimeoutTimer();

        //Canceling download process
        this.downloadHandler.CancelAsync();

        //Removing handler from model
        if (this.downloadHandler != null) this.downloadHandler.Dispose();
        this.downloadHandler = null;

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
    /// Method for handling download completed action
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleDownloadStop(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            HandleDownloadCancel();
        }
        else if (e.Error != null)
        {
            HandleDownloadFailure(e.Error);

        }
        else
        {
            HandleDownloadFinish();
        }

        //Removing handler from model
        if(this.downloadHandler != null) this.downloadHandler.Dispose();
        this.downloadHandler = null;

        //Stopping timeout handler
        StopTimeoutTimer();
    }

    /// <summary>
    /// Method called when download is canceled
    /// </summary>
    private void HandleDownloadCancel()
    {
        SetProgress(0);

        if (OnDownloadCanceled != null)
        {
            OnDownloadCanceled();
        }
    }

    /// <summary>
    /// Method called when download fails
    /// </summary>
    private void HandleDownloadFailure(Exception err)
    {
        SetProgress(0);

        if (OnDownloadFailure != null)
        {
            OnDownloadFailure(err.Message);
        }

    }

    /// <summary>
    /// Method called when download finishes
    /// </summary>
    private void HandleDownloadFinish()
    {
        SetProgress(100);
        MoveTemporaryFileToModelDir();

        if (OnDownloadCompleted != null)
        {
            OnDownloadCompleted();
        }

    }

    /// <summary>
    /// Method for handling hange progress event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        //Resetting timer timeout
        ResetTimeoutTimer();

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
    /// Method for deleting model file if it exists
    /// </summary>
    public void DeleteModelFileIfExists()
    {
        if (CheckIfModelFileExists()) File.Delete(this.BundleFilePath);
    }

    /// <summary>
    /// Method for deleting model temporary file if it exists
    /// </summary>
    public void DeleteTempFileIfExists()
    {
        if (CheckIfTempFileExists()) File.Delete(this.TemporaryDownloadPath);
    }

    /// <summary>
    /// Method for moving temporary file to model directory
    /// </summary>
    private void MoveTemporaryFileToModelDir()
    {
        //Deleting file from model dir if it exists
        DeleteModelFileIfExists();

        File.Move(this.TemporaryDownloadPath, this.BundleFilePath);
    }

    /// <summary>
    /// Method for creating and assigning model creator to loader object
    /// </summary>
    private void CreateAssetModelCreator()
    {
        //Constructor throws in case there is no such file!!
        this._modelCreator = new AssetModelCreator(this.BundleFilePath, this.ModelName);
    }

    ~AssetModelLoader()
    {
        //Removing timer and clearing memory
        if (this.timeoutHandler != null)
        {
            StopTimeoutTimer();
            this.timeoutHandler.Dispose();
            this.timeoutHandler = null;
        }

    }
}
