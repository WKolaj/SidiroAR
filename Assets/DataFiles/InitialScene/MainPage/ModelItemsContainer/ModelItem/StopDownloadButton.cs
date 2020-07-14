using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopDownloadButton : MonoBehaviour
{
    [SerializeField]
    private Button.ButtonClickedEvent _onClick = null;
    public Button.ButtonClickedEvent OnClick
    {
        get
        {
            return _onClick;
        }

        set
        {
            _onClick = value;
        }
    }


    private RectTransform _rectTrans= null; 

    private GameObject _iconGO = null;
    private GameObject _labelGO = null;
    private GameObject _buttonMaskGO = null;
    private GameObject _progressBarGO = null;

    private Image _icon = null;
    private TextMeshProUGUI _labelTMP = null;
    private ModelItemButtonMask _buttonMask = null;
    private RectTransform _progressBarRectTrans = null;

    public float ProgressBarPercentage
    {
        get
        {
            return 100 * (_progressBarRectTrans.rect.width + 20)/ (_rectTrans.rect.width);
        }

    }


    private void Awake()
    {
        this._rectTrans = this.transform.GetComponent<RectTransform>();

        this._iconGO = this.transform.Find("Icon").gameObject;
        this._labelGO = this.transform.Find("Label").gameObject;
        this._buttonMaskGO = this.transform.Find("ButtonMask").gameObject;
        this._progressBarGO = this.transform.Find("ProgressBar").gameObject;

        this._icon = this._iconGO.GetComponent<Image>();
        this._labelTMP = this._labelGO.GetComponent<TextMeshProUGUI>();
        this._buttonMask = this._buttonMaskGO.GetComponent<ModelItemButtonMask>();
        this._progressBarRectTrans = this._progressBarGO.GetComponent<RectTransform>();


        this._buttonMask.OnClick = this.OnClick;

        SetBarPercentage(0);
    }



    public void SetText(string text)
    {
        _labelTMP.text = text;
    }


    public void SetBarPercentage(float value)
    {
        //20 pixel is an offset from right side!
        this._progressBarRectTrans.sizeDelta = new Vector2((value / 100) * (_rectTrans.sizeDelta.x - 20), _progressBarRectTrans.sizeDelta.y);
    }

    public void SetWidth(float width)
    {
        //Getting percentage before resizing
        var percentageBefore = ProgressBarPercentage;

        //Setting size - must be in start, after parent total initialization
        _rectTrans.sizeDelta = new Vector2(width, _rectTrans.sizeDelta.y);

        //Setting the same percentage after
        SetBarPercentage(percentageBefore);
    }
}
