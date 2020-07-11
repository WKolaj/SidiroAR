using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MenuItem : MonoBehaviour
{
   
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


    private GameObject _iconGO = null;
    private GameObject _labelGO = null;
    private GameObject _separatorGO = null;
    private GameObject _menuItemButtonGO = null;

    private RectTransform _iconRectTrans = null;
    private RectTransform _labelRectTrans = null;
    private RectTransform _separatorRectTrans = null;

    private Image _iconImage = null;
    private Image _separatorImage = null;

    private TextMeshProUGUI _labelTMP = null;

    private MenuitemButton _menuItemButton = null;

    //ITEM DEPENDS ON SIZE AFTER UI SCALING - SO METHODS MUST BE INVOKED AFTER SCALING (IN START), AWAKE IS USED TO SET UP REFERENCES
    private void Awake()
    {
        this._iconGO = this.transform.Find("Icon").gameObject;
        this._iconRectTrans = _iconGO.GetComponent<RectTransform>();
        this._iconImage = _iconGO.GetComponent<Image>();


        this._labelGO = this.transform.Find("Label").gameObject;
        this._labelRectTrans = _labelGO.GetComponent<RectTransform>();
        this._labelTMP = _labelGO.GetComponent<TextMeshProUGUI>();

        this._separatorGO = this.transform.Find("Separator").gameObject;
        this._separatorRectTrans = _separatorGO.GetComponent<RectTransform>();
        this._separatorImage = _separatorGO.GetComponent<Image>();

        _menuItemButtonGO = this.transform.Find("MenuItemButton").gameObject;
        this._menuItemButton = _menuItemButtonGO.GetComponent<MenuitemButton>();


        this._menuItemButton.OnClick = this.OnClick;


    }

    public void SetText(string text)
    {
        this._labelTMP.text = text;
    }


}
