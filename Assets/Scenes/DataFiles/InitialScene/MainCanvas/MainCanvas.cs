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

    public void ShowDialog1()
    {
        DialogWindow.ShowStatic("To jest jakaś wiadomość", "OK", () => Debug.Log("OK Clicked"), "#eb4334");
    }

    public void ShowDialog2()
    {
        DialogWindow.ShowStatic("windo text2", "button0 Text2", () => Debug.Log("Button0 2"), "#2e5de8", "Button1 text2", () => Debug.Log("BUtton1 2"), "#e8e22e");
    }
}
