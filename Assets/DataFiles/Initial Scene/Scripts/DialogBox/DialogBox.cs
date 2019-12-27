using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DialogBoxType
{
    Ok, OkCancel, YesNo
}
public enum DialogBoxMode
{
    Question, Warning, Info
}


public class DialogBox : MonoBehaviour
{
    public event Action onOkClicked;
    public event Action onCancelClicked;
    public event Action onYesClicked;
    public event Action onNoClicked;

    /// <summary>
    /// Sprite determining icon for info
    /// </summary>
    [SerializeField]
    private Sprite _infoIconSprite;
    public Sprite InfoIconSprite
    {
        get
        {
            return _infoIconSprite;
        }

        set
        {
            _infoIconSprite = value;
        }
    }

    /// <summary>
    /// Sprite determining icon for question
    /// </summary>
    [SerializeField]
    private Sprite _questionIconSprite;
    public Sprite QuestionIconSprite
    {
        get
        {
            return _questionIconSprite;
        }

        set
        {
            _questionIconSprite = value;
        }
    }

    /// <summary>
    /// Sprite determining icon for warning
    /// </summary>
    [SerializeField]
    private Sprite _warningIconSprite;
    public Sprite WarningIconSprite
    {
        get
        {
            return _warningIconSprite;
        }

        set
        {
            _warningIconSprite = value;
        }
    }

    /// <summary>
    /// Mode determining Dialog window type (buttons)
    /// </summary>
    private DialogBoxType _type = DialogBoxType.Ok;
    public DialogBoxType Type
    {
        get
        {
            return _type;
        }
    }

    /// <summary>
    /// Mode determining Dialog window mode (question, warning, info)
    /// </summary>
    private DialogBoxMode _mode = DialogBoxMode.Info;
    public DialogBoxMode Mode
    {
        get
        {
            return _mode;
        }
    }

    private GameObject infoIcon;
    private GameObject infoTitleLabel;
    private GameObject infoContentLabel;
    private GameObject okButton;
    private GameObject cancelButton;
    private GameObject yesButton;
    private GameObject noButton;
    private GameObject okButtonLabel;
    private GameObject cancelButtonLabel;
    private GameObject yesButtonLabel;
    private GameObject noButtonLabel;

    // Start is called before the first frame update
    void Awake()
    {
        InitComponents();
        SetMode(DialogBoxMode.Question);
        SetType(DialogBoxType.YesNo);
    }


    /// <summary>
    /// Method for initialising (assigning) all compoenents
    /// </summary>
    void InitComponents()
    {
        var infoItem = transform.Find("InfoItem").gameObject;
        infoIcon = infoItem.transform.Find("InfoIcon").gameObject;

        var infoContainer = infoItem.transform.Find("InfoContainer").gameObject;
        infoTitleLabel = infoContainer.transform.Find("InfoTitleLabel").gameObject;
        infoContentLabel = infoContainer.transform.Find("InfoContentLabel").gameObject;

        var buttonsItemContainer = transform.Find("ButtonsItemContainer").gameObject;
        okButton = buttonsItemContainer.transform.Find("OkButton").gameObject;
        cancelButton = buttonsItemContainer.transform.Find("CancelButton").gameObject;
        yesButton = buttonsItemContainer.transform.Find("YesButton").gameObject;
        noButton = buttonsItemContainer.transform.Find("NoButton").gameObject;
        okButtonLabel = okButton.transform.Find("Label").gameObject;
        cancelButtonLabel = cancelButton.transform.Find("Label").gameObject;
        yesButtonLabel = yesButton.transform.Find("Label").gameObject;
        noButtonLabel = noButton.transform.Find("Label").gameObject;

        okButton.GetComponent<Button>().onClick.AddListener(handleOkButtonClicked);
        cancelButton.GetComponent<Button>().onClick.AddListener(handleCancelButtonClicked);
        yesButton.GetComponent<Button>().onClick.AddListener(handleYesButtonClicked);
        noButton.GetComponent<Button>().onClick.AddListener(handleNoButtonClicked);
    }

    private void handleOkButtonClicked()
    {
        if (onOkClicked != null) onOkClicked();
        Destroy(gameObject);
    }

    private void handleCancelButtonClicked()
    {
        if (onCancelClicked != null) onCancelClicked();
        Destroy(gameObject);
    }
    private void handleYesButtonClicked()
    {
        if (onYesClicked != null) onYesClicked();
        Destroy(gameObject);
    }
    private void handleNoButtonClicked()
    {
        if (onNoClicked != null) onNoClicked();
        Destroy(gameObject);
    }

    /// <summary>
    /// Method for setting mode of dialog box
    /// </summary>
    /// <param name="newMode">
    /// new mode
    /// </param>
    public void SetMode(DialogBoxMode newMode)
    {
        if(newMode == DialogBoxMode.Info)
        {
            var iconImage = infoIcon.GetComponent<Image>().sprite = InfoIconSprite;
            this._mode = newMode;
        }
        else if(newMode == DialogBoxMode.Question)
        {
            var iconImage = infoIcon.GetComponent<Image>().sprite = QuestionIconSprite;
            this._mode = newMode;
        }
        else if(newMode == DialogBoxMode.Warning)
        {
            var iconImage = infoIcon.GetComponent<Image>().sprite = WarningIconSprite;
            this._mode = newMode;
        }
    }

    /// <summary>
    /// Method for setting type of dialog box
    /// </summary>
    /// <param name="newType">
    /// new type
    /// </param>
    public void SetType(DialogBoxType newType)
    {
        if (newType == DialogBoxType.Ok)
        {
            okButton.SetActive(true);
            cancelButton.SetActive(false);
            yesButton.SetActive(false);
            noButton.SetActive(false);

            this._type = newType;

        }
        else if (newType == DialogBoxType.OkCancel)
        {
            okButton.SetActive(true);
            cancelButton.SetActive(true);
            yesButton.SetActive(false);
            noButton.SetActive(false);

            this._type = newType;
        }
        else if (newType == DialogBoxType.YesNo)
        {
            okButton.SetActive(false);
            cancelButton.SetActive(false);
            yesButton.SetActive(true);
            noButton.SetActive(true);

            this._type = newType;
        }
    }

    /// <summary>
    /// Method for setting title of dialog box
    /// </summary>
    /// <param name="titleText">
    /// New title of dialog box
    /// </param>
    public void SetTitle(string titleText)
    {
        TextMeshProUGUI titleTextMeshPro = infoTitleLabel.GetComponent<TextMeshProUGUI>();
        titleTextMeshPro.SetText(titleText);
    }

    /// <summary>
    /// Method for setting content text of dialog box
    /// </summary>
    /// <param name="contentText">
    /// Content text of dialog box
    /// </param>
    public void SetContent(string contentText)
    {
        TextMeshProUGUI contentTextMeshPro = infoContentLabel.GetComponent<TextMeshProUGUI>();
        contentTextMeshPro.SetText(contentText);
    }

}
