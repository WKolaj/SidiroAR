using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
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

    private RectTransform _rectTrans = null;

    private GameObject _windowGO = null;
    private GameObject _textLabelGO = null;
    private GameObject _buttonContainerGO = null;

    private RectTransform _windowRectTrans = null;
    private RectTransform _textLabelRectTrans = null;
    private RectTransform _buttonContainerRectTrans = null;

    private TextMeshProUGUI _textLabelTMP = null;

    private void Awake()
    {
        this._rectTrans = this.GetComponent<RectTransform>();

        this._windowGO = this.transform.Find("Window").gameObject;
        this._textLabelGO = _windowGO.transform.Find("TextLabel").gameObject;
        this._buttonContainerGO = _windowGO.transform.Find("ButtonContainer").gameObject;

        this._windowRectTrans = _windowGO.GetComponent<RectTransform>();
        this._textLabelRectTrans = _textLabelGO.GetComponent<RectTransform>();
        this._buttonContainerRectTrans = _buttonContainerGO.GetComponent<RectTransform>();

        this._textLabelTMP = _textLabelGO.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _setWindowWidth();
    }

    private void _setWindowWidth()
    {
        var currentWidth = _rectTrans.rect.width;

        var widthToSet = currentWidth * WindowWidthPercentage / 100;

        this._windowRectTrans.sizeDelta = new Vector2(widthToSet, this._windowRectTrans.sizeDelta.y);
    }

}
