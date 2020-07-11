using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpPage : MonoBehaviour
{
    private GameObject _auxPageHeaderGO = null;
    private GameObject _mainTitleLabelGO = null;

    private AuxPageHeader _auxPageHeader = null;
    private TextMeshProUGUI _mainTitleLabel = null;

    private void Awake()
    {
        this._auxPageHeaderGO = this.transform.Find("AuxPageHeader").gameObject;
        this._mainTitleLabelGO = this._auxPageHeaderGO.transform.Find("Label").gameObject;

        this._auxPageHeader = this._auxPageHeaderGO.GetComponent<AuxPageHeader>();
        this._mainTitleLabel = this._mainTitleLabelGO.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        this._mainTitleLabel.text = Translator.GetTranslation("HelpPage.HeaderTitle");
    }
}
