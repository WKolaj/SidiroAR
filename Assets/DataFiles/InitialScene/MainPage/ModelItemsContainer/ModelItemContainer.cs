using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ModelItemContainer : MonoBehaviour
{
    /// <summary>
    /// Model prefab used for instantiate models
    /// </summary>
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


    /// <summary>
    /// All instantiated models
    /// </summary>
    private List<ModelItem> _instantiatedModels = new List<ModelItem>();
    public List<ModelItem> InstantiatedModels
    {
        get
        {
            return _instantiatedModels;
        }
    }

    /// <summary>
    /// Refresh circle object
    /// </summary>
    private GameObject _refreshCircleObject = null;

    /// <summary>
    /// Refresh circle object rect trans - used for moving refresh circle
    /// </summary>
    private RectTransform _refreshCircleObjectRectTrans = null;


    /// <summary>
    /// Refresh circle object loading script - used for rotating refresh circle
    /// </summary>
    private Loading _refreshCircleLoadingScript = null;

    private Image _refreshCircleImage = null;

    /// <summary>
    /// Item container
    /// </summary>
    private GameObject _itemContainerGO;

    /// <summary>
    /// Main scroll view 
    /// </summary>
    private ScrollRect _scrollView;

    /// <summary>
    /// Rect trans of scroll view 
    /// </summary>
    private RectTransform _scrollViewRectTrans;

    /// <summary>
    /// Item for checking position of scroll view in order to implement refresh - outside scroll area
    /// </summary>
    private GameObject _scrollViewTopLimit;

    /// <summary>
    /// Item for checking position of scroll view in order to implement refresh - inside scroll area
    /// </summary>
    private GameObject _scrollViewPositionCheckItem;

    /// <summary>
    /// Flag determining whether login page started refreshing - in order not to start it again before coming back to previous state
    /// </summary>
    private bool _refreshingShouldBeFired = false;

    /// <summary>
    /// Flag determining if users data is being refreshed - preventing from refreshing again before actual refreshing ends
    /// </summary>
    private bool _whileRefreshing = false;

    private void Awake()
    {
        var scrollViewGO = this.transform.Find("ScrollView").gameObject;
        this._refreshCircleObject = this.transform.Find("RefreshCircleElement").gameObject;
        var circularProgressGO = _refreshCircleObject.transform.Find("CircularProgress").gameObject;
        var vicaGO = circularProgressGO.transform.Find("vica").gameObject;

        this._itemContainerGO = scrollViewGO.transform.Find("ItemsContainer").gameObject;


        this._scrollView = scrollViewGO.GetComponent<ScrollRect>();
        this._scrollViewRectTrans = scrollViewGO.GetComponent<RectTransform>();

        //Items for detecting position of scroll bar
        this._scrollViewPositionCheckItem = this._itemContainerGO.transform.Find("ScrollViewPositionCheckItem").gameObject;
        this._scrollViewTopLimit = scrollViewGO.transform.Find("ScrollViewTopLimit").gameObject;

        this._refreshCircleObjectRectTrans = _refreshCircleObject.GetComponent<RectTransform>();

        this._refreshCircleLoadingScript = _refreshCircleObject.GetComponentInChildren<Loading>();
        this._refreshCircleImage = vicaGO.GetComponentInChildren<Image>();

        //refreshing circle roation should be disabled in the begining
        _stopLoadingScript();

        this._scrollView.onValueChanged.AddListener(_handleScrollValueChanged);
    }

    private void Start()
    {
        //Refreshing data to display on startup
        RefreshDataDisplay();
    }

    /// <summary>
    /// Method used for refreshing model item container with MainCanvas
    /// </summary>
    public void RefreshDataDisplay()
    {
        _refreshModelList();
        _refreshAllModelDataDisplay();
    }

    /// <summary>
    /// Method called when scroll view values changes
    /// </summary>
    /// <param name="pos"></param>
    private void _handleScrollValueChanged(Vector2 pos)
    {
        //Calculating differance between element
        var differance = this._scrollViewTopLimit.transform.position.y - this._scrollViewPositionCheckItem.transform.position.y;

         _handleMoveRefreshCircle(differance);

    }

    /// <summary>
    /// Method used for starting loading script
    /// </summary>
    private void _startLoadingScript()
    {
        //starting script
        _refreshCircleLoadingScript.enabled = true;


    }

    /// <summary>
    /// Method used for stoping loading script
    /// </summary>
    private void _stopLoadingScript()
    {
        //stoping script
        _refreshCircleLoadingScript.enabled = false;

        //reseting loading image
        _refreshCircleImage.fillAmount = 0.75f;
        _refreshCircleImage.transform.localRotation = new Quaternion(0, 0, 0, 0);
    }

    /// <summary>
    /// Method that controls position of refreshing circle - fires refreshing methods if circle is low enought
    /// </summary>
    /// <param name="scrollDownOffset">
    /// scroll down offset - difference between first element in scroll view y position and reference element on the top
    /// </param>
    private void _handleMoveRefreshCircle(float scrollDownOffset)
    {
        if(scrollDownOffset > _refreshCircleObjectRectTrans.rect.height + 1)
        {
            //if refreshing circle is low enought - refreshing should be fired after refreshing circle comes back
            _refreshingShouldBeFired = true;
        }
        else if (scrollDownOffset <= _refreshCircleObjectRectTrans.rect.height)
        {
            //while refreshing flag blocks invoking several refreshiong at once
            if (_refreshingShouldBeFired && !_whileRefreshing)
            {
                //Invoking first part of user refresh - second one will be invoked with simulated delay
                _refreshUserDataBegin();

                //Specially delay method invoke to simulate longer loading- then invoke rest 
                Invoke(nameof(_refreshUserDataEnd), 0.5f);
            }
        }

        //Setting circle position y based on refreshing state:
        //If refreshing - circle should be fixed y position on the top of scroll view
        //If not refreshing - circle should move together with first element in scroll view - based on scrollDownOffset
        if(_whileRefreshing)
        {
            //if element is being refreshed - circle should stay presented
            _refreshCircleObjectRectTrans.anchoredPosition = new Vector2(_refreshCircleObjectRectTrans.anchoredPosition.x, -_refreshCircleObjectRectTrans.rect.height);
        }
        else
        {
            //if element is not being refreshed - circle offset should be equal to scroll offset
            _refreshCircleObjectRectTrans.anchoredPosition = new Vector2(_refreshCircleObjectRectTrans.anchoredPosition.x, -scrollDownOffset);
        }
    }

    /// <summary>
    /// method for refreshing user data- first part before simulating delay
    /// </summary>
    private void _refreshUserDataBegin()
    {
        //Invoking first part and other part after simulated time
        _whileRefreshing = true;

        //Starting rotating circle
        _startLoadingScript();

    }

    /// <summary>
    /// method for refreshing user data- second part after simulating delay
    /// </summary>
    private async Task _refreshUserDataEnd()
    {
        //Refreshing user data from server - refreshes also content of modelItemContainer via MainCanvas
        await MainCanvas.RefreshUserDataFromServer();

        //Returning with refreshing to initial position
        _refreshCircleObjectRectTrans.anchoredPosition = new Vector2(_refreshCircleObjectRectTrans.anchoredPosition.x, 0);

        //Stoping rotating circle
        _stopLoadingScript();

        //Resetting blocking flag if difference came back to normal
        _refreshingShouldBeFired = false;

        //Refreshing ends
        _whileRefreshing = false;
    }

    /// <summary>
    /// Method for creating and additing new model item to Instantiated model
    /// </summary>
    /// <param name="model"></param>
    private void _createAndAddModelItem(AssetModelLoader model)
    {
        var newModelItemGO = GameObject.Instantiate(ModelItemPrefab, _itemContainerGO.transform);

        //Creating and assigning model to new model item
        var newModelItem = newModelItemGO.GetComponent<ModelItem>();
        newModelItem.AssignModel(model);

        this.InstantiatedModels.Add(newModelItem);

    }

    /// <summary>
    /// Method for deleting 
    /// </summary>
    /// <param name="modelItem"></param>
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
    
    private void _refreshAllModelDataDisplay()
    {
        foreach(var item in InstantiatedModels)
        {
            item.RefreshDataDisplay();
        }
    }

}
