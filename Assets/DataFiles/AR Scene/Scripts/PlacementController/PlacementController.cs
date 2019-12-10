using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    [SerializeField]
    //only for testing
    public GameObject modelToPlace;

    [SerializeField]
    private GameObject _indicator;
    /// <summary>
    /// Quad of indicicator of object holder
    /// </summary>
    public GameObject Indicator
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
    private GameObject _doorComponentsButtonGO;
    /// <summary>
    /// Component for representing door components button game object
    /// </summary>
    public GameObject DoorComponentsButtonGO
    {
        get
        {
            return _doorComponentsButtonGO;
        }

        set
        {
            _doorComponentsButtonGO = value;
        }
    }

    private DoorComponentsButton _doorComponentsButton;
    /// <summary>
    /// Component for representing door components button
    /// </summary>
    public DoorComponentsButton DoorComponentsButton
    {
        get
        {
            return _doorComponentsButton;
        }

        set
        {
            _doorComponentsButton = value;
        }
    }

    private OBJModel _model = null;
    /// <summary>
    /// Object OBJ already placed on scene
    /// </summary>
    protected OBJModel Model
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
            return Indicator.activeSelf;
        }
    }

    protected void ShowIndicator()
    {
        this.Indicator.SetActive(true);
    }
    protected void HideIndicator()
    {
        this.Indicator.SetActive(false);
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

    private UnityEngine.XR.ARSubsystems.TrackableType _trackingType = UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds;
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
        this.MainController.onPlaceButtonClicked += MainControllerOnPlaceButtonClicked;
        this.MainController.onRemoveButtonClicked += MainControllerOnRemoveButtonClicked;
    }

    /// <summary>
    /// Method for initializing door component button
    /// </summary>
    private void InitDoorComponentsButton()
    {
        //Retrieveng main controller script from main controller game object
        this._doorComponentsButton = DoorComponentsButtonGO.GetComponent<DoorComponentsButton>();

        this.DoorComponentsButton.onHideDoorComponentsClicked += DoorComponentsButton_onHideDoorComponentsClicked;
        this.DoorComponentsButton.onShowDoorComponentsClicked += DoorComponentsButton_onShowDoorComponentsClicked;

        DoorComponentsButton.Hide();
        ShowDoorComponents();
    }

    /// <summary>
    /// Method for showing door components of switchboard
    /// </summary>
    private void ShowDoorComponents()
    {
        this.Model.ShowDoorComponents();
        this.DoorComponentsButton.SetDoorsToShown();
    }

    /// <summary>
    /// Method for hiding door components of switchboard
    /// </summary>
    private void HideDoorComponents()
    {
        this.Model.HideDoorComponents();
        this.DoorComponentsButton.SetDoorsToHidden();
    }

    /// <summary>
    /// Method invoked when show door compoenent button is clicked
    /// </summary>
    private void DoorComponentsButton_onShowDoorComponentsClicked()
    {
        ShowDoorComponents();
    }

    /// <summary>
    /// Method invoked when hide door compoenent button is clicked
    /// </summary>
    private void DoorComponentsButton_onHideDoorComponentsClicked()
    {
        HideDoorComponents();
    }

    /// <summary>
    /// On main controller remove button clicked
    /// </summary>
    private void MainControllerOnRemoveButtonClicked()
    {
        this.PickModel();
        MainController.ShowPlaceButton();
        MainController.ShowRotationRing();
        MainController.HideRemoveButton();
    }

    /// <summary>
    /// On main controller place button clicked
    /// </summary>
    private void MainControllerOnPlaceButtonClicked()
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
        //Rescale indicator to the size of model
        Indicator.transform.localScale = new Vector3(Model.Size.x / Indicator.transform.localScale.x, Model.Size.z / Indicator.transform.localScale.y, 1);

        //Hide indicator at the begining
        HideIndicator();
    }

    private void InitModel()
    {
        _model = new OBJModel();
        _model.Init(modelToPlace);

        //Assinging model to container
        Model.AssignParent(Container);

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
        DoorComponentsButton.Show();
        ShowDoorComponents();
    }

    public void PickModel()
    {
        if (!RotationInitialized || !PositionInitialized) return;

        Model.Hide();
        ShowIndicator();
        DoorComponentsButton.Hide();
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


    private void Start()
    {
        InitRaycastManager();
        InitModel();
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
