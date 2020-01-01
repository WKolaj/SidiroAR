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

    private GameObject itemsContainerGO;

    protected override void OnInitializeComponents()
    {
        var scrollViewGO = this.transform.Find("ScrollView").gameObject;
        this.itemsContainerGO = scrollViewGO.transform.Find("ItemsContainer").gameObject;
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
            modelItemScript.Init(modelLoader);
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
