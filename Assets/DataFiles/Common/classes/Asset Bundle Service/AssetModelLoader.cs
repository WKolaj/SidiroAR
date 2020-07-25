using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Timers;
using UnityEngine;

public class AssetModelLoader
{
    static string fileApiURL = "https://sidiro.pl/sidiroar/api/file/me";
    static string iosFileApiURL = "https://sidiro.pl/sidiroar/api/file/ios/me";

    /// <summary>
    /// File downloader
    /// </summary>
    private FileDownloader _fileDownloader = null;
    public FileDownloader FileDownloader
    {
        get
        {
            return this._fileDownloader;
        }

        private set
        {
            this._fileDownloader = value;
        }
    }

    /// <summary>
    /// Flag to determine if model file is downloading
    /// </summary>
    public bool IsDownloading
    {
        get
        {
            if (FileDownloader == null) return false;

            return FileDownloader.IsDownloading;
        }
    }

    /// <summary>
    /// Method for assigning and initializing file downloader
    /// </summary>
    /// <param name="fileDownloader">
    /// File downloader to assign
    /// </param>
    public void AssignFileDownloader(FileDownloader fileDownloader)
    {
        this._fileDownloader = fileDownloader;

        if (UserLoader.LoggedUser == null) throw new InvalidOperationException("User is not logged in!");

        fileDownloader.SetHeader("x-auth-token", this.User.JWT);

        //timeout 30s
        fileDownloader.SetTimeout(30);

        #region PLATFORM_DEPENDED_CODE

        Common.RunplatformDependendCode(
            () => {
                //Android Code

                fileDownloader.SetURL(String.Format("{0}/{1}", fileApiURL, this.ID));

                return null;
            }, () =>
            {
                //IOS Code

                fileDownloader.SetURL(String.Format("{0}/{1}", iosFileApiURL, this.ID));

                return null;
            });

        #endregion PLATFORM_DEPENDED_CODE
    }


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
    public AssetModelLoader(string id, string modelName, bool fileExists, User user, bool iosFileExists)
    {
        this.Init(id, modelName, fileExists, user, iosFileExists);
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
    private void Init(string id, string modelName, bool fileExists, User user, bool iosFileExists)
    {
        this._id = id;
        this._modelName = modelName;
        this._user = user;
        this._fileExists = fileExists;
        this._iosFileExists = iosFileExists;
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

    private bool _fileExists;
    /// <summary>
    /// File exists on server
    /// </summary>
    public bool FileExists
    {
        get
        {
            return _fileExists;
        }

        private set
        {
            _fileExists = value;
        }
    }

    private bool _iosFileExists;
    /// <summary>
    /// File exists on server
    /// </summary>
    public bool IOSFileExists
    {
        get
        {
            return _iosFileExists;
        }

        private set
        {
            _iosFileExists = value;
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
            #region PLATFORM_DEPENDED_CODE

            return (String)Common.RunplatformDependendCode(
                () => {
                    //Android Code

                    return Path.Combine(User.DirectoryPath, String.Format("{0}.{1}", this.ID, "smdl"));
                }, () =>
                {
                    //IOS Code

                    return Path.Combine(User.DirectoryPath, String.Format("{0}.{1}", this.ID, "ismdl"));
                });

            #endregion PLATFORM_DEPENDED_CODE


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
    private int _downloadProgress = 0;
    public int DownloadProgress
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
        //Generating new temporary path for download file
        this._temporaryDownloadPath = Common.GenerateRandomTemporaryFilePath();

        //Starting downloading
        FileDownloader.StartDownload(this.TemporaryDownloadPath, _handleDownloadingStarted, _handleDownloadingStopped, _handleProgressChanged, _handleDownloadingFinished, _handleDownloadingError);
    }

    /// <summary>
    /// Method for downloading model from server
    /// </summary>
    public void StopDownload()
    {
        //Canceling download process
        FileDownloader.StopDownloading();
    }

    /// <summary>
    /// Method called when downloading error occurs
    /// </summary>
    /// <param name="errorCode">
    /// Error code
    /// </param>
    /// <param name="errorText">
    /// Error text
    /// </param>
    private void _handleDownloadingError(long errorCode, string errorText)
    {
        if(OnDownloadFailure != null)
        {
            OnDownloadFailure(errorCode, errorText);
        }
    }

    /// <summary>
    /// Method called when downloading finished successfuly
    /// </summary>
    private void _handleDownloadingFinished()
    {
        //Moving file from temporary location to permanent location
        MoveTemporaryFileToModelDir();

        if (OnDownloadCompleted != null)
        {
            OnDownloadCompleted();
        }
    }

    /// <summary>
    /// Method called when progress changes
    /// </summary>
    private void _handleProgressChanged(int value)
    {
        if (OnProgressChanged != null)
        {
            OnProgressChanged(value);
        }
    }

    /// <summary>
    /// Method called when downloading stops
    /// </summary>
    private void _handleDownloadingStopped()
    {
        if (OnDownloadCanceled != null)
        {
            OnDownloadCanceled();
        }
    }

    /// <summary>
    /// Method called when downloading starts
    /// </summary>
    private void _handleDownloadingStarted()
    {
        if (OnDownloadStarted != null)
        {
            OnDownloadStarted();
        }
    }

    /// <summary>
    /// Event called when download is started
    /// </summary>
    public Action OnDownloadStarted;

    /// <summary>
    /// Event called when progress changed
    /// </summary>
    public Action<int> OnProgressChanged;

    /// <summary>
    /// Event called when download has been completed
    /// </summary>
    public Action OnDownloadCompleted;

    /// <summary>
    /// Event called when download has been canceled
    /// </summary>
    public Action OnDownloadCanceled;

    /// <summary>
    /// Event called when download has failed
    /// </summary>
    public Action<long,string> OnDownloadFailure;

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

}