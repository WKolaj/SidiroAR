using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    /// <summary>
    /// Method for generating size and position of model
    /// </summary>
    /// <param name="model">
    /// Model to measure
    /// </param>
    /// <returns>
    /// Array of vectors
    /// [0] - size
    /// [1] - position
    /// </returns>
    private static Vector3[] GetSizeAndPositionOfCombinedMesh(GameObject model)
    {
        GameObject objectToReturn = new GameObject("object with mesh");

        objectToReturn.AddComponent<MeshFilter>();
        objectToReturn.AddComponent<MeshRenderer>();
        MeshFilter[] meshFilters = model.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            i++;
        }

        objectToReturn.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        objectToReturn.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        Mesh mesh = objectToReturn.transform.GetComponent<MeshFilter>().mesh;

        var sizeVector = new Vector3(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z);
        var positionVector = new Vector3(mesh.bounds.min.x, mesh.bounds.min.y, mesh.bounds.min.z);

        if (objectToReturn != null)
            Destroy(objectToReturn);

        return new Vector3[] { sizeVector, positionVector };
    }


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

    /// <summary>
    /// Object OBJ to place on scene
    /// </summary>
    protected GameObject ModelToPlace
    {
        get
        {
            return Common.GetModel(); 
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

    private GameObject _model = null;
    /// <summary>
    /// Object OBJ already placed on scene
    /// </summary>
    protected GameObject Model
    {
        get
        {
            return _model;
        }

        set
        {
            _model = value;
        }
    }

    protected bool ModelShown
    {
        get
        {
            return Model.activeSelf;
        }
    }

    protected bool IndicatorShown
    {
        get
        {
            return Indicator.activeSelf;
        }
    }

    protected void ShowModel()
    {
        this.Model.SetActive(true);
    }
    protected void HideModel()
    {
        this.Model.SetActive(false);
    }

    protected void ShowIndicator()
    {
        this.Indicator.SetActive(true);
    }
    protected void HideIndicator()
    {
        this.Indicator.SetActive(false);
    }

    private Vector3 _modelSize;
    protected Vector3 ModelSize
    {
        get
        {
            return _modelSize;
        }
    }

    private Vector3 _modelPosition;
    protected Vector3 ModelPosition
    {
        get
        {
            return _modelPosition;
        }
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
        Indicator.transform.localScale = new Vector3(ModelSize.x / Indicator.transform.localScale.x, ModelSize.z / Indicator.transform.localScale.y, 1);

        //Hide indicator at the begining
        HideIndicator();
    }

    private void InitModel()
    {
        Debug.Log(ModelToPlace);
        //Creating instance of model and assing it to container
        Model = Instantiate(ModelToPlace);
        Model.transform.parent = Container.transform;

        //Get and assign model size and position
        var sizeAndPosition = GetSizeAndPositionOfCombinedMesh(Model);
        _modelSize = sizeAndPosition[0];
        _modelPosition = sizeAndPosition[1];

        //Center the model according to container
        Model.transform.Translate(-ModelPosition);
        Model.transform.Translate(new Vector3(-0.5f * ModelSize.x, 0, -0.5f * ModelSize.z));

        //Hide model at the begining
        HideModel();
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

        ShowModel();
        HideIndicator();
    }

    public void PickModel()
    {
        if (!RotationInitialized || !PositionInitialized) return;

        HideModel();
        ShowIndicator();
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
        if (ModelShown) return;
        
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
