using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Event call on click of button
    /// </summary>
    public event Action onButtonClicked;

    //Method called per every click
    public void OnPointerClick(PointerEventData eventData)
    {
        //Calling virtual on click method
        onClicked();

        if (onButtonClicked != null)
        {
            onButtonClicked();
        }
    }

    protected virtual void onClicked()
    {

    }

    /// <summary>
    /// Method for hiding button
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method for showing button
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Check if button is shown
    /// </summary>
    public bool IsShown
    {
        get
        {
            return this.gameObject.activeSelf;
        }
    }

}
