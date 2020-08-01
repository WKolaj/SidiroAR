using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public enum PlacementControllerState
{
    waitingForSurface, indicatorPresented, modelPresented
}

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
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

    /// <summary>
    /// Flag determining whether door components are avaiable in model
    /// </summary>
    public Boolean DoorsAvailable
    {
        get
        {
            if (Model == null) return false;
            return Model.AreDoorComponentsAvailable;
        }
    }

    /// <summary>
    /// Flag determining whether cover components are avaiable in model
    /// </summary>
    public Boolean CoversAvailable
    {
        get
        {
            if (Model == null) return false;
            return Model.AreCoverComponentsAvailable;
        }
    }

    /// <summary>
    /// Flag determining in which state placement controller is actually in
    /// </summary>
    private PlacementControllerState _placementControllerState = PlacementControllerState.waitingForSurface;
    public PlacementControllerState PlacementControllerState
    {
        get
        {
            return _placementControllerState;
        }

        private set
        {
            _placementControllerState = value;
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

    /// <summary>
    /// Flag determing whether doors are shown
    /// </summary>
    private bool _doorsShown = true;

    public bool DoorsShown
    {
        get
        {
            return _doorsShown;
        }

        private set
        {
            _doorsShown = value;
        }
    }

    /// <summary>
    /// Flag determing whether covers are shown
    /// </summary>
    private bool _coversShown = true;

    public bool CoversShown
    {
        get
        {
            return _coversShown;
        }

        private set
        {
            _coversShown = value;
        }
    }

    private void ShowIndicator()
    {
        this.Indicator.gameObject.SetActive(true);
    }

    private void HideIndicator()
    {
        Indicator.gameObject.SetActive(false);
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
    /// Method for showing door components of switchboard
    /// </summary>
    public void ShowDoorComponents()
    {
        this.Model.ShowDoorComponents();
        DoorsShown = true;
    }

    /// <summary>
    /// Method for hiding door components of switchboard
    /// </summary>
    public void HideDoorComponents()
    {
        this.Model.HideDoorComponents();
        DoorsShown = false;
    }

    /// <summary>
    /// Method for showing cover components of switchboard
    /// </summary>
    public void ShowCoverComponents()
    {
        this.Model.ShowCoverComponents();
        CoversShown = true;
    }

    /// <summary>
    /// Method for hiding cover components of switchboard
    /// </summary>
    public void HideCoverComponents()
    {
        this.Model.HideCoverComponents();
        CoversShown = false;
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
        _indicator = Container.GetComponentInChildren<Indicator>(true);

        //Border width should be scaled as well
        if (Common.Scale != 1.0f)
            this.Indicator.BorderWidth = Common.Scale * this.Indicator.BorderWidth;

        this.Indicator.Init(Model.Size, Model.InitialPosition);

        //Hide indicator at the begining
        HideIndicator();
    }

    private void InitModel()
    {
        var creator = new AssetModelCreator(Common.ModelPath);
        _model = creator.CreateModel(Container, Common.Scale);

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
        ShowCoverComponents();
        this.PlacementControllerState = PlacementControllerState.modelPresented;
    }

    public void PickModel()
    {
        if (!RotationInitialized || !PositionInitialized) return;

        Model.Hide();
        ShowIndicator();
        this.PlacementControllerState = PlacementControllerState.indicatorPresented;
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
        InitIndicator();
        InitScreenCenterPoint();
        InitContainer();

        PlacementControllerState = PlacementControllerState.waitingForSurface;
        DoorsShown = true;
        CoversShown = true;

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
            {
                //Initial rotation should be 180 degree rotated - arrow pointing to camera
                Vector3 rot = CurrentRaycastPointPose.rotation.eulerAngles;
                rot = new Vector3(rot.x, rot.y + 180, rot.z);

                InitRotation(Quaternion.Euler(rot));
            }

            if (!PositionInitialized)
                InitPosition(CurrentRaycastPointPose.position);

            //Adjusting container to raycast hit
            AdjustContainerPosition(CurrentRaycastPointPose.position, ContainerRotation);

            PlacementControllerState = PlacementControllerState.indicatorPresented;

        }
    }
}
