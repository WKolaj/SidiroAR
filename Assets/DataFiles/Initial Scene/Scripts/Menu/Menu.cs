using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : InitializableWithInitializerBase
{
    [SerializeField]
    private GameObject _mainCanvasGO;
    /// <summary>
    /// Main canvas Game Object - for running file browser
    /// </summary>
    public GameObject MainCanvasGO
    {
        get
        {
            return _mainCanvasGO;
        }

        set
        {
            _mainCanvasGO = value;
        }
    }

    [SerializeField]
    private Sprite _loginButtonIconPrefab;
    /// <summary>
    /// Login button icon
    /// </summary>
    public Sprite LoginButtonIconPrefab
    {
        get
        {
            return _loginButtonIconPrefab;
        }

        set
        {
            _loginButtonIconPrefab = value;
        }
    }

    [SerializeField]
    private Sprite _logoutButtonIconPrefab;
    /// <summary>
    /// Logout button icon
    /// </summary>
    public Sprite LogoutButtonIconPrefab
    {
        get
        {
            return _logoutButtonIconPrefab;
        }

        set
        {
            _logoutButtonIconPrefab = value;
        }
    }

    private MainCanvas _mainCanvas;
    /// <summary>
    /// Main canvas  - for running file browser
    /// </summary>
    public MainCanvas MainCanvas
    {
        get
        {
            return _mainCanvas;
        }

        set
        {
            _mainCanvas = value;
        }
    }


    private Animator slidingAnimator;
    private TextMeshProUGUI userNameLabel;
    private Image loginButtonIcon;
    private TextMeshProUGUI loginButtonLabel;

    protected override void OnInitializeComponents()
    {
        this.slidingAnimator = GetComponentInChildren<Animator>();
        MainCanvas = MainCanvasGO.GetComponent<MainCanvas>();

        var menuListMaskGO = this.transform.Find("MenuListMask").gameObject;
        var menuListGO = menuListMaskGO.transform.Find("MenuList").gameObject;

        var userItemGO = menuListGO.transform.Find("UserItem").gameObject;
        var userNameLabelGO = userItemGO.transform.Find("UserNameLabel").gameObject;
        this.userNameLabel = userNameLabelGO.GetComponent<TextMeshProUGUI>();

        var loginItemGO = menuListGO.transform.Find("LoginItem").gameObject;
        var loginButtonGO = loginItemGO.transform.Find("LoginButton").gameObject;
        loginButtonIcon = loginButtonGO.GetComponent<Image>();
        var loginLabelGO = loginItemGO.transform.Find("LoginLabel").gameObject;
        loginButtonLabel = loginLabelGO.GetComponent<TextMeshProUGUI>();

    }

    /// <summary>
    /// Causes slider to slide out
    /// </summary>
    public void SlideOut()
    {
        this.slidingAnimator.SetBool("SlidingIn", false);
    }

    /// <summary>
    /// Causes slider to slide in
    /// </summary>
    public void SlideIn()
    {
        this.slidingAnimator.SetBool("SlidingIn", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method for loading file
    /// </summary>
    public void LoadFromFile()
    {
        var explorer = MainCanvas.OpenModelExplorer();
    }

    /// <summary>
    /// Method for handling closing of file browser when file is selected
    /// </summary>
    /// <param name="filePath">
    /// Path to selected file
    /// </param>
    private void HandleFileBrowserFileSelected(string filePath)
    {
        Debug.Log(filePath);
    }

    /// <summary>
    /// Method for handling click of user button
    /// </summary>
    public void HandleUserButtonClicked()
    {
        if(UserLoader.LoggedUser == null)
        {
            MainCanvas.ShowLoginWindow();
        }
    }

    /// <summary>
    /// Method for handling click of logout button
    /// </summary>
    public void HandleLoginButtonClicked()
    {
        if (UserLoader.LoggedUser != null)
        {
            var dialogBoxResult = MainCanvas.ShowDialogBox("Wylogowanie", "Czy na pewno chcesz się wylogować?", DialogBoxMode.Question, DialogBoxType.YesNo);
            dialogBoxResult.onYesClicked += new Action(() =>
            {
                MainCanvas.Initalizer.Loader.LogoutUser();
                MainCanvas.ShowLoginWindow();
                this.SlideOut();
                
            });
        }
        else
        {
            MainCanvas.ShowLoginWindow();
            this.SlideOut();
        }

    }

    /// <summary>
    /// Method for refreshing display of all user elements
    /// </summary>
    /// <param name="newUser">
    /// Object of new user
    /// </param>
    public void RefreshUserDisplay(User newUser)
    {
        if(newUser != null)
        {
            this.userNameLabel.SetText(newUser.Name);
            this.loginButtonIcon.sprite = this.LogoutButtonIconPrefab;
            this.loginButtonLabel.SetText("Wyloguj");
        }
        else
        {
            this.userNameLabel.SetText(string.Empty);
            this.loginButtonIcon.sprite = this.LoginButtonIconPrefab;
            this.loginButtonLabel.SetText("Zaloguj");
        }
    }

}
