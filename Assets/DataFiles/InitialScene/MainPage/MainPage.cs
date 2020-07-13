using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainPage : MonoBehaviour
{
    private GameObject _modelsLabelGO = null;

    private TextMeshProUGUI _modelsLabel = null;

    private void Awake()
    {
        this._modelsLabelGO = transform.Find("ModelsLabel").gameObject;

        this._modelsLabel = _modelsLabelGO.GetComponent<TextMeshProUGUI>();


    }

    // Update is called once per frame
    void Update()
    {
        this._modelsLabel.SetText(Translator.GetTranslation("MainPage.ModelsLabelText"));
    }
}
