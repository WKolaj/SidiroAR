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
    private Sprite _iconSprite = null;
    public Sprite IconSprite
    {
        get
        {
            return _iconSprite;
        }

        set
        {
            _iconSprite = value;
        }
    }

    [SerializeField]
    private Color _iconColor = Color.white;
    public Color IconColor
    {
        get
        {
            return _iconColor;
        }

        set
        {
            _iconColor = value;
        }
    }

    [SerializeField]
    private float _iconSize = 100;
    public float IconSize
    {
        get
        {
            return _iconSize;
        }

        set
        {
            _iconSize = value;
        }
    }

    [SerializeField]
    private float _iconOffset = 50;
    public float IconOffset
    {
        get
        {
            return _iconOffset;
        }

        set
        {
            _iconOffset = value;
        }
    }

    [SerializeField]
    private float _labelOffset = 100;
    public float LabelOffset
    {
        get
        {
            return _labelOffset;
        }

        set
        {
            _labelOffset = value;
        }
    }

    [SerializeField]
    private float _labelFontSize = 56;
    public float LabelFontSize
    {
        get
        {
            return _labelFontSize;
        }

        set
        {
            _labelFontSize = value;
        }
    }


    [SerializeField]
    private Color _labelFontColor = Color.white;
    public Color LabelFontColor
    {
        get
        {
            return _labelFontColor;
        }

        set
        {
            _labelFontColor = value;
        }
    }

    [SerializeField]
    private bool _labelBold = false;
    public bool LabelBold
    {
        get
        {
            return _labelBold;
        }

        set
        {
            _labelBold = value;
        }
    }

    [SerializeField]
    private string _labelText = "Button with icon";
    public string LabelText
    {
        get
        {
            return _labelText;
        }

        set
        {
            _labelText = value;
        }
    }

    [SerializeField]
    private float _separatorOffset = 0;
    public float SeparatorOffset
    {
        get
        {
            return _separatorOffset;
        }

        set
        {
            _separatorOffset = value;
        }
    }

    [SerializeField]
    private float _separatorWidth= 3;
    public float SeparatorWidth
    {
        get
        {
            return _separatorWidth;
        }

        set
        {
            _separatorWidth = value;
        }
    }

    [SerializeField]
    private Color _separatorColor = Color.gray;
    public Color SeparatorColor
    {
        get
        {
            return _separatorColor;
        }

        set
        {
            _separatorColor = value;
        }
    }

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

    private void Awake()
    {
        _initIcon();
        _initLabel();
        _initSeprator();
        _initMenuItemButton();
    }

    private void _initIcon()
    {
        this._iconGO = this.transform.Find("Icon").gameObject;
        this._iconRectTrans = _iconGO.GetComponent<RectTransform>();
        this._iconImage = _iconGO.GetComponent<Image>();

        _iconImage.sprite = _iconSprite;
        _iconImage.color = _iconColor;

        _iconRectTrans.anchoredPosition = new Vector3(IconOffset, _iconRectTrans.anchoredPosition.y);
        _iconRectTrans.sizeDelta = new Vector2(IconSize, IconSize);
    }

    private void _initLabel()
    {
        this._labelGO = this.transform.Find("Label").gameObject;
        this._labelRectTrans = _labelGO.GetComponent<RectTransform>();
        this._labelTMP = _labelGO.GetComponent<TextMeshProUGUI>();

        this._labelTMP.text = LabelText;
        this._labelTMP.fontSize = LabelFontSize;
        this._labelTMP.color = LabelFontColor;
        this._labelTMP.fontStyle = this.LabelBold ? FontStyles.Bold : FontStyles.Normal;

        this._labelRectTrans.offsetMin = new Vector2(LabelOffset, this._labelRectTrans.offsetMin.y);
    }

    private void _initSeprator()
    {
        this._separatorGO = this.transform.Find("Separator").gameObject;
        this._separatorRectTrans = _separatorGO.GetComponent<RectTransform>();
        this._separatorImage = _separatorGO.GetComponent<Image>();

        this._separatorImage.color = SeparatorColor;
        this._separatorRectTrans.sizeDelta = new Vector3(_separatorRectTrans.sizeDelta.x, SeparatorWidth);
        this._separatorRectTrans.anchoredPosition = new Vector3(_separatorRectTrans.anchoredPosition.x, SeparatorOffset);

    }

    private void _initMenuItemButton()
    {
        _menuItemButtonGO = this.transform.Find("MenuItemButton").gameObject;

        this._menuItemButton = _menuItemButtonGO.GetComponent<MenuitemButton>();

        this._menuItemButton.OnClick = this.OnClick;

    }

}
