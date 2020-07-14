using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPage : MonoBehaviour
{
    private GameObject _auxPageHeaderGO = null;
    private AuxPageHeader _auxPageHeader = null;

    private void Awake()
    {
        this._auxPageHeaderGO = this.transform.Find("AuxPageHeader").gameObject;

        this._auxPageHeader = this._auxPageHeaderGO.GetComponent<AuxPageHeader>();
    }

    // Update is called once per frame
    void Update()
    {
        this._auxPageHeader.SetText(Translator.GetTranslation("SettingsPage.HeaderTitle"));
    }
}
