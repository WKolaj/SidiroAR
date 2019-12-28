using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    [SerializeField]
    //only for testing
    public GameObject modelToPlace;

    [SerializeField]
    private GameObject _indicatorGO;
    /// <summary>
    /// Quad of indicicator of object holder
    /// </summary>
    public GameObject IndicatorGO
    {
        get
        {
            return _indicatorGO;
        }

        set
        {
            _indicatorGO = value;
        }
    }

    private Indicator _indicator;
    /// <summary>
    /// Quad of indicicator of object holder
    /// </summary>
    public Indicator Indicator
    {
        get
        {
            return _indicator;
        }

        set
        {
            _indicator = value;
        }
    }

    [SerializeField]
    private GameObject _indicatorArrowGO;
    /// <summary>
    /// Quad of indicicator arrow of object holder
    /// </summary>
    public GameObject IndicatorArrowGO
    {
        get
        {
            return _indicatorArrowGO;
        }

        set
        {
            _indicatorArrowGO = value;
        }
    }

    private IndicatorArrow _indicatorArrow;
    /// <summary>
    /// Quad of indicicator Arrow of object holder
    /// </summary>
    public IndicatorArrow IndicatorArrow
    {
        get
        {
            return _indicatorArrow;
        }

        set
        {
            _indicatorArrow = value;
        }
    }

    [SerializeField]
    private GameObject _container;
    /// <summary>
    /// Holder to contain both - OBJ and quad with indicator
    /// </summary>
    public GameObject Container
    {
        get
        {
            return _container;
        }

        set
        {
            _container = value;
        }
    }

    [SerializeField]
    private GameObject _mainControllerGO;
    public GameObject MainControllerGO
    {
        get
        {
            return _mainControllerGO;
        }

        set
        {
            _mainControllerGO = value;
        }
    }

    private MainController _mainController;
    protected MainController MainController
    {
        get
        {
            return _mainController;
        }
    }

    [SerializeField]
    private GameObject _showDoorComponentsButtonGO;
    /// <summary>
    /// Component for representing door components button game object
    /// </summary>
    public GameObject ShowDoorComponentsButtonGO
    {
        get
        {
            return _showDoorComponentsButtonGO;
        }

        set
        {
            _showDoorComponentsButtonGO = value;
        }
    }

    [SerializeField]
    private GameObject _hideDoorComponentsButtonGO;
    /// <summary>
    /// Component for representing door components button game object
    /// </summary>
    public GameObject HideDoorComponentsButtonGO
    {
        get
        {
            return _hideDoorComponentsButtonGO;
        }

        set
        {
            _hideDoorComponentsButtonGO = value;
        }
    }


    private ShowDoorComponentsButton _showDoorComponentsButton;
    /// <summary>
    /// Component for representing door components button
    /// </summary>
    public ShowDoorComponentsButton ShowDoorComponentsButton
    {
        get
        {
            return _showDoorComponentsButton;
        }

        set
        {
            _showDoorComponentsButton = value;
        }
    }

    private HideDoorComponentsButton _hideDoorComponentsButton;
    /// <summary>
    /// Component for representing door components button
    /// </summary>
    public HideDoorComponentsButton HideDoorComponentsButton
    {
        get
        {
            return _hideDoorComponentsButton;
        }

        set
        {
            _hideDoorComponentsButton = value;
        }
    }

    private AssetModel _model = null;
    /// <summary>
    /// Object OBJ already placed on scene
    /// </summary>
    protected AssetModel Model
    {
        get
        {
            return _model;
        }

    }


    protected bool IndicatorShown
    {
        get
        {
            return IndicatorGO.activeSelf;
        }
    }

    protected void ShowIndicator()
    {
        this.IndicatorGO.SetActive(true);
        this.IndicatorArrowGO.SetActive(true);
    }

    protected void HideIndicator()
    {
        this.IndicatorGO.SetActive(false);
        this.IndicatorArrowGO.SetActive(false);
    }

    private Quaternion _initialContainerRotation;
    public Quaternion InitialContainerRotation
    {
        get
        {
            return _initialContainerRotation;
        }
    }


    private Quaternion _containerRotation;
    public Quaternion ContainerRotation
    {
        get
        {
            return _containerRotation;
        }
    }

    private Vector3 _containerPosition ;
    public Vector3 ContainerPosition
    {
        get
        {
            return _containerPosition;
        }
    }

    private bool _rotationInitialized = false;
    public bool RotationInitialized
    {
        get
        {
            return _rotationInitialized;
        }
    }

    private bool _positionInitialized = false;
    public bool PositionInitialized
    {
        get
        {
            return _positionInitialized;
        }
    }

    private ARRaycastManager _raycastManager = default;
    public ARRaycastManager RaycastManager
    {
        get
        {
            return _raycastManager;
        }
    }

    private Vector2 _screenCenterPoint = default;
    public Vector2 ScreenCenterPoint
    {
        get
        {
            return _screenCenterPoint;
        }
    }

    private Pose _currentRaycastPointPose = default;
    public Pose CurrentRaycastPointPose
    {
        get
        {
            return _currentRaycastPointPose;
        }
    }

    private UnityEngine.XR.ARSubsystems.TrackableType _trackingType = UnityEngine.XR.ARSubsystems.TrackableType.Planes;
    public UnityEngine.XR.ARSubsystems.TrackableType TrackingType
    {
        get
        {
            return _trackingType;
        }
    }

    /// <summary>
    /// Method for initializing main controller
    /// </summary>
    private void InitMainController()
    {
        //Retrieveng main controller script from main controller game object
        this._mainController = MainControllerGO.GetComponent<MainController>();

        this.MainController.onAngleChanged += OnAngleChanged;
    }

    /// <summary>
    /// Method for initializing door component button
    /// </summary>
    private void InitDoorComponentsButton()
    {
        //Retrieveng main controller script from main controller game object
        this._showDoorComponentsButton = ShowDoorComponentsButtonGO.GetComponent<ShowDoorComponentsButton>();
        this._hideDoorComponentsButton = HideDoorComponentsButtonGO.GetComponent<HideDoorComponentsButton>();

        this.ShowDoorComponentsButton.gameObject.SetActive(false);
        this.HideDoorComponentsButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method for showing door components of switchboard
    /// </summary>
    private void ShowDoorComponents()
    {
        this.Model.ShowDoorComponents();
        this.ShowDoorComponentsButton.gameObject.SetActive(false);
        this.HideDoorComponentsButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Method for hiding door components of switchboard
    /// </summary>
    private void HideDoorComponents()
    {
        this.Model.HideDoorComponents();
        this.ShowDoorComponentsButton.gameObject.SetActive(true);
        this.HideDoorComponentsButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method invoked when show door compoenent button is clicked
    /// </summary>
    public void HandleShowDoorComponentsClicked()
    {
        ShowDoorComponents();
    }

    /// <summary>
    /// Method invoked when hide door compoenent button is clicked
    /// </summary>
    public void HandleHideDoorComponentsClicked()
    {
        HideDoorComponents();
    }


    /// <summary>
    /// On main controller remove button clicked
    /// </summary>
    public void HandleRemoveButtonClicked()
    {
        this.PickModel();
        MainController.ShowPlaceButton();
        MainController.ShowRotationRing();
        MainController.HideRemoveButton();
    }

    /// <summary>
    /// On main controller place button clicked
    /// </summary>
    public void HandlePlaceButtonClicked()
    {
        this.PlaceModel();
        MainController.HidePlaceButton();
        MainController.HideRotationRing();
        MainController.ShowRemoveButton();
    }

    /// <summary>
    /// Method called when main controller's angle changes
    /// </summary>
    /// <param name="newAngle">
    /// New angle
    /// </param>
    private void OnAngleChanged(float newAngle)
    {
        AssingContainerAngle(newAngle);
    }

    private void InitScreenCenterPoint()
    {
        _screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    private void InitRotation(Quaternion rotation)
    {
        this._initialContainerRotation = rotation;
        this._containerRotation = rotation;
        this._rotationInitialized = true;
    }

    private void InitPosition(Vector3 position)
    {
        this._containerPosition = position;
        this._positionInitialized = true;
    }

    private void InitIndicator()
    {
        //Assigning indicator component
        _indicator = IndicatorGO.GetComponent<Indicator>();

        this.Indicator.AdjustSize(Model.Size);

        //Hide indicator at the begining
        HideIndicator();
    }

    private void InitIndicatorArrow()
    {
        //Assigning indicator component
        _indicatorArrow = IndicatorArrowGO.GetComponent<IndicatorArrow>();

        this.IndicatorArrow.AdjustSize(Model.Size);

    }

    private void InitModel()
    {
        var creator = new AssetModelCreator(Common.ModelPath);
        _model = creator.CreateModel(Container);

        Model.Hide();
    }

    private void InitRaycastManager()
    {
        //Assing ray cast to object
        _raycastManager = GetComponent<ARRaycastManager>();
    }

    private void InitContainer()
    {
        Container.SetActive(true);
    }

    private void SetCurrentRaycastPointPose(ARRaycastHit hit)
    {
        _currentRaycastPointPose = hit.pose;
    }

    private void AdjustContainerPosition(Vector3 position, Quaternion rotation)
    {
        Container.transform.SetPositionAndRotation(position, rotation);
        _containerPosition = position;
        _containerRotation = rotation;
    }

    public void PlaceModel()
    {
        if (!RotationInitialized || !PositionInitialized) return;

        Model.Show();
        HideIndicator();
        ShowDoorComponents();
    }

    public void PickModel()
    {
        if (!RotationInitialized || !PositionInitialized) return;

        Model.Hide();
        ShowIndicator();
        this.ShowDoorComponentsButton.gameObject.SetActive(false);
        this.HideDoorComponentsButton.gameObject.SetActive(false);
    }

    public void RotateContainer(float x, float y, float z)
    {
        //Making rotation
        Container.transform.Rotate(new Vector3(x, y, z));

        //Refreshing current rotation
        _containerRotation = Container.transform.rotation;
    }

    public void AssingContainerAngle(float angle)
    {
        AdjustContainerPosition(ContainerPosition, InitialContainerRotation);

        RotateContainer(0, angle, 0);
    }


    private void Awake()
    {
        InitRaycastManager();
        InitModel();
        //Always init arrow before indicator
        InitIndicatorArrow();
        InitIndicator();
        InitScreenCenterPoint();
        InitContainer();
        InitMainController();
        InitDoorComponentsButton();
    }

    /// <summary>
    /// Method for casting raycast into 
    /// </summary>
    /// <returns></returns>
    private List<ARRaycastHit> CastRaycast()
    {
        List<ARRaycastHit> hitsToReturn = new List<ARRaycastHit>();

        if (RaycastManager.Raycast(ScreenCenterPoint, hitsToReturn, TrackingType))
        {
            return hitsToReturn;
        }
        else
        {
            return new List<ARRaycastHit>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If model is shown - do not anything
        if (Model.Shown) return;
        
        //Casting raycast to real enviroment
        List<ARRaycastHit> hits = CastRaycast();

        if (hits.Count > 0)
        {
                //Casting successfull - show indicator
                ShowIndicator();

                //Setting current raycast point pose
                SetCurrentRaycastPointPose(hits[0]);

                //Initializing rotation if not initialized
                if (!RotationInitialized)
                    InitRotation(CurrentRaycastPointPose.rotation);

                if (!PositionInitialized)
                    InitPosition(CurrentRaycastPointPose.position);

                //Adjusting container to raycast hit
                AdjustContainerPosition(CurrentRaycastPointPose.position, ContainerRotation);

        }
    }
}
