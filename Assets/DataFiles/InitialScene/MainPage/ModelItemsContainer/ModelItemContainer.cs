using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ModelItemContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject _modelItemPrefab = null;
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

    private List<ModelItem> _instantiatedModels = new List<ModelItem>();
    public List<ModelItem> InstantiatedModels
    {
        get
        {
            return _instantiatedModels;
        }
    }

    private GameObject _itemContainerGO;

    /// <summary>
    /// Main scroll view 
    /// </summary>
    private ScrollRect _scrollView;

    /// <summary>
    /// Item for checking position of scroll view in order to implement refresh - outside scroll area
    /// </summary>
    private GameObject _scrollViewTopLimit;

    /// <summary>
    /// Item for checking position of scroll view in order to implement refresh - inside scroll area
    /// </summary>
    private GameObject _scrollViewPositionCheckItem;

    private void Awake()
    {
        var scrollViewGO = this.transform.Find("ScrollView").gameObject;
        this._itemContainerGO = scrollViewGO.transform.Find("ItemsContainer").gameObject;

        this._scrollView = scrollViewGO.GetComponent<ScrollRect>();

        //Items for detecting position of scroll bar
        this._scrollViewPositionCheckItem = this._itemContainerGO.transform.Find("ScrollViewPositionCheckItem").gameObject;
        this._scrollViewTopLimit = scrollViewGO.transform.Find("ScrollViewTopLimit").gameObject;

        this._scrollView.onValueChanged.AddListener(_handleScrollValueChanged);
    }

    private void Start()
    {
        if(UserLoader.LoggedUser != null)
        {
            foreach(var model in UserLoader.LoggedUser.ModelList)
            {
                _createAndAddModelItem(model);
            }
        }
    }

    /// <summary>
    /// Method used for refreshing model item container with MainCanvas
    /// </summary>
    public void RefreshDataDisplay()
    {
        _refreshModelList();
    }

    /// <summary>
    /// Method called when scroll view values changes
    /// </summary>
    /// <param name="pos"></param>
    private async void _handleScrollValueChanged(Vector2 pos)
    {
        //Calculating differance between element
        var differance = this._scrollViewTopLimit.transform.position.y - this._scrollViewPositionCheckItem.transform.position.y;

        //Firing scroll refresh if elements move is greater than limit
        if (differance > 30.0) await _handleScrollRefresh();
    }

    private async Task _handleScrollRefresh()
    {
        await MainCanvas.RefreshUserDataFromServer();
    }

    private void _createAndAddModelItem(AssetModelLoader model)
    {
        var newModelItemGO = GameObject.Instantiate(ModelItemPrefab, _itemContainerGO.transform);

        var newModelItem = newModelItemGO.GetComponent<ModelItem>();
        newModelItem.AssignModel(model);

        this.InstantiatedModels.Add(newModelItem);

    }
    private void _deleteModelItem(ModelItem modelItem)
    {
        InstantiatedModels.Remove(modelItem);
        GameObject.Destroy(modelItem.gameObject);
    }

    private void _refreshModelList()
    {
        List<ModelItem> _itemsToDelete = new List<ModelItem>();
        List<AssetModelLoader> _modelsToAdd = new List<AssetModelLoader>();

        if (UserLoader.LoggedUser != null)
        {
            //If model has new parameters - it is instantiated as new object, so reference comparision is sufficient
            foreach(var model in UserLoader.LoggedUser.ModelList)
            {
                //Adding all models to models to add that are not present in instantiated model items
                if(!InstantiatedModels.Exists((item)=>item.Model == model))
                    _modelsToAdd.Add(model);
            }

            //Adding all modelItem which models are not presented in current user as models to delete
            foreach (var item in InstantiatedModels)
            {
                if (!UserLoader.LoggedUser.ModelList.Contains(item.Model))
                    _itemsToDelete.Add(item);
            }
        }
        else
        {
            //All models should be deleted
            foreach(var modelItem in InstantiatedModels)
            {
                _itemsToDelete.Add(modelItem);
            }
        }

        //Deleting all models to delete
        foreach(var item in _itemsToDelete)
        {
            _deleteModelItem(item);
        }

        //Adding all new models
        foreach (var model in _modelsToAdd)
        {
            _createAndAddModelItem(model);
        }


    }

}
