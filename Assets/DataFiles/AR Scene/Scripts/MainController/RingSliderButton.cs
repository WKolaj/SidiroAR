using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingSliderButton : MonoBehaviour, IDragHandler
{

    private MainController _mainController = null;
    /// <summary>
    /// Main controller containg ring slider button
    /// </summary>
    public MainController MainController
    {
        get
        {
            return _mainController;
        }
    }

    private GameObject _container = null;
    /// <summary>
    /// Element containing ring slider button
    /// </summary>
    public GameObject Container
    {
        get
        {
            return _container;
        }
    }

    private Vector3 _containerCenterPoint = default;
    /// <summary>
    /// Center point of container of ring slider button
    /// </summary>
    public Vector3 ContainerCenterPoint
    {
        get
        {
            return _containerCenterPoint;
        }
    }

    private float _magnitude = default;
    /// <summary>
    /// Magnitude of rotation for button
    /// </summary>
    public float Magintude
    {
        get
        {
            return _magnitude;
        }
    }

    /// <summary>
    /// Method for initializing main controller
    /// </summary>
    private void InitMainController()
    {
        _mainController = GetComponentInParent<MainController>();
    }

    /// <summary>
    /// Method for initializing container
    /// </summary>
    private void InitContainerController()
    {
        _container = this.transform.parent.gameObject;
    }

    /// <summary>
    /// Method for recalculating container center point and magnitude of rotation
    /// </summary>
    private void RecalculateContainerCenterPointAndMagnitude()
    {
        _containerCenterPoint = Container.transform.position;
        _magnitude = (this.transform.position - _containerCenterPoint).magnitude;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitMainController();
        InitContainerController();
        RecalculateContainerCenterPointAndMagnitude();
    }


    /// <summary>
    /// Method called every drag of button
    /// </summary>
    /// <param name="eventData">
    /// Events associated with dragging
    /// </param>
    public void OnDrag(PointerEventData eventData)
    {
        //Recalcuting container center point and magnitude - in case of window resizing
        RecalculateContainerCenterPointAndMagnitude(); 

        //Calculating new normalized direction vector
        var newDragPosition = this.transform.position + (Vector3)eventData.delta;
        var relativeDragPosition = newDragPosition - ContainerCenterPoint;
        var normalizedRelativeDragPosition = relativeDragPosition / relativeDragPosition.magnitude;

        //Refreshing main controller based on new vector
        MainController.AssingNewDirectionVector(normalizedRelativeDragPosition);

    }

    /// <summary>
    /// Method for assigning new direction vector from Main controller
    /// </summary>
    /// <param name="newDirectionVector">
    /// new direction vector
    /// </param>
    public void AssingNewDirectionVector(Vector3 newDirectionVector)
    {
        this.transform.position = ContainerCenterPoint + Magintude * newDirectionVector;
    }
}
