using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DialogWindowButton : MonoBehaviour
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

    private GameObject _labelGO = null;
    private GameObject _buttonMaskGO = null;

    private RectTransform _labelRectTrans = null;
    private RectTransform _buttonRectTrans = null;

    private TextMeshProUGUI _labelTMP  = null;
    private DialogWindowButtonMask _buttonMask = null;


    private void Awake()
    {
        this._labelGO = this.transform.Find("Label").gameObject;
        this._buttonMaskGO = this.transform.Find("ButtonMask").gameObject;

        this._labelRectTrans = this._labelGO.GetComponent<RectTransform>();
        this._buttonRectTrans = this._buttonMaskGO.GetComponent<RectTransform>();

        this._labelTMP = this._labelGO.GetComponent<TextMeshProUGUI>();
        this._buttonMask = this._buttonMaskGO.GetComponent<DialogWindowButtonMask>();

        this._buttonMask.OnClick = this.OnClick;
    }


    public void SetTextAndColor(string text, string color)
    {
        _labelTMP.text = text;

        Color newCol;

        if (ColorUtility.TryParseHtmlString(color, out newCol))
        {
            _labelTMP.color = newCol;
        }

    }

}
