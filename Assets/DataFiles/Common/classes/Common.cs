using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Common
{

    /// <summary>
    /// Name of directory for app data
    /// </summary>
    private static string dataDirName = "data";

    /// <summary>
    /// Name for directory inside app data dir to store models
    /// </summary>
    private static string modelsDirName = "models";

    /// <summary>
    /// Path to applicaton files directory
    /// </summary>
    public static string AppDirPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, dataDirName);
        }
    }

    /// <summary>
    /// Path to models directory
    /// </summary>
    public static string ModelsDirPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, dataDirName, modelsDirName);
        }
    }

    /// <summary>
    /// Path for presented model - used to connect two scenes - initial and AR - together
    /// </summary>
    private static string _modelPath = string.Empty;
    public static string ModelPath
    {
        get
        {
            return _modelPath;
        }

        set
        {
            _modelPath = value;
        }
    }

    /// <summary>
    /// Scale of presented model - set in player prefs in order to be permantent
    /// </summary>
    public static float Scale
    {
        get
        {
            //Returning value from model scale or 1.0 as default value if it was not set before
            if(!PlayerPrefs.HasKey("ModelScale"))
                PlayerPrefs.SetFloat("ModelScale", 1);

                return PlayerPrefs.GetFloat("ModelScale");
        }

        set
        {
            PlayerPrefs.SetFloat("ModelScale", value);
        }
    }

    public static void LoadARScene()
    {
        SceneManager.LoadScene("ARScene");
    }

    public static void LoadInitialScene()
    {
        SceneManager.LoadScene("InitialScene");
    }

    public static void QuitApp()
    {
        Application.Quit();
    }

    /// <summary>
    /// Method for generating random file name for download
    /// </summary>
    /// <returns></returns>
    public static string GenerateRandomTemporaryFilePath()
    {
        return Path.Combine(Application.temporaryCachePath, Path.GetRandomFileName());
    }

    public static void DispatchInMainThread(Action action)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(action);
    }

    /// <summary>
    /// Method for running code
    /// </summary>
    /// <param name="androidCode"></param>
    /// <param name="iosCode"></param>
    /// <returns></returns>
    public static object RunplatformDependendCode(Func<object> androidCode, Func<object> iosCode)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    return androidCode();
                }

            case RuntimePlatform.IPhonePlayer:
                {
                    return iosCode();
                }

            case RuntimePlatform.WindowsEditor:
                {
                    return androidCode();
                }

            case RuntimePlatform.LinuxEditor:
                {
                    return androidCode();
                }

            case RuntimePlatform.OSXEditor:
                {
                    return androidCode();
                }

            default:
                {
                    throw new InvalidProgramException(String.Format("Unsupported platform :{0}", Application.platform));
                }
        }

    }

    /// <summary>
    /// Method for getting response error code of error. returning -1 if error is not a web error
    /// </summary>
    /// <returns>
    /// Respone code or -1 if invalid error type
    /// </returns>
    public static int getResponseCodeOfError(Exception err)
    {
        try
        {
            var message = err.Message;

            //If error is a net error - retrieve and return code, return -1 if not
            if (err is System.Net.WebException)
            {
                var webErr = (System.Net.WebException)err;

                using (WebResponse response = webErr.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;

                    return (int)httpResponse.StatusCode;
                }
            }
            else
            {
                return -1;
            }
        }
        catch(Exception)
        {
            return -1;
        }
    }

    /// <summary>
    /// Method for getting network error reason. Return unknown error if it is a valid web error
    /// </summary>
    /// <returns>
    /// Web error type or unknown error if it is not a web error type
    /// </returns>
    public static WebExceptionStatus getWebErrorType(Exception err)
    {
        try
        {
            var message = err.Message;

            //If error is a net error - retrieve and return code, return -1 if not
            if (err is System.Net.WebException)
            {
                var webErr = (System.Net.WebException)err;

                return webErr.Status;
            }
            //returning timeout error in case WebClientWithTimeout throws timeout exception
            else if (err is TimeoutException)
            {
                return WebExceptionStatus.Timeout;
            }
            else
            {
                return WebExceptionStatus.UnknownError;
            }
        }
        catch (Exception)
        {
            return WebExceptionStatus.UnknownError;
        }
    }


    /// <summary>
    /// Method for getting errorCode for translation from network error.
    /// Returning error message if error is no an network error
    /// </summary>
    /// <param name="err">
    /// Error to check
    /// </param>
    /// <returns>
    /// ErrorCode for translation from network error.
    /// Returning error message if error is no an network error
    /// </returns>
    public static string getNetworkErrorTextCode(Exception err)
    {
        //Checking if error reason is a connection timeout
        var errorType = getWebErrorType(err);

        if (errorType == WebExceptionStatus.Timeout || 
            errorType == WebExceptionStatus.ConnectFailure || 
            errorType == WebExceptionStatus.ReceiveFailure || 
            errorType == WebExceptionStatus.NameResolutionFailure)
        {
            return "ConnectionError";
        }
        else
        {
            //Getting response code from error
            var responseCode = getResponseCodeOfError(err);

            if(responseCode >= 0)
            {
                return String.Format("HttpResponseErrorCode{0}",responseCode);
            }
        }

        return err.Message;
    }

    /// <summary>
    /// Name for directory inside models directory to store models for default user
    /// </summary>
    private static string defaultUserDirName = "_default";

    /// <summary>
    /// Path to default user directory
    /// </summary>
    public static string DefaultUserDirPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, dataDirName, modelsDirName, defaultUserDirName);
        }
    }

}