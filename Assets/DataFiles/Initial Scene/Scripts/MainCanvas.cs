using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject _appInitializerGO;
    /// <summary>
    /// Application initizer game object
    /// </summary>
    public GameObject AppInitializerGO
    {
        get
        {
            return _appInitializerGO;
        }

        set
        {
            _appInitializerGO = value;
        }
    }

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

    /// <summary>
    /// Actual dialog box - shown in order to check no to instantiate more than one dialog box at once
    /// </summary>
    private GameObject actualDialogBox = null;

    /// <summary>
    /// Actual model explorer - shown in order to check no to instantiate more than one model explorer at once
    /// </summary>
    private GameObject actualModelExplorer = null;

    void Awake()
    {
        InitAppInitializer();

    }

    private void InitAppInitializer()
    {
        AppInitializer = AppInitializerGO.GetComponent<ApplicationInitializer>();
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
    public DialogBox ShowDialogBox(string titleText, string contentText, DialogBoxMode mode, DialogBoxType type)
    {
        if (actualDialogBox) Destroy(actualDialogBox);

        GameObject dialogBox = Instantiate(DialogBoxPrefab, transform);
        dialogBox.name = "DialogBox";

        actualDialogBox = dialogBox;

        DialogBox dialogBoxScript = dialogBox.GetComponent<DialogBox>();

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

        modelExplorerScript.Init(AppInitializer.DefaultUserDirPath);

        return modelExplorerScript;
    }
}
