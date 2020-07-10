using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleControllerIndicator : MonoBehaviour, IDragHandler
{
    public Action<Vector2> DragHandler = null;

    /// <summary>
    /// Method called every drag of button
    /// </summary>
    /// <param name="eventData">
    /// Events associated with dragging
    /// </param>
    public void OnDrag(PointerEventData eventData)
    {
        if(DragHandler != null)
        {
            DragHandler(eventData.delta);
        }
    }
}
