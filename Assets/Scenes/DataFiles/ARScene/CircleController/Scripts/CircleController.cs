using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[System.Serializable]
public class CircleControllerValueChange : UnityEvent<float>
{
}

[ExecuteInEditMode]
public class CircleController : MonoBehaviour
{
    /// <summary>
    /// Event invoked every change of value - method connected to event should change value after finishing
    /// </summary>
    [SerializeField]
    private CircleControllerValueChange _onValueChange = null;
    public CircleControllerValueChange OnValueChange
    {
        get
        {
            return _onValueChange;
        }

        set
        {
            _onValueChange = value;
        }
    }

    [SerializeField]
    private Button.ButtonClickedEvent _middleButtonClicked = null;
    public Button.ButtonClickedEvent MiddleButtonClicked
    {
        get
        {
            return _middleButtonClicked;
        }

        set
        {
            _middleButtonClicked = value;
        }
    }

    private GameObject _circleGO;
    private RectTransform _circleRectTrans;

    private GameObject _indicatorGO;
    private CircleControllerIndicator _indicator;
    private RectTransform _indicatorRectTransform;

    private GameObject _middleButtonGO;
    private ClickableImage _middleButton;
    private RectTransform _middleButtonRectTransform;

    private Vector3 _referenceAngleVector;
    private Vector3 _rotationVector;

    private float _circleControllerScale;
    private float _indicatorMagnitude;
    private float _indicatorSize;

    [SerializeField]
    private float _minValue = 0;
    public float MinValue
    {
        get
        {
            return _minValue;
        }

        set
        {
            _minValue = value;
        }
    }

    [SerializeField]
    private float _maxValue = 100;
    public float MaxValue
    {
        get
        {
            return _maxValue;
        }

        set
        {
            _maxValue = value;
        }
    }


    void Awake()
    {
        _initCircleController();
    }

    private void _initCircleController()
    {
        //Initialzing Circle
        _initCircle();

        //Initializing indicator
        _initCircleIndicator();

        //Initializing middle button
        _initMiddleButton();
    }

    private void _initCircle()
    {

        _circleGO = this.transform.Find("Circle").gameObject;
        _circleRectTrans = _circleGO.GetComponent<RectTransform>();

        _circleControllerScale = _circleRectTrans.rect.width / 583;

    }

    private void _initCircleIndicator()
    {
        _indicatorGO = _circleGO.transform.Find("Indicator").gameObject;

        _indicator = _indicatorGO.GetComponent<CircleControllerIndicator>();
        _indicatorRectTransform = _indicatorGO.GetComponent<RectTransform>();

        _indicator.DragHandler = _handleIndicatorDrag;


        //Calculating indicator maginitude, size and position
        _indicatorMagnitude = 249 * _circleControllerScale;
        _indicatorSize = 67 * _circleControllerScale;

        //Postioning and scaling indicator
        _indicatorRectTransform.sizeDelta = new Vector2(_indicatorSize, _indicatorSize);
        _indicatorRectTransform.localPosition = new Vector2(0, _indicatorMagnitude);

        //Enabling indicator
        _indicatorGO.SetActive(true);

        //Setting reference angle vector
        _referenceAngleVector = new Vector3(0, -_indicatorMagnitude, 0); ;
        _rotationVector = new Vector3(0, 0, 1);
    }

    private void _initMiddleButton()
    {
        _middleButtonGO = _circleGO.transform.Find("MiddleButton").gameObject;

        _middleButton = _middleButtonGO.GetComponent<ClickableImage>();
        _middleButtonRectTransform = _middleButtonGO.GetComponent<RectTransform>();

        //Setting size of middle button
        var middleButtonSize = 285 * _circleControllerScale;
        _middleButtonRectTransform.sizeDelta = new Vector2(middleButtonSize, middleButtonSize);

        //Initializing events
        _middleButton.OnClick = _middleButtonClicked;

    }

    public float Value
    {
        get
        {
            var angle = getAngleFromIndicatorPosition(_indicatorRectTransform.localPosition);

            return _convertAngleToValue(angle);
        }
    }

    /// <summary>
    /// Method invoked to change value and set rotation - has to be invoked in parent component when changeValueHandler is invoked
    /// </summary>
    /// <param name="newValue">
    /// new value
    /// </param>
    public void ChangeValue(float newValue)
    {
        var angle = _convertValueToAngle(newValue);
        _setIndicatorAngle(angle);
    }

    private void _handleIndicatorDrag(Vector3 delta)
    {
        var newIndicatorPosition = _indicatorRectTransform.localPosition + delta;
        var newAngle = getAngleFromIndicatorPosition(newIndicatorPosition);

        var newValue = _convertAngleToValue(newAngle);

        if (OnValueChange != null) OnValueChange.Invoke(newValue);
    }

    private float getAngleFromIndicatorPosition(Vector3 indicatorPosition)
    {
        return Vector3.SignedAngle(indicatorPosition, _referenceAngleVector, _rotationVector);
    }

    private void _setIndicatorAngle(float angle)
    {
        var newVector = Quaternion.AngleAxis(-angle, _rotationVector) * _referenceAngleVector;

        _indicatorRectTransform.localPosition = newVector;
    }

    private float _convertAngleToValue(float angle)
    {
        return MinValue + (MaxValue - MinValue) * ((180 + angle) / 360);
    }

    private float _convertValueToAngle(float value)
    {
        return  (360 * ((value - MinValue) / (MaxValue - MinValue)))-180;
    }

}
