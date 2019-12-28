using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModelExplorer : MonoBehaviour
{
    public event Action onCancelClicked;

    /// <summary>
    /// Prefab of modelItem
    /// </summary>
    [SerializeField]
    private GameObject _modelItemPrefab;
    public GameObject ModelItemPrefab
    {
        get
        {
            return _modelItemPrefab;
        }

        set
        {
            _modelItemPrefab = value;
        }
    }

    private GameObject headerTitleItem;
    private GameObject headerPathItem;
    private GameObject itemContainer;
    private GameObject cancelButton;

    /// <summary>
    /// Path of models directory
    /// </summary>
    private string _dirPath;
    public string DirPath
    {
        get
        {
            return _dirPath;
        }
    }

    /// <summary>
    /// All model creators read from directory
    /// </summary>
    private List<AssetModelCreator> _allCreators = null;
    public List<AssetModelCreator> AllCreators
    {
        get
        {
            return _allCreators;
        }
    }

    /// <summary>
    /// Method for initializing model explorer based on given directory path
    /// </summary>
    /// <param name="dirPath">
    /// Directory path
    /// </param>
    public void Init(string dirPath)
    {
        InitDirPath(dirPath);
        InitCreators();
    }

    /// <summary>
    /// Initializing directory path
    /// </summary>
    /// <param name="dirPath">
    /// Directory path
    /// </param>
    private void InitDirPath(string dirPath)
    {
        this._dirPath = dirPath;
        //Assigning path to label text
        headerPathItem.GetComponent<TextMeshProUGUI>().SetText(dirPath);
    }

    /// <summary>
    /// Initializing creator and model items in container
    /// </summary>
    private void InitCreators()
    {
        //Getting all possible creators from directory
        _allCreators = AssetModelCreator.GetCreatorsFromDirectory(DirPath);

        //Instantiating model items for each creator an assigning them
        foreach (var creator in AllCreators)
        {
            GameObject modelItem = Instantiate(ModelItemPrefab, itemContainer.transform);
            ModelItem modelItemScript = modelItem.GetComponent<ModelItem>();
            modelItemScript.AssignModelCreator(creator);
        }
    }

    /// <summary>
    /// Method for assigning all neccessary components
    /// </summary>
    private void InitComponents()
    {
        var header = transform.Find("Header").gameObject;

        headerTitleItem = header.transform.Find("HeaderTitle").gameObject;
        headerPathItem = header.transform.Find("HeaderPath").gameObject;

        var container = transform.Find("Container").gameObject;
        var scrollView = container.transform.Find("ScrollView").gameObject;

        itemContainer = scrollView.transform.Find("ItemsContainer").gameObject;

        var footer = transform.Find("Footer").gameObject;
        cancelButton = footer.transform.Find("CancelButton").gameObject;

        cancelButton.GetComponent<Button>().onClick.AddListener(HandleCancelButtonClicked);
    }

    /// <summary>
    /// Method for handling cancel button click
    /// </summary>
    private void HandleCancelButtonClicked()
    {
        if(onCancelClicked != null)
        {
            onCancelClicked();
        }

        Destroy(gameObject);
    }

    void Awake()
    {
        InitComponents();
    }

}
