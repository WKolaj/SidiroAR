using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    CircleController _circleController;

    private void Awake()
    {
        _circleController = this.GetComponentInChildren<CircleController>();
    }

    public void HandleCircleChanged(float newValue)
    {
    
        _circleController.ChangeValue(newValue);
    }

    public void HandleCircleButtonClicked()
    {
        Debug.Log("Circle button clicked");
    }
}
