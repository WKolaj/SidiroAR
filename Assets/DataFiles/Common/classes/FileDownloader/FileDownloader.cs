using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for downloading file
/// </summary>
public class FileDownloader : MonoBehaviour
{
    /// <summary>
    /// Flag used for stopping downloading coroutine
    /// </summary>
    private bool _stopDownloading = false;

    /// <summary>
    /// Timeout of downloading progress operation [s]
    /// </summary>
    private int _timeout = 10;
    public int Timeout
    {
        get
        {
            return _timeout;
        }

        private set
        {
            _timeout = value;
        }

    }

    /// <summary>
    /// Operation of downloading
    /// </summary>
    private Int32 _progress = 0;
    public Int32 Progress
    {
        get
        {
            return _progress;
        }

        private set
        {
            _progress = value;
        }
    }


    /// <summary>
    /// Operation of downloading
    /// </summary>
    private UnityWebRequestAsyncOperation _downloadingOperation = null;
    public UnityWebRequestAsyncOperation DownloadingOperation
    {
        get
        {
            return _downloadingOperation;
        }

        private set
        {
            _downloadingOperation = value;
        }
    }


    /// <summary>
    /// Web request of file downloading
    /// </summary>
    private UnityWebRequest _webRequest = null;
    public UnityWebRequest WebRequest
    {
        get
        {
            return _webRequest;
        }

        private set
        {
            _webRequest = value;
        }
    }

    /// <summary>
    /// URL to download
    /// </summary>
    private string _url = null;
    public string URL
    {
        get
        {
            return _url;
        }

        private set
        {
            this._url = value;
        }
    }

    /// <summary>
    /// Headers to append
    /// </summary>
    private Dictionary<string, string> _headers = new Dictionary<string, string>();
    public Dictionary<string, string> Headers
    {
        get
        {
            return _headers;
        }
    }

    /// <summary>
    /// Flag determing wether downlading is in progress
    /// </summary>
    private bool _isDownloading = false;
    public bool IsDownloading
    {
        get
        {
            return _isDownloading;
        }

        private set
        {
            _isDownloading = value;
        }
    }


    /// <summary>
    /// Method for setting url of file to download
    /// </summary>
    /// <param name="url"></param>
    public void SetURL(string url)
    {
        this.URL = url;
    }

    /// <summary>
    /// Method for setting
    /// </summary>
    /// <param name="key">
    /// Header name
    /// </param>
    /// <param name="value">
    /// Header value
    /// </param>
    public void SetHeader(string key, string value)
    {
        this.Headers[key] = value;
    }

    /// <summary>
    /// Method for timeout value
    /// </summary>
    /// <param name="timeout">
    /// Timeout value
    /// </param>
    public void SetTimeout(int timeout)
    {
        this.Timeout = timeout;
    }

    /// <summary>
    /// Method for starting downloading
    /// </summary>
    /// <param name="path">
    /// Path to file
    /// </param>
    /// <param name="onDownloadingStarted">
    /// Action called on downloading started
    /// </param>
    /// <param name="onDownloadingStopped">
    /// Action called on downloading stopped
    /// </param>
    /// <param name="onDownloadingProgress">
    /// Action called on downloading progress change
    /// </param>
    /// <param name="onDownloadingFinished">
    /// Action called on downloading finish
    /// </param>
    /// <param name="onError">
    /// Action called on downloading error (errorCode, errorMessage)
    /// </param>
    public void StartDownload(String path, Action onDownloadingStarted, Action onDownloadingStopped, Action<int> onDownloadingProgress, Action onDownloadingFinished, Action<long, string> onError)
    {
        if (!IsDownloading)
            StartCoroutine(_downloadFile(path, onDownloadingStarted, onDownloadingStopped, onDownloadingProgress, onDownloadingFinished, onError));
    }

    /// <summary>
    /// Method for stopping downloading
    /// </summary>
    public void StopDownloading()
    {
        if (IsDownloading) _stopDownloading = true;
    }

