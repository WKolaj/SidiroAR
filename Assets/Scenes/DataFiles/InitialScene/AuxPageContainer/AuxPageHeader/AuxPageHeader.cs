using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuxPageHeader : MonoBehaviour
{

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

    private void _handleHidePageButtonClicked()
    {
        MainCanvas.HideAuxPage();
    }

    public void SetText(string text)
    {
        this._label.text = text;
    }

}
