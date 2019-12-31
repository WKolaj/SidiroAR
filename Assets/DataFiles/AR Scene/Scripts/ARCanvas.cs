using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject _dialogBoxPrefab;
    /// <summary>
    /// Prefab of file browser
    /// </summary>
    public GameObject DialogBoxPrefab
    {
        get
        {
            return _dialogBoxPrefab;
        }

        set
        {
            _dialogBoxPrefab = value;
        }
    }

    /// <summary>
    /// Actual dialog box - shown in order to check no to instantiate more than one dialog box at once
    /// </summary>
    private GameObject actualDialogBox = null;



    /// <summary>
    /// Method for showing new dialog box
    /// </summary>
    /// <param name="titleText">
    /// Title of dialog box
    /// </param>
    /// <param name="contentText">
    /// Content of dialog box
    /// </param>
    /// <param name="mode">
    /// Mode of dialog box
    /// </param>
    /// <param name="type">
    /// Type of dialog box
    /// </param>
    /// <returns>
    /// Dialog box script
    /// </returns>
    public DialogBoxWindow ShowDialogBox(string titleText, string contentText, DialogBoxMode mode, DialogBoxType type)
    {
        if (actualDialogBox) Destroy(actualDialogBox);

        GameObject dialogBox = Instantiate(DialogBoxPrefab, transform);
        dialogBox.name = "DialogBox";

        actualDialogBox = dialogBox;

        DialogBoxWindow dialogBoxScript = dialogBox.GetComponent<DialogBoxWindow>();

        dialogBoxScript.SetMode(mode);
        dialogBoxScript.SetType(type);
        dialogBoxScript.SetTitle(titleText);
        dialogBoxScript.SetContent(contentText);

        return dialogBoxScript;
    }

}
