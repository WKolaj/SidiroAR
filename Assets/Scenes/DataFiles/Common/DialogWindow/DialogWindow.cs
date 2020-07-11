using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogWindow : MonoBehaviour
{

    [SerializeField]
    private float _windowWidthPercentage = 50;
    public float WindowWidthPercentage
    {
        get
        {
            return _windowWidthPercentage;
        }

        set
        {
            _windowWidthPercentage = value;
        }
    }

    [SerializeField]
    private float _maxWidth = 800;
    public float MaxWidth
    {
        get
        {
            return _maxWidth;
        }

        set
        {
            _maxWidth = value;
        }
    }

    private Action _button0Method = null;
    private Action _button1Method = null;

    private RectTransform _rectTrans = null;

    private GameObject _windowGO = null;
    private GameObject _textLabelGO = null;
    private GameObject _buttonContainerGO = null;
    private GameObject _button0GO = null;
    private GameObject _button1GO = null;

    private RectTransform _windowRectTrans = null;
    private RectTransform _textLabelRectTrans = null;
    private RectTransform _buttonContainerRectTrans = null;
    private RectTransform _button0RectTrans = null;
    private RectTransform _button1RectTrans = null;

    private TextMeshProUGUI _textLabelTMP = null;
    private DialogWindowButton _button0 = null;
    private DialogWindowButton _button1 = null;

    private void Awake()
    {

        this._rectTrans = this.GetComponent<RectTransform>();

        this._windowGO = this.transform.Find("Window").gameObject;
        this._textLabelGO = _windowGO.transform.Find("TextLabel").gameObject;
        this._buttonContainerGO = _windowGO.transform.Find("ButtonContainer").gameObject;
        this._button0GO = this._buttonContainerGO.transform.Find("Button0").gameObject;
        this._button1GO = this._buttonContainerGO.transform.Find("Button1").gameObject;

        this._windowRectTrans = _windowGO.GetComponent<RectTransform>();
        this._textLabelRectTrans = _textLabelGO.GetComponent<RectTransform>();
        this._buttonContainerRectTrans = _buttonContainerGO.GetComponent<RectTransform>();
        this._button0RectTrans = _button0GO.GetComponent<RectTransform>();
        this._button1RectTrans = _button1GO.GetComponent<RectTransform>();

        this._textLabelTMP = _textLabelGO.GetComponent<TextMeshProUGUI>();
        this._button0 = _button0GO.GetComponent<DialogWindowButton>();
        this._button1 = _button1GO.GetComponent<DialogWindowButton>();

        this._button0.OnClick.AddListener(_handleButton0Clicked);
        this._button1.OnClick.AddListener(_handleButton1Clicked);
    }

    private void Start()
    {
        //Init window
        _setWindowWidth();

        this.gameObject.SetActive(false);
    }

    private void _setWindowWidth()
    {
        var currentWidth = _rectTrans.rect.width;

        var widthToSet = currentWidth * WindowWidthPercentage / 100;

        if (widthToSet > MaxWidth) widthToSet = MaxWidth;

        this._windowRectTrans.sizeDelta = new Vector2(widthToSet, this._windowRectTrans.sizeDelta.y);
    }

    private void _handleButton0Clicked()
    {
        if (_button0Method != null)
            _button0Method();

        Hide();
    }

    private void _handleButton1Clicked()
    {
        if (_button1Method != null)
            _button1Method();

        Hide();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(string windowText = "", string button0Text=null, Action button0Method = null, string button0Color = "#FFFF0266", string button1Text = null, Action button1Method = null, string button1Color = "#FF3EACAB")
    {
        _textLabelTMP.text = windowText;

        //Initializng button0 only if it's label is not null
        if(!String.IsNullOrEmpty(button0Text))
        {
            _button0.SetTextAndColor(button0Text, button0Color);
            _button0Method = button0Method;
            _button0GO.SetActive(true);
        }
        else
        {
            _button0GO.SetActive(false);
        }

        //Initializng button1 only if it's label is not null
        if (!String.IsNullOrEmpty(button1Text))
        {
            _button1.SetTextAndColor(button1Text, button1Color);
            _button1Method = button1Method;
            _button1GO.SetActive(true);
        }
        else
        {
            _button1GO.SetActive(false);
        }

        this.gameObject.SetActive(true);
    }

}
