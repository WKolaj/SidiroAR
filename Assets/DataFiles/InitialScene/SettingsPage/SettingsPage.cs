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

    /// <summary>
    /// Collection containg all postion delta elements - MUST BE IN THE SAME ORDER AS IN DROPDOWN MENU!
    /// </summary>
    private float[] _positionDeltaList = new float[] { 0.005f, 0.01f, 0.02f, 0.05f, 0.1f };

    /// <summary>
    /// Collection containg all rotation delta elements - MUST BE IN THE SAME ORDER AS IN DROPDOWN MENU!
    /// </summary>
    private float[] _rotationDeltaList = new float[] { 0.25f, 0.5f, 1f, 2f, 5f };

    private GameObject _auxPageHeaderGO = null;
    private GameObject _scaleDropDownListGO = null;
    private GameObject _scaleDropDownLabelGO = null;
    private GameObject _positionDeltaDropDownListGO = null;
    private GameObject _positionDeltaDropDownLabelGO = null;
    private GameObject _rotationDeltaDropDownListGO = null;
    private GameObject _rotationDeltaDropDownLabelGO = null;

    private AuxPageHeader _auxPageHeader = null;
    private TMP_Dropdown _scaleDropDownList = null;
    private TextMeshProUGUI _scaleDropDownLabel = null;
    private TMP_Dropdown _positionDeltaDropDownList = null;
    private TextMeshProUGUI _positionDeltaDropDownLabel = null;
    private TMP_Dropdown _rotationDeltaDropDownList = null;
    private TextMeshProUGUI _rotationDeltaDropDownLabel = null;

    private void Awake()
    {
        this._auxPageHeaderGO = this.transform.Find("AuxPageHeader").gameObject;
        var scaleComboBox = this.transform.Find("ScaleComboBox").gameObject;
        this._scaleDropDownLabelGO = scaleComboBox.transform.Find("Label").gameObject;
        this._scaleDropDownListGO = scaleComboBox.transform.Find("Dropdown").gameObject;

        var positionDeltaComboBox = this.transform.Find("PositionDeltaComboBox").gameObject;
        this._positionDeltaDropDownLabelGO = positionDeltaComboBox.transform.Find("Label").gameObject;
        this._positionDeltaDropDownListGO = positionDeltaComboBox.transform.Find("Dropdown").gameObject;

        var rotationDeltaComboBox = this.transform.Find("RotationDeltaComboBox").gameObject;
        this._rotationDeltaDropDownLabelGO = rotationDeltaComboBox.transform.Find("Label").gameObject;
        this._rotationDeltaDropDownListGO = rotationDeltaComboBox.transform.Find("Dropdown").gameObject;

        this._auxPageHeader = this._auxPageHeaderGO.GetComponent<AuxPageHeader>();
        this._scaleDropDownLabel = this._scaleDropDownLabelGO.GetComponent<TextMeshProUGUI>();
        this._scaleDropDownList = this._scaleDropDownListGO.GetComponent<TMP_Dropdown>();
        this._positionDeltaDropDownLabel = this._positionDeltaDropDownLabelGO.GetComponent<TextMeshProUGUI>();
        this._positionDeltaDropDownList = this._positionDeltaDropDownListGO.GetComponent<TMP_Dropdown>();
        this._rotationDeltaDropDownLabel = this._rotationDeltaDropDownLabelGO.GetComponent<TextMeshProUGUI>();
        this._rotationDeltaDropDownList = this._rotationDeltaDropDownListGO.GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        //Setting initial index of scale list
        _setScaleDropDownSelectedItem(Common.Scale);
        _setPositionDeltaDropDownSelectedItem(Common.PrecisePositionPositionDelta);
        _setRotationDeltaDropDownSelectedItem(Common.PrecisePositionRotateDelta);
    }

    private void _setScaleDropDownSelectedItem(float scale)
    {
        //Finding index in array
        int indexOfScale = Array.IndexOf(this._scaleList, scale);

        this._scaleDropDownList.SetValueWithoutNotify(indexOfScale);
    }

    private void _setPositionDeltaDropDownSelectedItem(float position)
    {
        //Finding index in array
        int indexOfPosition = Array.IndexOf(this._positionDeltaList, position);

        this._positionDeltaDropDownList.SetValueWithoutNotify(indexOfPosition);
    }

    private void _setRotationDeltaDropDownSelectedItem(float rotation)
    {
        //Finding index in array
        int indexOfRotation = Array.IndexOf(this._rotationDeltaList, rotation);

        this._rotationDeltaDropDownList.SetValueWithoutNotify(indexOfRotation);
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


        SetPositionDeltaLabelText(Translator.GetTranslation("SettingsPage.PositionDeltaDropDown.LabelText"));

        SetPositionDeltaDropDownOptionsText(
            Translator.GetTranslation("SettingsPage.PositionDeltaDropDown.PositionOption05"),
            Translator.GetTranslation("SettingsPage.PositionDeltaDropDown.PositionOption10"),
            Translator.GetTranslation("SettingsPage.PositionDeltaDropDown.PositionOption20"),
            Translator.GetTranslation("SettingsPage.PositionDeltaDropDown.PositionOption50"),
            Translator.GetTranslation("SettingsPage.PositionDeltaDropDown.PositionOption100")
            );

        SetRotationDeltaLabelText(Translator.GetTranslation("SettingsPage.RotationDeltaDropDown.LabelText"));

        SetRotationDeltaDropDownOptionsText(
            Translator.GetTranslation("SettingsPage.RotationDeltaDropDown.RotationOption025"),
            Translator.GetTranslation("SettingsPage.RotationDeltaDropDown.RotationOption050"),
            Translator.GetTranslation("SettingsPage.RotationDeltaDropDown.RotationOption100"),
            Translator.GetTranslation("SettingsPage.RotationDeltaDropDown.RotationOption200"),
            Translator.GetTranslation("SettingsPage.RotationDeltaDropDown.RotationOption500")
            );
    }

    public void HandleScaleDropdownValueChanged(int newValue)
    {
        if (newValue >= 0 && newValue < _scaleList.Length)
            Common.Scale = _scaleList[newValue];
    }

    public void HandlePositionDeltaDropdownValueChanged(int newValue)
    {
        if (newValue >= 0 && newValue < _positionDeltaList.Length)
            Common.PrecisePositionPositionDelta = _positionDeltaList[newValue];
    }

    public void HandleRotationDeltaDropdownValueChanged(int newValue)
    {
        if (newValue >= 0 && newValue < _rotationDeltaList.Length)
            Common.PrecisePositionRotateDelta = _rotationDeltaList[newValue];
    }

    public void SetScaleLabelText(string text)
    {
        this._scaleDropDownLabel.text = text;
    }

    public void SetPositionDeltaLabelText(string text)
    {
        this._positionDeltaDropDownLabel.text = text;
    }

    public void SetRotationDeltaLabelText(string text)
    {
        this._rotationDeltaDropDownLabel.text = text;
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

    public void SetPositionDeltaDropDownOptionsText(string position05option, string position10option, string position20option, string position50option, string position100option)
    {

        this._positionDeltaDropDownList.options[0].text = position05option;
        this._positionDeltaDropDownList.options[1].text = position10option;
        this._positionDeltaDropDownList.options[2].text = position20option;
        this._positionDeltaDropDownList.options[3].text = position50option;
        this._positionDeltaDropDownList.options[4].text = position100option;

        //Changing value of currently selected item
        this._positionDeltaDropDownList.RefreshShownValue();
    }

    public void SetRotationDeltaDropDownOptionsText(string rotation025option, string rotation050option, string rotation100option, string rotation200option, string rotation500option)
    {

        this._rotationDeltaDropDownList.options[0].text = rotation025option;
        this._rotationDeltaDropDownList.options[1].text = rotation050option;
        this._rotationDeltaDropDownList.options[2].text = rotation100option;
        this._rotationDeltaDropDownList.options[3].text = rotation200option;
        this._rotationDeltaDropDownList.options[4].text = rotation500option;

        //Changing value of currently selected item
        this._rotationDeltaDropDownList.RefreshShownValue();
    }
}
