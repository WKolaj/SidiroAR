using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{

    /// <summary>
    /// Prefab used for showing login page
    /// </summary>
    [SerializeField]
    private GameObject _loginPagePrefab;
    public GameObject LoginPagePrefab
    {
        get
        {
            return _loginPagePrefab;
        }

        set
        {
            _loginPagePrefab = value;
        }
    }

    /// <summary>
    /// Main, actual canvas
    /// </summary>
    private static MainCanvas _actualMainCanvas = null;

    /// <summary>
    /// Method for showing menu
    /// </summary>
    public static void ShowMenu()
    {
        _actualMainCanvas._menu.DrawOut();
    }

    /// <summary>
    /// Method for hiding menu
    /// </summary>
    public static void HideMenu()
    {
        _actualMainCanvas._menu.DrawIn();
    }

    /// <summary>
    /// Method for showing loading page
    /// </summary>
    public static void ShowLoadingPage()
    {
        _actualMainCanvas._loadingPage.Show();
    }

    /// <summary>
    /// Method for hiding loading page
    /// </summary>
    public static void HideLoadingPage()
    {
        _actualMainCanvas._loadingPage.Hide();
    }

    /// <summary>
    /// Method for showing auxilliary page. If content is the same as exiting - it won't be instantiate again
    /// </summary>
    /// <param name="pageContent">
    /// Content to show
    /// </param>
    public static void ShowAuxPage(GameObject pageContent)
    {
        _actualMainCanvas._auxPageContainer.DrawOut(pageContent);
    }

    /// <summary>
    /// Method for showing auxilliary page instantly. If content is the same as exiting - it won't be instantiate again. Used to present login window on the begining if user is not logged in
    /// </summary>
    /// <param name="pageContent">
    /// Content to show
    /// </param>
    public static void ShowAuxPageInstantly(GameObject pageContent)
    {
        _actualMainCanvas._auxPageContainer.DrawOutInstantly(pageContent);
    }

    /// <summary>
    /// Method for hiding auxiliary page
    /// </summary>
    public static void HideAuxPage()
    {
        _actualMainCanvas._auxPageContainer.DrawIn();
    }

    /// <summary>
    /// Method for shoing login page
    /// </summary>
    public static void ShowLoginPage()
    {
        ShowAuxPage(_actualMainCanvas.LoginPagePrefab);
    }

    /// <summary>
    /// Method for showing login page instantly - without animation
    /// </summary>
    public static void ShowLoginPageInstantly()
    {
        ShowAuxPageInstantly(_actualMainCanvas.LoginPagePrefab);
    }

    /// <summary>
    /// Method for showing dialog window
    /// </summary>
    /// <param name="windowText">
    /// Text embeded in window
    /// </param>
    /// <param name="button0Text">
    /// first button text
    /// </param>
    /// <param name="button0Method">
    /// method connected to first button
    /// </param>
    /// <param name="button0Color">
    /// color of first button
    /// </param>
    /// <param name="button1Text">
    /// second button text
    /// </param>
    /// <param name="button1Method">
    /// method connected to second button
    /// </param>
    /// <param name="button1Color">
    /// color of second button
    /// </param>
    public static void ShowDialogWindow(string windowText = "", string button0Text = null, Action button0Method = null, string button0Color = "#FFFF0266", string button1Text = null, Action button1Method = null, string button1Color = "#FF3EACAB")
    {
        _actualMainCanvas._dialogWindow.Show(windowText, button0Text, button0Method, button0Color, button1Text, button1Method, button1Color);
    }

    /// <summary>
    /// Method for hiding dialog window
    /// </summary>
    public static void HideDialogWindow()
    {
        _actualMainCanvas._dialogWindow.Hide();
    }
    
    /// <summary>
    /// Method for showing language window
    /// </summary>
    public static void ShowLanguageWindow()
    {
        //Getting translations of window components
        var translationWindowContentText = Translator.GetTranslation("TranslationWindow.ContentText");
        var translationWindowPolishButtonText = Translator.GetTranslation("TranslationWindow.PolishButtonText");
        var translationWindowEnglishButtonText = Translator.GetTranslation("TranslationWindow.EnglishButtonText");

        MainCanvas.ShowDialogWindow(
            translationWindowContentText,
            translationWindowPolishButtonText,
            () => { Translator.SetLang("pl"); HideMenu(); },
            "#41ABAB",
            translationWindowEnglishButtonText,
            () => { Translator.SetLang("en"); HideMenu(); },
            "#41ABAB");
    }

    /// <summary>
    /// Method for showing error window
    /// </summary>
    /// <param name="errorText">
    /// Error text to display
    /// </param>
    /// <param name="variant">
    /// Variant of call - eg. different translation of error depedining on situation
    /// </param>
    public static void ShowErrorWindow(string errorText, string variant)
    {
        var totalErrorKey = String.Format("Errors.{0}.{1}", variant, errorText);

        var dialogText = Translator.GetTranslation(totalErrorKey);

        //Checking if text was translated - setting original error if not
        if (dialogText == totalErrorKey) dialogText = errorText;

        //Getting translations of window components
        var okButtonText = Translator.GetTranslation("ErrorDialogWindow.OkButtonText");

        //Showing window
        MainCanvas.ShowDialogWindow(
            dialogText,
            okButtonText,
            () => {  },
            "#FF0266");
    }

    /// <summary>
    /// Method for showing window for deleting models file
    /// </summary>
    public static void ShowDeleteModelFileWindow(ModelItem assetModelItem)
    {
        //Getting translations of window components
        var windowContentText = Translator.GetTranslation("DeleteModelFileWindow.ContentText");
        var button0ButtonText = Translator.GetTranslation("DeleteModelFileWindow.YesButtonText");
        var button1ButtonText = Translator.GetTranslation("DeleteModelFileWindow.NoButtonText");

        var fullWindowContentText = String.Format("{0} {1}?", windowContentText, assetModelItem.Model.ModelName);

        MainCanvas.ShowDialogWindow(
            fullWindowContentText,
            button0ButtonText,
            () => {

                try
                {
                    assetModelItem.Model.DeleteModelFileIfExists();
                    assetModelItem.RefreshDataDisplay();
                }
                catch(Exception err)
                {
                    ShowErrorWindow(err.Message, "DeletingModelFile");
                }
            },
            "#FF0266",
            button1ButtonText,
            () => {  },
            "#3EACAB");
    }

    /// <summary>
    /// Method for showing window for stopping model downloading
    /// </summary>
    public static void ShowStopModelDownloadingWindow(ModelItem assetModelItem)
    {
        //Getting translations of window components
        var windowContentText = Translator.GetTranslation("StopModelDownloading.ContentText");
        var button0ButtonText = Translator.GetTranslation("StopModelDownloading.YesButtonText");
        var button1ButtonText = Translator.GetTranslation("StopModelDownloading.NoButtonText");

        var fullWindowContentText = String.Format("{0} {1}?", windowContentText, assetModelItem.Model.ModelName);

        MainCanvas.ShowDialogWindow(
            fullWindowContentText,
            button0ButtonText,
            () =>
            {
                try
                {
                    assetModelItem.Model.StopDownload();
                    assetModelItem.RefreshDataDisplay();
                }
                catch (Exception err)
                {
                    ShowErrorWindow(err.Message, "StoppingModelDownloading");
                }

            },
            "#FF0266",
            button1ButtonText,
            () => { },
            "#3EACAB");
    }

    /// <summary>
    /// Method for showing error window while downloading file
    /// </summary>
    public static void ShowDownloadingErrorWindow()
    {
        ShowErrorWindow("DownloadingError", "DownloadingError");
    }

    /// <summary>
    /// Method for asynchronously log in a user
    /// </summary>
    /// <param name="userEmail">
    /// user email
    /// </param>
    /// <param name="userPassword">
    /// user password
    /// </param>
    /// <returns>
    /// Has user been logged in successfully
    /// </returns>
    public async static Task<bool> AsyncTryLogIn(string userEmail, string userPassword)
    {
        try
        {
            ShowLoadingPage();
            await _actualMainCanvas.Loader.LoginUserFromServer(userEmail, userPassword);
            HideLoadingPage();

            RefreshModelItemsContainerDisplay();
            //Returning true - user logged in succesfully
            return true;
        }
        catch (Exception err)
        {
            //Getting error message
            //If error is known network error - it will be replaced with translation
            //Otherwise whole error text will be displayed
            var message = Common.getNetworkErrorTextCode(err);

            HideLoadingPage();

            //Variant - LogginIn
            ShowErrorWindow(message, "LoggingIn");

            //Returning false - user logged in succesfully
            return false;
        }
    }

    /// <summary>
    /// Method for getting users data from server and refreshing data based on gathered data from server
    /// </summary>
    public async static Task RefreshUserDataFromServer()
    {
        Common.DispatchInMainThread(() =>
        {
            ShowLoadingPage();
        });

        try
        {
            await UserLoader.LoggedUser.RefreshDataFromServer();
        }
        catch (Exception err)
        {
            Common.DispatchInMainThread(() =>
            {
                //Getting error message
                //If error is known network error - it will be replaced with translation
                //Otherwise whole error text will be displayed
                var message = Common.getNetworkErrorTextCode(err);

                HideLoadingPage();

                //Variant - LogginIn
                ShowErrorWindow(message, "RefreshingData");

            });
        }

        Common.DispatchInMainThread(() =>
        {
            HideLoadingPage();
            RefreshModelItemsContainerDisplay();
        });
    }

    /// <summary>
    /// Method used for refreshing model item container data
    /// </summary>
    public static void RefreshModelItemsContainerDisplay()
    {
        MainCanvas._actualMainCanvas._modelItemContainer.RefreshDataDisplay();
    }

    /// <summary>
    /// Method for logging out a user
    /// </summary>
    public static void LogOutUser()
    {
        _actualMainCanvas.Loader.LogoutUser();
    }

    private UserLoader _userLoader = new UserLoader();
    /// <summary>
    /// Object of user loader - for creating and managing users
    /// </summary>
    public UserLoader Loader
    {
        get
        {
            return _userLoader;
        }
        set
        {
            _userLoader = value;
        }
    }

    /// <summary>
    /// Reference to main menu
    /// </summary>
    private MainMenu _menu = null;

    /// <summary>
    /// Reference to loading page
    /// </summary>
    private LoadingPage _loadingPage = null;

    /// <summary>
    /// Refernce to auxilliary page container
    /// </summary>
    private AuxPageContainer _auxPageContainer = null;

    /// <summary>
    /// Reference to dialog window
    /// </summary>
    private DialogWindow _dialogWindow = null;

    /// <summary>
    /// Reference to model item container
    /// </summary>
    private ModelItemContainer _modelItemContainer = null;

    /// <summary>
    /// Method called on application start
    /// </summary>
    private void Awake()
    {
        MainCanvas._actualMainCanvas = this;

        this._dialogWindow = this.GetComponentInChildren<DialogWindow>(true);
        this._menu = this.GetComponentInChildren<MainMenu>(true);
        this._loadingPage = this.GetComponentInChildren<LoadingPage>(true);
        this._auxPageContainer = this.GetComponentInChildren<AuxPageContainer>(true);
        this._modelItemContainer = this.GetComponentInChildren<ModelItemContainer>(true);


        //Enable screen dimming
        Screen.sleepTimeout = SleepTimeout.SystemSetting;

    }

    /// <summary>
    /// Method called after awake - for application initialization
    /// </summary>
    private void Start()
    {
        _initApp();
    }

    /// <summary>
    /// Method for initializing application
    /// </summary>
    private void _initApp()
    {
        //Create directories if not exist
        if (!Directory.Exists(Common.AppDirPath)) Directory.CreateDirectory(Common.AppDirPath);

        //Create directories if not exist
        if (!Directory.Exists(Common.ModelsDirPath)) Directory.CreateDirectory(Common.ModelsDirPath);

        //Trying loading user from prefs
        var successfullyLogIn = Loader.LoginUserFromPlayerPrefs();

        //Showing log in page instantly if logging was unsuccessful
        if (!successfullyLogIn) ShowLoginPageInstantly();
        else RefreshModelItemsContainerDisplay();

    }

    /// <summary>
    /// Method called every update of UI
    /// </summary>
    private void Update()
    {
        //Showing page for user to log in - if any user is not logged in
        _showLoginPageIfUserIsNotLoggedIn();

    }

    /// <summary>
    /// Method for showing login page if user is not logged in
    /// </summary>
    private void _showLoginPageIfUserIsNotLoggedIn()
    {
        if (UserLoader.LoggedUser == null)
        {
            //Showing page in every update does not make creation of new content of auxPageContainer - it checks wether new content is equal to acutal and prevent creation new instance in this case
            ShowLoginPage();
        }
    }

}
