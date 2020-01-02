using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchboardContainer : InitializableWithInitializerBase
{
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

    private GameObject itemsContainerGO;

    protected override void OnInitializeComponents()
    {
        var scrollViewGO = this.transform.Find("ScrollView").gameObject;
        this.itemsContainerGO = scrollViewGO.transform.Find("ItemsContainer").gameObject;
        this._mainCanvas = this.MainCanvasGO.GetComponent<MainCanvas>();
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
        ClearAllSwitchboardItems();

        foreach(var modelLoader in modelLoaders)
        {
            GameObject modelItem = Instantiate(SwitchboadItemPrefab, this.itemsContainerGO.transform);
            SwitchboardItem modelItemScript = modelItem.GetComponent<SwitchboardItem>();
            modelItemScript.Init(modelLoader,MainCanvas);
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
}
