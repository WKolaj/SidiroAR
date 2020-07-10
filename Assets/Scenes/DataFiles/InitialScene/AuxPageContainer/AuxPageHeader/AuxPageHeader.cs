using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuxPageHeader : MonoBehaviour
{
    [SerializeField]
    private bool _buttonVisible = true;
    public bool ButtonVisible
    {
        get
        {
            return _buttonVisible;
        }

        set
        {
            _buttonVisible = value;
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


    private GameObject _hidePageButtonGO;
    private GameObject _labelGO;
    private GameObject _separatorGO;

    private RectTransform _hidePageButtonRectTrans;
    private RectTransform _labelRectTrans;
    private RectTransform _separatorRectTrans;

    private ClickableImage _hidePageButton;
    private TextMeshProUGUI _label;
    private Image _separator;

    void Awake()
    {
        this._hidePageButtonGO = this.transform.Find("HidePageButton").gameObject;
        this._labelGO = this.transform.Find("Label").gameObject;
        this._separatorGO = this.transform.Find("Separator").gameObject;

        this._hidePageButtonRectTrans = _hidePageButtonGO.GetComponent<RectTransform>();
        this._labelRectTrans = _labelGO.GetComponent<RectTransform>();
        this._separatorRectTrans = _separatorGO.GetComponent<RectTransform>();

        this._hidePageButton = _hidePageButtonGO.GetComponent<ClickableImage>();
        this._label = _labelGO.GetComponent<TextMeshProUGUI>();
        this._separator = _separatorGO.GetComponent<Image>();

        this._hidePageButton.OnClick.AddListener(_handleHidePageButtonClicked);
    }

    private void Start()
    {
        this._label.text = LabelText;

        if(this.ButtonVisible)
        {
            _hidePageButtonGO.SetActive(true);
        }
        else
        {
            _hidePageButtonGO.SetActive(false);
        }
    }

    private void _handleHidePageButtonClicked()
    {
        AuxPageContainer.DrawInStatic();
    }

}
