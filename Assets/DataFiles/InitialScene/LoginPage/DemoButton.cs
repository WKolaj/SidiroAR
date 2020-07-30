using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DemoButton : MonoBehaviour
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

    private GameObject _loginButtonMaskGO;
    private GameObject _backgroundGO;
    private GameObject _textLabelGO;

    private LoginButtonMask _loginButtonMask;
    private Image _background;
    private TextMeshProUGUI _textLabel;


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

    public void SetText(string text)
    {
        this._textLabel.text = text;
    }
}
