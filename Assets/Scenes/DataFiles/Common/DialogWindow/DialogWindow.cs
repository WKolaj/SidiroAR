using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogWindow : MonoBehaviour
{
    private static DialogWindow _actualDialogWindow = null;

    public static void HideStatic()
    {
        if (_actualDialogWindow != null)
            _actualDialogWindow.Hide();
    }

    public static void ShowStatic(string windowText = "", string button0Text = null, Action button0Method = null, string button0Color = "#FFFF0266", string button1Text = null, Action button1Method = null, string button1Color = "#FF3EACAB")
    {
        if (_actualDialogWindow != null)
            _actualDialogWindow.Show(windowText,button0Text,button0Method,button0Color,button1Text,button1Method,button1Color);
    }

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

    [SerializeField]
    private string _windowContentText = "Content text of window";
    public string WindowContentText
    {
        get
        {
            return _windowContentText;
        }

        set
        {
            _windowContentText = value;
        }
    }

    [SerializeField]
    private float _fontSizeWindowContent = 36;
    public float FontSizeWindowContent
    {
        get
        {
            return _fontSizeWindowContent;
        }

        set
        {
            _fontSizeWindowContent = value;
        }
    }


    [SerializeField]
    private Color _fontColorWindowContent = Color.white;
    public Color FontColorWindowContent
    {
        get
        {
            return _fontColorWindowContent;
        }

        set
        {
            _fontColorWindowContent = value;
        }
    }

    [SerializeField]
    private bool _showButton0 = true;
    public bool ShowButton0
    {
        get
        {
            return _showButton0;
        }

        set
        {
            _showButton0 = value;
        }
    }

    [SerializeField]
    private string _textButton0 = "Cancel";
    public string TextButton0
    {
        get
        {
            return _textButton0;
        }

        set
        {
            _textButton0 = value;
        }
    }

    [SerializeField]
    private float _fontSizeButton0 = 36;
    public float FontSizeButton0
    {
        get
        {
            return _fontSizeButton0;
        }

        set
        {
            _fontSizeButton0 = value;
        }
    }


    [SerializeField]
    private Color _fontColorButton0 = Color.white;
    public Color FontColorButton0
    {
        get
        {
            return _fontColorButton0;
        }

        set
        {
            _fontColorButton0 = value;
        }
    }


    [SerializeField]
    private bool _showButton1 = true;
    public bool ShowButton1
    {
        get
        {
            return _showButton1;
        }

        set
        {
            _showButton1 = value;
        }
    }

    [SerializeField]
    private string _textButton1 = "Apply";
    public string TextButton1
    {
        get
        {
            return _textButton1;
        }

        set
        {
            _textButton1 = value;
        }
    }

    [SerializeField]
    private float _fontSizeButton1 = 36;
    public float FontSizeButton1
    {
        get
        {
            return _fontSizeButton1;
        }

        set
        {
            _fontSizeButton1 = value;
        }
    }


    [SerializeField]
    private Color _fontColorButton1 = Color.white;
    public Color FontColorButton1
    {
        get
        {
            return _fontColorButton1;
        }

        set
        {
            _fontColorButton1 = value;
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
        DialogWindow._actualDialogWindow = this;

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
        _textLabelTMP.text = WindowContentText;
        _textLabelTMP.fontSize = FontSizeWindowContent;
        _textLabelTMP.color = FontColorWindowContent;

        //Init button 0
        if (ShowButton0) _button0GO.SetActive(true);
        else _button0GO.SetActive(false);

        _button0.LabelText = TextButton0;
        _button0.LabelFontSize = FontSizeButton0;
        _button0.LabelFontColor = FontColorButton0;

        //Init button 1
        if (ShowButton1) _button1GO.SetActive(true);
        else _button1GO.SetActive(false);

        _button1.LabelText = TextButton1;
        _button1.LabelFontSize = FontSizeButton1;
        _button1.LabelFontColor = FontColorButton1;

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
            _button0GO.SetActive(true);
            _button0.SetTextAndColor(button0Text, button0Color);
            _button0Method = button0Method;
        }
        else
        {
            _button0GO.SetActive(false);
        }

        //Initializng button1 only if it's label is not null
        if (!String.IsNullOrEmpty(button1Text))
        {
            _button1GO.SetActive(true);
            _button1.SetTextAndColor(button1Text, button1Color);
            _button1Method = button1Method;
        }
        else
        {
            _button1GO.SetActive(false);
        }

        this.gameObject.SetActive(true);
    }

}
