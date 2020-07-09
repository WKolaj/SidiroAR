﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ClickableImage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Sprite _backgroundSprite = null;
    public Sprite BackgroundSprite
    {
        get
        {
            return _backgroundSprite;
        }

        set
        {
            _backgroundSprite = value;
        }
    }

    [SerializeField]
    private Color _backgroundColorEnabled = Color.white;
    public Color BackgroundColorEnabled
    {
        get
        {
            return _backgroundColorEnabled;
        }

        set
        {
            _backgroundColorEnabled = value;
        }
    }

    [SerializeField]
    private Color _backgroundColorDisabled = Color.grey;
    public Color BackgroundColorDisabled
    {
        get
        {
            return _backgroundColorDisabled;
        }

        set
        {
            _backgroundColorDisabled = value;
        }
    }

    [SerializeField]
    private Sprite _foregroundSprite = null;
    public Sprite ForegroundSprite
    {
        get
        {
            return _foregroundSprite;
        }

        set
        {
            _foregroundSprite = value;
        }
    }

    [SerializeField]
    private Color _foregroundColorEnabled = Color.white;
    public Color ForegroundColorEnabled
    {
        get
        {
            return _foregroundColorEnabled;
        }

        set
        {
            _foregroundColorEnabled = value;
        }
    }

    [SerializeField]
    private Color _foregroundColorDisabled = Color.grey;
    public Color ForegroundColorDisabled
    {
        get
        {
            return _foregroundColorDisabled;
        }

        set
        {
            _foregroundColorDisabled = value;
        }
    }

    [SerializeField]
    private float _foregroundMargin = 10;
    public float ForegroundMargin
    {
        get
        {
            return _foregroundMargin;
        }

        set
        {
            _foregroundMargin = value;
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

    private GameObject _backgroundGO;
    private Image _backgroundImage;
    private RectTransform _backgroundRectTrans;

    private GameObject _foregroundGO;
    private Image _foregroundImage;
    private RectTransform _foregroundRectTrans;

    private bool _disabled = false;
    public bool Disabled
    {
        get
        {
            return _disabled;
        }

    }
    
    public void Disable()
    {
        this._disabled = true;
    }

    public void Enable()
    {
        this._disabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //If disabled - return imidiatelly
        if (Disabled) return;

        if (OnClick != null) OnClick.Invoke();
    }

    private void Update()
    {
        _refreshColors();
    }

    private void _refreshColors()
    {
        if (Disabled)
        {
            _backgroundImage.color = BackgroundColorDisabled;
            _foregroundImage.color = ForegroundColorDisabled;
        }
        else
        {
            _backgroundImage.color = BackgroundColorEnabled;
            _foregroundImage.color = ForegroundColorEnabled;
        }
    }

    private void Awake()
    {
        _initBackground();
        _initForeground();
    }

    private void _initBackground()
    {
        //Setting background image
        this._backgroundGO = this.transform.Find("Background").gameObject;
        this._backgroundImage = _backgroundGO.GetComponent<Image>();
        this._backgroundImage.sprite = BackgroundSprite;
        this._backgroundImage.color = BackgroundColorEnabled;
        this._backgroundRectTrans = _backgroundGO.GetComponent<RectTransform>();
    }

    private void _initForeground()
    {
        this._foregroundGO = this.transform.Find("Foreground").gameObject;

        //Setting foreground image
        this._foregroundImage = _foregroundGO.GetComponent<Image>();
        this._foregroundImage.sprite = ForegroundSprite;
        this._foregroundImage.color = ForegroundColorEnabled;
        this._foregroundRectTrans = _foregroundGO.GetComponent<RectTransform>();

        //Setting margin to icon
        this._foregroundRectTrans.offsetMin = new Vector2(ForegroundMargin,ForegroundMargin);
        this._foregroundRectTrans.offsetMax = new Vector2(-ForegroundMargin, -ForegroundMargin);

    }
}