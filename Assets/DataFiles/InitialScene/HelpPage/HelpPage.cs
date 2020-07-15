using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpPage : MonoBehaviour
{
    private GameObject _auxPageHeaderGO = null;
    private GameObject _linkButtonGO = null;

    private AuxPageHeader _auxPageHeader = null;
    private LinkButton _linkButton = null;

    private void Awake()
    {
        this._auxPageHeaderGO = this.transform.Find("AuxPageHeader").gameObject;
        this._linkButtonGO = this.transform.Find("LinkButton").gameObject;

        this._auxPageHeader = this._auxPageHeaderGO.GetComponent<AuxPageHeader>();
        this._linkButton = this._linkButtonGO.GetComponent<LinkButton>();
    }

    // Update is called once per frame
    void Update()
    {
        this._auxPageHeader.SetText(Translator.GetTranslation("HelpPage.HeaderTitle"));
        this._linkButton.SetTextAndURL(Translator.GetTranslation("HelpPage.LinkButtonText"), Translator.GetTranslation("HelpPage.URL"));
    }
}
