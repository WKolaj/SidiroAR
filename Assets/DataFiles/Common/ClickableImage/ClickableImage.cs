using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ClickableImage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Color _backgroundColorEnabled = Color.white;
    public Color BackgroundColorEnabled
    {
        get
        {
            return _backgroundColorEnabled;
        }

        set
        {
            _backgroundColorEnabled = value;
        }
    }

    [SerializeField]
    private Color _backgroundColorDisabled = Color.grey;
    public Color BackgroundColorDisabled
    {
        get
        {
            return _backgroundColorDisabled;
        }

        set
        {
            _backgroundColorDisabled = value;
        }
    }

    [SerializeField]
    private Color _foregroundColorEnabled = Color.white;
    public Color ForegroundColorEnabled
    {
        get
        {
            return _foregroundColorEnabled;
        }

        set
        {
            _foregroundColorEnabled = value;
        }
    }

    [SerializeField]
    private Color _foregroundColorDisabled = Color.grey;
    public Color ForegroundColorDisabled
    {
        get
        {
            return _foregroundColorDisabled;
        }

        set
        {
            _foregroundColorDisabled = value;
        }
    }

    [SerializeField]
    private float _foregroundMargin = 10;
    public float ForegroundMargin
    {
        get
        {
            return _foregroundMargin;
        }

        set
        {
            _foregroundMargin = value;
        }
    }

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

    private GameObject _backgroundGO;
    private Image _backgroundImage;
    private RectTransform _backgroundRectTrans;

    private GameObject _foregroundGO;
    private Image _foregroundImage;
    private RectTransform _foregroundRectTrans;

    private bool _disabled = false;
    public bool Disabled
    {
        get
        {
            return _disabled;
        }

    }
    
    public void Disable()
    {
        this._disabled = true;
    }

    public void Enable()
    {
        this._disabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //If disabled - return imidiatelly
        if (Disabled) return;

        if (OnClick != null) OnClick.Invoke();
    }

    private void Update()
    {
        _refreshColors();
    }

    private void _refreshColors()
    {
        if (Disabled)
        {
            _backgroundImage.color = BackgroundColorDisabled;
            _foregroundImage.color = ForegroundColorDisabled;
        }
        else
        {
            _backgroundImage.color = BackgroundColorEnabled;
            _foregroundImage.color = ForegroundColorEnabled;
        }
    }

    //ITEM DEPENDS ON SIZE AFTER UI SCALING - SO METHODS MUST BE INVOKED AFTER SCALING (IN START), AWAKE IS USED TO SET UP REFERENCES
    private void Awake()
    {

        this._foregroundGO = this.transform.Find("Foreground").gameObject;
        this._foregroundImage = _foregroundGO.GetComponent<Image>();
        this._foregroundRectTrans = _foregroundGO.GetComponent<RectTransform>();

        this._backgroundGO = this.transform.Find("Background").gameObject;
        this._backgroundImage = _backgroundGO.GetComponent<Image>();
        this._backgroundRectTrans = _backgroundGO.GetComponent<RectTransform>();
    }

    //ITEM DEPENDS ON SIZE AFTER UI SCALING - SO METHODS MUST BE INVOKED AFTER SCALING (IN START), START IS USED TO SET UP UI
    private void Start()
    {
        //Setting foreground image
        this._foregroundImage.color = ForegroundColorEnabled;

        //Setting margin to icon
        this._foregroundRectTrans.offsetMin = new Vector2(ForegroundMargin, ForegroundMargin);
        this._foregroundRectTrans.offsetMax = new Vector2(-ForegroundMargin, -ForegroundMargin);

        //Setting background image
        this._backgroundImage.color = BackgroundColorEnabled;
    }

}
