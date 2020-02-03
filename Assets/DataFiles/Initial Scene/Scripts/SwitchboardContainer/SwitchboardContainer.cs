using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SwitchboardContainer : InitializableWithInitializerBase
{
    /// <summary>
    /// Element showing if continer items are being loaded from server
    /// </summary>
    private bool itemsLoading = false;


    /// <summary>
    /// Prefab of switchboard item
    /// </summary>
    [SerializeField]
    private GameObject _switchboardItemPrefab;
    public GameObject SwitchboadItemPrefab
    {
        get
        {
            return _switchboardItemPrefab;
        }

        set
        {
            _switchboardItemPrefab = value;
        }
    }

    /// <summary>
    /// Main canvas game object
    /// </summary>
    [SerializeField]
    private GameObject _mainCanvasGO;
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

    /// <summary>
    /// Main canvas 
    /// </summary>
    private MainCanvas _mainCanvas;
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

    /// <summary>
    /// Main scroll view 
    /// </summary>
    private ScrollRect _scrollView;
    public ScrollRect ScrollView
    {
        get
        {
            return _scrollView;
        }
        
    }

    /// <summary>
    /// Item for checking position of scroll view in order to implement refresh - outside scroll area
    /// </summary>
    private GameObject _scrollViewTopLimit;
    public GameObject ScrollViewTopLimit
    {
        get
        {
            return _scrollViewTopLimit;
        }

    }

    /// <summary>
    /// Item for checking position of scroll view in order to implement refresh - inside scroll area
    /// </summary>
    private GameObject _scrollViewPositionCheckItem;
    public GameObject ScrollViewPositionCheckItem
    {
        get
        {
            return _scrollViewPositionCheckItem;
        }

    }

    private GameObject itemsContainerGO;

    protected override void OnInitializeComponents()
    {
        var scrollViewGO = this.transform.Find("ScrollView").gameObject;
        this.itemsContainerGO = scrollViewGO.transform.Find("ItemsContainer").gameObject;
        this._scrollView = scrollViewGO.GetComponent<ScrollRect>();
        this._mainCanvas = this.MainCanvasGO.GetComponent<MainCanvas>();

        //Items for detecting position of scroll bar
        this._scrollViewPositionCheckItem = this.itemsContainerGO.transform.Find("ScrollViewPositionCheckItem").gameObject;
        this._scrollViewTopLimit = this.ScrollView.transform.Find("ScrollViewTopLimit").gameObject;

        this.ScrollView.onValueChanged.AddListener(HandleScrollValueChanged);
    }

    /// <summary>
    /// Method called when scroll view values changes
    /// </summary>
    /// <param name="pos"></param>
    void HandleScrollValueChanged(Vector2 pos)
    {
        //Calculating differance between element
        var differance = this.ScrollViewTopLimit.transform.position.y - this.ScrollViewPositionCheckItem.transform.position.y;

        //Firing scroll refresh if elements move is greater than limit
        if (differance > 30.0) HandleScrollRefresh();
    }

    /// <summary>
    /// Method for destroying all switchboard items
    /// </summary>
    private void ClearAllSwitchboardItems()
    {
        var allSwitchboardItems = gameObject.transform.GetComponentsInChildren<SwitchboardItem>();

        //Clearing all elements
        foreach (var switchboadItem in allSwitchboardItems)
        {
            GameObject.Destroy(switchboadItem.gameObject);
        }

    }

    private void AssignSwitchboardItems(List<AssetModelLoader> modelLoaders)
    {
        //Finding models to delete and to add
        List<SwitchboardItem> itemsToAdd = new List<SwitchboardItem>();
        List<SwitchboardItem> itemsToDelete = new List<SwitchboardItem>();
        List<SwitchboardItem> actualItems = new List<SwitchboardItem>(this.GetComponentsInChildren<SwitchboardItem>());

        //Finding items to add and add them
        foreach(var modelLoader in modelLoaders)
        {
            //Checking if model exists in given item
            var itemWithModelExists = actualItems.Exists((item) => item.ModelLoader == modelLoader);
            if (!itemWithModelExists)
            {
                GameObject modelItem = Instantiate(SwitchboadItemPrefab, this.itemsContainerGO.transform);
                SwitchboardItem modelItemScript = modelItem.GetComponent<SwitchboardItem>();
                modelItemScript.Init(modelLoader, MainCanvas);
                itemsToAdd.Add(modelItemScript);
            }
        }

        //Finding items to delete and delete them
        foreach (var item in actualItems)
        {
            if (!modelLoaders.Exists(model => model == item.ModelLoader))
                GameObject.Destroy(item.gameObject);
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
        if(newUser !=null)
        {
            AssignSwitchboardItems(newUser.ModelList);
        }
        else
        {
            ClearAllSwitchboardItems();
        }
    }

    /// <summary>
    /// Method for refreshing elements of scroll view
    /// </summary>
    private async void HandleScrollRefresh()
    {
        await MainCanvas.RefreshUserDataFromServer();
    }
}
