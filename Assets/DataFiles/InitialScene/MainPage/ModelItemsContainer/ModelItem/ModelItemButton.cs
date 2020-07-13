using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModelItemButton : MonoBehaviour
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
    private GameObject _buttonMaskGO = null;

    private Image _icon = null;
    private TextMeshProUGUI _labelTMP = null;
    private ModelItemButtonMask _buttonMask = null;


    private void Awake()
    {
        this._iconGO = this.transform.Find("Icon").gameObject;
        this._labelGO = this.transform.Find("Label").gameObject;
        this._buttonMaskGO = this.transform.Find("ButtonMask").gameObject;

        this._icon = this._iconGO.GetComponent<Image>();
        this._labelTMP = this._labelGO.GetComponent<TextMeshProUGUI>();
        this._buttonMask = this._buttonMaskGO.GetComponent<ModelItemButtonMask>();

        this._buttonMask.OnClick = this.OnClick;
    }

    public void SetText(string text)
    {
        _labelTMP.text = text;
    }
}
