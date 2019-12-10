using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorComponentsButton : ClickableButton
{
    [SerializeField]
    private Material _hideDoorsButtonMaterial;
    public Material HideDoorsButtonMaterial
    {
        get
        {
            return _hideDoorsButtonMaterial;
        }

        set
        {
            _hideDoorsButtonMaterial = value;
        }
    }

    [SerializeField]
    private Material _showDoorsButtonMaterial;
    public Material ShowDoorsButtonMaterial
    {
        get
        {
            return _showDoorsButtonMaterial;
        }

        set
        {
            _showDoorsButtonMaterial = value;
        }
    }

    private bool _doorsShown = true;
    public bool DoorsShown
    {
        get
        {
            return this._doorsShown;
        }
    }

    public event Action onHideDoorComponentsClicked;
    public event Action onShowDoorComponentsClicked;

    protected override void onClicked()
    {
        base.onClicked();
        
        if (DoorsShown && onHideDoorComponentsClicked != null) onHideDoorComponentsClicked();
        else if (!DoorsShown && onShowDoorComponentsClicked != null) onShowDoorComponentsClicked();
    }

    public void SetDoorsToShown()
    {
        this._doorsShown = true;
        this.SetHideDoorsMaterial();
    }

    public void SetDoorsToHidden()
    {
        this._doorsShown = false;
        this.SetShowDoorsMaterial();
    }

    public void ShowButton()
    {
        this.gameObject.SetActive(true);
    }

    public void HideButton()
    {
        this.gameObject.SetActive(false);
    }

    private void SetShowDoorsMaterial()
    {
        gameObject.GetComponent<Image>().material = ShowDoorsButtonMaterial;
    }
    private void SetHideDoorsMaterial()
    {
        gameObject.GetComponent<Image>().material = HideDoorsButtonMaterial;
    }
}
