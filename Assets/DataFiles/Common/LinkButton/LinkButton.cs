using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LinkButton : MonoBehaviour
{
    private string _url = @"https://docs.unity3d.com/ScriptReference/Application.OpenURL.html";
    public string URL
    {
        get
        {
            return _url;
        }
    }

    private GameObject _buttonMaskGO;
    private GameObject _textLabelGO;

    private LinkButtonMask _linkButtonMask;
    private TextMeshProUGUI _textLabel;

    private void Awake()
    {
        this._buttonMaskGO = transform.Find("ButtonMask").gameObject;
        this._textLabelGO = transform.Find("Label").gameObject;

        this._linkButtonMask = _buttonMaskGO.GetComponent<LinkButtonMask>();
        this._textLabel = _textLabelGO.GetComponent<TextMeshProUGUI>();

        this._linkButtonMask.OnClick.AddListener(_handleClick);
    }

    private void _handleClick()
    {
        if(!string.IsNullOrEmpty(URL))
        {
            Application.OpenURL(URL);
        }
    }

    public void SetTextAndURL(string text, string url)
    {
        this._textLabel.text = text;
        this._url = url;
    }
}