    /// <summary>
    /// Main method to download file asynchronously
    /// </summary>
    /// <param name="path">
    /// Path to file
    /// </param>
    /// <param name="onDownloadingStarted">
    /// Action called on downloading started
    /// </param>
    /// <param name="onDownloadingStopped">
    /// Action called on downloading stopped
    /// </param>
    /// <param name="onDownloadingProgress">
    /// Action called on downloading progress change
    /// </param>
    /// <param name="onDownloadingFinished">
    /// Action called on downloading finish
    /// </param>
    /// <param name="onError">
    /// Action called on downloading error (errorCode, errorMessage)
    /// </param>
    /// <returns>
    /// IEnumerator for Coroutine
    /// </returns>
    private IEnumerator _downloadFile(string path, Action onDownloadingStarted, Action onDownloadingStopped, Action<int> onDownloadingProgress, Action onDownloadingFinished, Action<long, string> onError)
    {
        //Creating and initializing web request
        using (this.WebRequest = new UnityWebRequest(this.URL, UnityWebRequest.kHttpVerbGET))
        {
            //Resetting progress
            this.Progress = 0;

            //Settting headers of request
            foreach (var headerKey in Headers.Keys)
            {
                this.WebRequest.SetRequestHeader(headerKey, Headers[headerKey]);
            }

            //Creating download handler
            using (this.WebRequest.downloadHandler = new DownloadHandlerFile(path))
            {
                IsDownloading = true;

                //Signalzing download started
                if (onDownloadingStarted != null)
                    onDownloadingStarted();

                //Timeout error flag
                bool timeoutError = false;
                long lastProgressChangeTime = DateTimeOffset.Now.ToUnixTimeSeconds();

                //Starting download
                DownloadingOperation = this.WebRequest.SendWebRequest();

                //Main loop - signaling new progress until download has been completed or error occurs
                while (!WebRequest.isDone)
                {
                    //Checking if downlading should be stopped
                    if (_stopDownloading)
                    {
                        //Stopping the download
                        this.WebRequest.Abort();

                        //breaking from loop
                        break;
                    }

                    yield return null;

                    //Checking timeout error
                    long newTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                    if (newTime - lastProgressChangeTime > Timeout)
                    {
                        timeoutError = true;

                        //Stopping the download
                        this.WebRequest.Abort();

                        //breaking from loop
                        break;
                    }

                    //Recalcuating progress
                    var newProgress = _calculateProgress(this.WebRequest.downloadProgress);

                    //Setting new progress and signaling - in case it's changed
                    if (newProgress != Progress)
                    {

                        //Setting new timeout time to check
                        lastProgressChangeTime = newTime;

                        Progress = newProgress;

                        if (onDownloadingProgress != null)
                            onDownloadingProgress(newProgress);
                    }
                }

                //Downloading ended
                IsDownloading = false;

                //Downloading ended - checking reason
                if (_stopDownloading)
                {
                    //Resetting the flag
                    _stopDownloading = false;

                    //Deleting downloaded file
                    try
                    {
                        if (File.Exists(path))
                            File.Delete(path);
                    }
                    catch (Exception err)
                    {
                        Debug.Log(err);
                    }

                    if (onDownloadingStopped != null)
                    {
                        onDownloadingStopped();
                    }

                }
                else if (timeoutError)
                {
                    //Deleting downloaded file
                    try
                    {
                        if (File.Exists(path))
                            File.Delete(path);
                    }
                    catch (Exception err)
                    {
                        Debug.Log(err);
                    }

                    if (onError != null)
                    {
                        onError(0, "Timeout error");
                    }
                }
                else if (this.WebRequest.isNetworkError)
                {
                    if (onError != null)
                    {
                        onError(0, "Network error");
                    }
                }
                else if (this.WebRequest.isHttpError)
                {
                    if (onError != null)
                    {
                        onError(this.WebRequest.responseCode, this.WebRequest.error);
                    }
                }
                else if (this.WebRequest.isDone)
                {
                    if (onDownloadingFinished != null)
                    {
                        onDownloadingFinished();
                    }
                }

            }

        }

        //Clearing memory
        this.WebRequest = null;


        //Sending ending progress
        Progress = 0;
        if (onDownloadingProgress != null)
            onDownloadingProgress(0);

    }

    /// <summary>
    /// Method for converting handler progress to int percentage
    /// </summary>
    /// <param name="handlerProgress">
    /// handler progress
    /// </param>
    /// <returns>
    /// progress in percentage
    /// </returns>
    private int _calculateProgress(float handlerProgress)
    {
        return Convert.ToInt32(Math.Round(handlerProgress * 100));
    }
}