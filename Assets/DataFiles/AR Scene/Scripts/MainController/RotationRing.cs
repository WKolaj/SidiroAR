using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationRing : MonoBehaviour
{
    /// <summary>
    /// Method for hiding rotation ring
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method for showing rotation ring
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Check if Rotation Ring is shown
    /// </summary>
    public bool IsShown
    {
        get
        {
            return this.gameObject.activeSelf;
        }
    }

}
