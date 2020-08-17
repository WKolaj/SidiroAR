using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class of button invoking OnClick event if it's pressed
/// </summary>
public class PrecizePositioningButton : MonoBehaviour
{
    /// <summary>
    /// Event invoked when button is pressed
    /// </summary>
    [SerializeField]
    private Button.ButtonClickedEvent _onClick = null;
    public Button.ButtonClickedEvent OnClick
    {
        get
        {
            return _onClick;
        }

        set
        {
            _onClick = value;
        }
    }

    /// <summary>
    /// Method invoked when button is released
    /// </summary>
    public void HandlePointerUp()
    {
        _stopPressedMechanism();
        _resetPressedMechanism();
    }

    /// <summary>
    /// Method invoked when button is pressed
    /// </summary>
    public void HandlePointerDown()
    {
        _startPressedMechanism();
    }

    /// <summary>
    /// Is button pressed
    /// </summary>
    private bool _pressed = false;
    public bool Pressed
    {
        get
        {
            return _pressed;
        }

        private set
        {
            _pressed = value;
        }
    }

    /// <summary>
    /// Time stamp of last button press
    /// </summary>
    private DateTime _lastTickDateTime = DateTime.MinValue;

    /// <summary>
    /// Default time dif between onclick event fire - default value will be always diveded by 2
    /// </summary>
    private Double _defaultTickTimeDif = 1000;

    /// <summary>
    /// Actual time diff between on click event fire
    /// </summary>
    private Double _tickTimeDif = 1000;

    /// <summary>
    /// Min time diff between on click event fire
    /// </summary>
    private Double _minTickTimeDif = 100;

    /// <summary>
    /// Method to reset pressed mechanism
    /// </summary>
    private void _resetPressedMechanism()
    {
        //Setting last tick date time to min value
        _lastTickDateTime = DateTime.MinValue;

        //Resetting onClick event timeDif to default value
        _tickTimeDif = _defaultTickTimeDif;
    }

    /// <summary>
    /// Method for starting pressed mechanism
    /// </summary>
    private void _startPressedMechanism()
    {
        _lastTickDateTime = DateTime.MinValue;
        this.Pressed = true;
    }

    /// <summary>
    /// Method for stoping pressed mechanism
    /// </summary>
    private void _stopPressedMechanism()
    {
        this.Pressed = false;
    }

    /// <summary>
    /// Method invoked every tick - invoking OnClick event
    /// </summary>
    private void _tick()
    {
        //Setting new last tick date time
        this._lastTickDateTime = DateTime.Now;
        
        //dividing tick time dif and checking whether its smaller than minimum value
        _tickTimeDif = _tickTimeDif / 2;
        if (_tickTimeDif < _minTickTimeDif) _tickTimeDif = _minTickTimeDif;

        //Invoking on click if its not null

        if (OnClick != null)
            OnClick.Invoke();
    }

    private void Update()
    {
        if (Pressed)
        {
            var timeDif = DateTime.Now - _lastTickDateTime;

            if(timeDif.TotalMilliseconds >= _tickTimeDif)
            {
                _tick();
            }
        }
    }
}
