using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public static void ShowLoadingPage()
    {
        _actualMainCanvas._loadingPage.Show();
    }

    public static void HideLoadingPage()
    {
        _actualMainCanvas._loadingPage.Hide();
    }

    public static void ShowAuxPage(GameObject pageContent)
    {
        _actualMainCanvas._auxPageContainer.DrawOut(pageContent);
    }

    public static void ShowAuxPageInstantly(GameObject pageContent)
    {
        _actualMainCanvas._auxPageContainer.DrawOutInstantly(pageContent);
    }


    public static void HideAuxPage()
    {
        _actualMainCanvas._auxPageContainer.DrawIn();
    }

    public static void ShowLoginPage()
    {
        ShowAuxPage(_actualMainCanvas.LoginPagePrefab);
    }

    public static void ShowLoginPageInstantly()
    {
        ShowAuxPageInstantly(_actualMainCanvas.LoginPagePrefab);
    }

    public static void ShowDialogWindow(string windowText = "", string button0Text = null, Action button0Method = null, string button0Color = "#FFFF0266", string button1Text = null, Action button1Method = null, string button1Color = "#FF3EACAB")
    {
        _actualMainCanvas._dialogWindow.Show(windowText, button0Text, button0Method, button0Color, button1Text, button1Method, button1Color);
    }

    public static void HideDialogWindow()
    {
        _actualMainCanvas._dialogWindow.Hide();
    }
    
    public static void ShowLanguageWindow()
    {
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

    public static void ShowErrorWindow(string errorText)
    {
        MainCanvas.ShowDialogWindow(
            errorText,
            "OK",
            () => {  },
            "#41ABAB");
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

    private Menu _menu = null;
    private LoadingPage _loadingPage = null;
    private AuxPageContainer _auxPageContainer = null;
    private DialogWindow _dialogWindow = null;

    private void Awake()
    {
        MainCanvas._actualMainCanvas = this;

        this._menu = this.GetComponentInChildren<Menu>(true);
        this._loadingPage = this.GetComponentInChildren<LoadingPage>(true);
        this._auxPageContainer = this.GetComponentInChildren<AuxPageContainer>(true);
        this._dialogWindow = this.GetComponentInChildren<DialogWindow>(true);

    }

    private void Start()
    {
        InitApp();
    }

    private void Update()
    {
        _showLoginPageIfUserIsNotLoggedIn();

    }

    private void _showLoginPageIfUserIsNotLoggedIn()
    {
        if (UserLoader.LoggedUser == null)
        {
            ShowLoginPage();
        }
    }

    /// <summary>
    /// Method for initializing application
    /// </summary>
    private void InitApp()
    {
        //Create directories if not exist
        if (!Directory.Exists(Common.AppDirPath)) Directory.CreateDirectory(Common.AppDirPath);

        //Create directories if not exist
        if (!Directory.Exists(Common.ModelsDirPath)) Directory.CreateDirectory(Common.ModelsDirPath);

        //Trying loading user from prefs
        var successfullyLogIn = Loader.LoginUserFromPlayerPrefs();

        //Showing log in page instantly if logging was unsuccessful
        if (!successfullyLogIn) ShowLoginPageInstantly();
    }

    /// <summary>
    /// Method for asynchronusly try to log in
    /// </summary>
    /// <returns>
    /// user successfully logged in
    /// </returns>
    public async static Task<bool> AsyncTryLogIn(string userEmail, string userPassword)
    {
        try
        {
            ShowLoadingPage();
            await _actualMainCanvas.Loader.LoginUserFromServer(userEmail, userPassword);
            HideLoadingPage();

            return true;
        }
        catch (Exception err)
        {
            HideLoadingPage();
            ShowErrorWindow(err.Message);

            return false;
        }
    }

    public static void LogOutUser()
    {
        _actualMainCanvas.Loader.LogoutUser();
    }

}
