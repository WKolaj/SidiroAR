using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private RingSliderButton _ringSliderButton = null;
    /// <summary>
    /// Object of rotating ring slider button
    /// </summary>
    public RingSliderButton RingSliderButton
    {
        get
        {
            return _ringSliderButton; 
        }
    }

    private PlaceButton _placeButton = null;
    /// <summary>
    /// Object of place button
    /// </summary>
    public PlaceButton PlaceButton
    {
        get
        {
            return _placeButton;
        }
    }

    private RemoveButton _removeButton = null;
    /// <summary>
    /// Object of remove button
    /// </summary>
    public RemoveButton RemoveButton
    {
        get
        {
            return _removeButton;
        }
    }

    private RotationRing _rotationRing= null;
    /// <summary>
    /// Object of remove button
    /// </summary>
    public RotationRing RotationRing
    {
        get
        {
            return _rotationRing;
        }
    }

    private Vector3 _directionVector = new Vector3(0, 1, 0);
    /// <summary>
    /// Normalized vector showing direction for ring slider button
    /// </summary>
    public Vector3 DirectionVector
    {
        get
        {
            return _directionVector;
        }
    }

    private float _angle = 0.0f;
    /// <summary>
    /// Angle of main controller
    /// </summary>
    public float Angle
    {
        get
        {
            return _angle;
        }

        set
        {
            _angle = value;
        }
    }

    /// <summary>
    /// Event called when rotation change
    /// </summary>
    public event Action<float> onAngleChanged;

    /// <summary>
    /// Event called when green button clicked
    /// </summary>
    public event Action onPlaceButtonClicked;

    /// <summary>
    /// Event called when red button clicked
    /// </summary>
    public event Action onRemoveButtonClicked;

    /// <summary>
    /// Method for initializing ring slider button
    /// </summary>
    private void InitComponents()
    {
        _ringSliderButton = GetComponentInChildren<RingSliderButton>();
        _rotationRing = GetComponentInChildren<RotationRing>();
        _placeButton = GetComponentInChildren<PlaceButton>();
        _removeButton = GetComponentInChildren<RemoveButton>();

        PlaceButton.onButtonClicked += PlaceButtonClicked;
        RemoveButton.onButtonClicked += RemoveButtonClicked;

        ShowPlaceButton();
        HideRemoveButton();
        ShowRotationRing();
    }

    /// <summary>
    /// Method invoked when remove button is clicked
    /// </summary>
    private void RemoveButtonClicked()
    {
        if(this.onRemoveButtonClicked != null)
        {
            this.onRemoveButtonClicked();
        }
    }

    /// <summary>
    /// Method invoked when place button is clicked
    /// </summary>
    private void PlaceButtonClicked()
    {
        if (this.onPlaceButtonClicked != null)
        {
            this.onPlaceButtonClicked();
        }
    }

    /// <summary>
    /// Method for calculating rotation related to Y axis based on direction vector
    /// </summary>
    /// <param name="directionVector">
    /// direction vector
    /// </param>
    /// <returns>
    /// Angle between direction vector and Y axis
    /// </returns>
    private float CalculateRotationFromDirectionVector(Vector3 directionVector)
    {
        var angle = Mathf.Atan2(directionVector.y, directionVector.x) * 180 / Mathf.PI;

        var normalizeAngleAccordingToYAxis = 90 - angle;

        return normalizeAngleAccordingToYAxis >= 0 ? normalizeAngleAccordingToYAxis : normalizeAngleAccordingToYAxis + 360; 
    }

    /// <summary>
    /// Method for assining new direction vector
    /// </summary>
    /// <param name="newDirectionVector">
    /// new direction vector</param>
    public void AssingNewDirectionVector(Vector3 newDirectionVector)
    {
        _directionVector = newDirectionVector;

        //Recalculating vector to angle
        _angle = CalculateRotationFromDirectionVector(DirectionVector);

        RingSliderButton.AssingNewDirectionVector(newDirectionVector);

        //Calling rotation change event
        if(onAngleChanged != null)
        {
            onAngleChanged(this.Angle);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitComponents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method for showing rotation ring
    /// </summary>
    public void ShowRotationRing()
    {
        RotationRing.Show();
    }

    /// <summary>
    /// Method for hiding rotation ring
    /// </summary>
    public void HideRotationRing()
    {
        RotationRing.Hide();
    }

    /// <summary>
    /// Method for showing place button
    /// </summary>
    public void ShowPlaceButton()
    {
        PlaceButton.Show();
    }

    /// <summary>
    /// Method for hiding place button
    /// </summary>
    public void HidePlaceButton()
    {
        PlaceButton.Hide();
    }

    /// <summary>
    /// Method for showing remove button
    /// </summary>
    public void ShowRemoveButton()
    {
        RemoveButton.Show();
    }

    /// <summary>
    /// Method for hiding remove button
    /// </summary>
    public void HideRemoveButton()
    {
        RemoveButton.Hide();
    }
}
