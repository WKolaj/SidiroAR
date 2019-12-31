﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    private ApplicationInitializer _appInitializer;
    /// <summary>
    /// Application initizer game object
    /// </summary>
    public ApplicationInitializer AppInitializer
    {
        get
        {
            return _appInitializer;
        }

        set
        {
            _appInitializer = value;
        }
    }


    [SerializeField]
    private GameObject _dialogBoxPrefab;
    /// <summary>
    /// Prefab of file browser
    /// </summary>
    public GameObject DialogBoxPrefab
    {
        get
        {
            return _dialogBoxPrefab;
        }

        set
        {
            _dialogBoxPrefab = value;
        }
    }

    [SerializeField]
    private GameObject _modelExplorerPrefab;
    /// <summary>
    /// Prefab of model explorer
    /// </summary>
    public GameObject ModelExplorerPrefab
    {
        get
        {
            return _modelExplorerPrefab;
        }

        set
        {
            _modelExplorerPrefab = value;
        }
    }

    [SerializeField]
    private GameObject _progressWindowPrefab;
    /// <summary>
    /// Prefab of progress window
    /// </summary>
    public GameObject ProgressWindowPrefab
    {
        get
        {
            return _progressWindowPrefab;
        }

        set
        {
            _progressWindowPrefab = value;
        }
    }


    [SerializeField]
    private GameObject _loginWindowPrefab;
    /// <summary>
    /// Prefab of loginWindow
    /// </summary>
    public GameObject LoginWindowPrefab
    {
        get
        {
            return _loginWindowPrefab;
        }

        set
        {
            _loginWindowPrefab = value;
        }
    }

    /// <summary>
    /// Actual dialog box - shown in order to check no to instantiate more than one dialog box at once
    /// </summary>
    private GameObject actualDialogBox = null;

    /// <summary>
    /// Actual model explorer - shown in order to check no to instantiate more than one model explorer at once
    /// </summary>
    private GameObject actualModelExplorer = null;

    /// <summary>
    /// Actual progress window - shown in order to check no to instantiate more than one model explorer at once
    /// </summary>
    private GameObject actualProgressWindow = null;

    /// <summary>
    /// Actual login window - shown in order to check no to instantiate more than one model explorer at once
    /// </summary>
    private GameObject actualLoginWindow = null;

    void Awake()
    {
    }

    /// <summary>
    /// Method for setting app initializer
    /// </summary>
    public void SetAppInitializer(ApplicationInitializer initializer)
    {
        //Has to be invoked separetelly - sometime before awake of main canvas (login window on beginin)
        AppInitializer = initializer;
    }

    /// <summary>
    /// Method for showing new dialog box
    /// </summary>
    /// <param name="titleText">
    /// Title of dialog box
    /// </param>
    /// <param name="contentText">
    /// Content of dialog box
    /// </param>
    /// <param name="mode">
    /// Mode of dialog box
    /// </param>
    /// <param name="type">
    /// Type of dialog box
    /// </param>
    /// <returns>
    /// Dialog box script
    /// </returns>
    public DialogBoxWindow ShowDialogBox(string titleText, string contentText, DialogBoxMode mode, DialogBoxType type)
    {
        if (actualDialogBox) Destroy(actualDialogBox);

        GameObject dialogBox = Instantiate(DialogBoxPrefab, transform);
        dialogBox.name = "DialogBox";

        actualDialogBox = dialogBox;

        DialogBoxWindow dialogBoxScript = dialogBox.GetComponent<DialogBoxWindow>();

        dialogBoxScript.SetMode(mode);
        dialogBoxScript.SetType(type);
        dialogBoxScript.SetTitle(titleText);
        dialogBoxScript.SetContent(contentText);

        return dialogBoxScript;
    }

    /// <summary>
    /// Method for showing model explorer
    /// </summary>
    /// <returns>
    /// Model explorer script
    /// </returns>
    public ModelExplorer OpenModelExplorer()
    {
        if (actualModelExplorer) Destroy(actualModelExplorer);

        GameObject modelExplorer = Instantiate(ModelExplorerPrefab, transform);
        modelExplorer.name = "modelExplorer";

        actualModelExplorer = modelExplorer;

        ModelExplorer modelExplorerScript = actualModelExplorer.GetComponent<ModelExplorer>();

        modelExplorerScript.Init(Common.DefaultUserDirPath);

        return modelExplorerScript;
    }

    /// <summary>
    /// Method for progress window
    /// </summary>
    public void ShowProgressWindow()
    {
        if (actualProgressWindow) Destroy(actualProgressWindow);

        GameObject progressWindow = Instantiate(ProgressWindowPrefab, transform);
        progressWindow.name = "ProgressWindow";

        actualProgressWindow = progressWindow;

    }


    /// <summary>
    /// Method for hiding progress window
    /// </summary>
    public void HideProgressWindow()
    {
        if (actualProgressWindow) Destroy(actualProgressWindow);

    }
    
    /// <summary>
    /// Method for showing login window
    /// </summary>
    public void ShowLoginWindow()
    {
        if (actualLoginWindow) Destroy(actualLoginWindow);

        GameObject loginWindow = Instantiate(this.LoginWindowPrefab, transform);
        loginWindow.name = "LoginWindow";

        LoginWindow loginWindowScript = loginWindow.GetComponent<LoginWindow>();
        loginWindowScript.Init(this,AppInitializer.Loader);

        actualLoginWindow = loginWindow;
    }

}
