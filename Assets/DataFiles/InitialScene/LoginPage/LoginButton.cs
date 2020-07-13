using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour
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


    private GameObject _loginButtonMaskGO;
    private GameObject _backgroundGO;
    private GameObject _textLabelGO;

    private LoginButtonMask _loginButtonMask;
    private Image _background;
    private TextMeshProUGUI _textLabel;

    public bool Disabled
    {
        get
        {
            return _loginButtonMask.Disabled;
        }

    }

    public void Disable()
    {
        _loginButtonMask.Disabled = true;
    }

    public void Enable()
    {
        _loginButtonMask.Disabled = false;
    }


    private void Awake()
    {
        this._loginButtonMaskGO = transform.Find("ButtonMask").gameObject;
        this._backgroundGO = transform.Find("Background").gameObject;
        this._textLabelGO = transform.Find("Label").gameObject;

        this._loginButtonMask = _loginButtonMaskGO.GetComponent<LoginButtonMask>();
        this._background = _backgroundGO.GetComponent<Image>();
        this._textLabel = _textLabelGO.GetComponent<TextMeshProUGUI>();


        this._loginButtonMask.OnClick = this.OnClick;
    }

    private void Update()
    {
        _refreshColors();
    }

    private void _refreshColors()
    {
        if (Disabled)
        {
            _background.color = BackgroundColorDisabled;
            _textLabel.color = ForegroundColorDisabled;
        }
        else
        {
            _background.color = BackgroundColorEnabled;
            _textLabel.color = ForegroundColorEnabled;
        }
    }

    public void SetText(string text)
    {
        this._textLabel.text = text;    
    }
}
