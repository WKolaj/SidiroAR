using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsPage : MonoBehaviour
{
    /// <summary>
    /// Collection containg all scale elements - MUST BE IN THE SAME ORDER AS IN DROPDOWN MENU!
    /// </summary>
    private float[] _scaleList = new float[] { 1, 0.5f, 0.2f, 0.1f };

    private GameObject _auxPageHeaderGO = null;
    private GameObject _scaleDropDownListGO = null;
    private GameObject _scaleDropDownLabelGO = null;

    private AuxPageHeader _auxPageHeader = null;
    private TMP_Dropdown _scaleDropDownList = null;
    private TextMeshProUGUI _scaleDropDownLabel = null;

    private void Awake()
    {
        this._auxPageHeaderGO = this.transform.Find("AuxPageHeader").gameObject;
        var scaleComboBox = this.transform.Find("ScaleComboBox").gameObject;
        this._scaleDropDownLabelGO = scaleComboBox.transform.Find("Label").gameObject;
        this._scaleDropDownListGO = scaleComboBox.transform.Find("Dropdown").gameObject;

        this._auxPageHeader = this._auxPageHeaderGO.GetComponent<AuxPageHeader>();
        this._scaleDropDownLabel = this._scaleDropDownLabelGO.GetComponent<TextMeshProUGUI>();
        this._scaleDropDownList = this._scaleDropDownListGO.GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        //Setting initial index of scale list
        _setScaleDropDownSelectedItem(Common.Scale);
    }

    private void _setScaleDropDownSelectedItem(float scale)
    {
        //Finding index in array
        int indexOfScale = Array.IndexOf(this._scaleList, scale);

        this._scaleDropDownList.SetValueWithoutNotify(indexOfScale);
    }

    // Update is called once per frame
    void Update()
    {
        this._auxPageHeader.SetText(Translator.GetTranslation("SettingsPage.HeaderTitle"));

        SetScaleLabelText(Translator.GetTranslation("SettingsPage.ScaleDropDown.LabelText"));

        SetScaleDropDownOptionsText(
            Translator.GetTranslation("SettingsPage.ScaleDropDown.ScaleOption1"),
            Translator.GetTranslation("SettingsPage.ScaleDropDown.ScaleOption1_2"),
            Translator.GetTranslation("SettingsPage.ScaleDropDown.ScaleOption1_5"),
            Translator.GetTranslation("SettingsPage.ScaleDropDown.ScaleOption1_10")
            );

    }

    public void HandleScaleDropdownValueChanged(int newValue)
    {
        if (newValue >= 0 && newValue < _scaleList.Length)
            Common.Scale = _scaleList[newValue];
    }

    public void SetScaleLabelText(string text)
    {
        this._scaleDropDownLabel.text = text;
    }

    public void SetScaleDropDownOptionsText(string scale1option,string scale1_2option, string scale1_5option, string scale1_10option)
    {

        this._scaleDropDownList.options[0].text = scale1option;
        this._scaleDropDownList.options[1].text = scale1_2option;
        this._scaleDropDownList.options[2].text = scale1_5option;
        this._scaleDropDownList.options[3].text = scale1_10option;

        //Changing value of currently selected item
        this._scaleDropDownList.RefreshShownValue();
    }
}
