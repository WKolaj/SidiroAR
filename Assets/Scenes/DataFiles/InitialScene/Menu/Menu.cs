using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private float _drawOutSpeed = 2000;
    public float DrawOutSpeed
    {
        get
        {
            return _drawOutSpeed;
        }

        set
        {
            _drawOutSpeed = value;
        }
    }

    [SerializeField]
    private float _menuWidthPercentage = 80;
    public float MenuWidthPercentage
    {
        get
        {
            return _menuWidthPercentage;
        }

        set
        {
            _menuWidthPercentage = value;
        }
    }

    private float _menuWidth;
    private float _normalizedDrawOutSpeed;

    private RectTransform _rectTrans;
    private GameObject _menuContentGO;
    private RectTransform _menuContentRectTrans;
    private GameObject _backgroundGO;

    private GameObject _userMenuItemGO;
    private GameObject _settingsMenuItemGO;
    private GameObject _languageMenuItemGO;
    private GameObject _helpMenuItemGO;
    private GameObject _exitMenuItemGO;
    private GameObject _logOutMenuItemGO;

    private MenuItem _userMenuItem;
    private MenuItem _settingsMenuItem;
    private MenuItem _languageMenuItem;
    private MenuItem _helpMenuItem;
    private MenuItem _exitMenuItem;
    private MenuItem _logOutMenuItem;

    //ITEM DEPENDS ON SIZE AFTER UI SCALING - SO METHODS MUST BE INVOKED AFTER SCALING (IN START), AWAKE IS USED TO SET UP REFERENCES
    private void Awake()
    {
        this._rectTrans = this.transform.GetComponent<RectTransform>();

        var menuListMask = this.transform.Find("MenuListMask").gameObject;
        this._menuContentGO = menuListMask.transform.Find("MenuContent").gameObject;
        this._menuContentRectTrans = _menuContentGO.GetComponent<RectTransform>();

        this._backgroundGO = menuListMask.transform.Find("Background").gameObject;

        var _menuItemContainerTopGO = _menuContentGO.transform.Find("MenuItemContainerTop").gameObject;
        var _menuItemContainerBottomGO = _menuContentGO.transform.Find("MenuItemContainerBottom").gameObject;

        _userMenuItemGO = _menuItemContainerTopGO.transform.Find("UserMenuItem").gameObject;
        _settingsMenuItemGO = _menuItemContainerTopGO.transform.Find("SettingsMenuItem").gameObject;
        _languageMenuItemGO = _menuItemContainerTopGO.transform.Find("LanguageMenuItem").gameObject;
        _helpMenuItemGO = _menuItemContainerTopGO.transform.Find("HelpMenuItem").gameObject;
        _exitMenuItemGO = _menuItemContainerBottomGO.transform.Find("ExitMenuItem").gameObject;
        _logOutMenuItemGO = _menuItemContainerBottomGO.transform.Find("LogOutMenuItem").gameObject;

        _userMenuItem = _userMenuItemGO.GetComponent<MenuItem>();
        _settingsMenuItem = _settingsMenuItemGO.GetComponent<MenuItem>();
        _languageMenuItem = _languageMenuItemGO.GetComponent<MenuItem>();
        _helpMenuItem= _helpMenuItemGO.GetComponent<MenuItem>();
        _exitMenuItem = _exitMenuItemGO.GetComponent<MenuItem>();
        _logOutMenuItem = _logOutMenuItemGO.GetComponent<MenuItem>();
    }

    //ITEM DEPENDS ON SIZE AFTER UI SCALING - SO METHODS MUST BE INVOKED AFTER SCALING (IN START), START IS USED TO SET UP UI
    private void Start()
    {
        //Calculating real menu width based on percentage
        _calculateMenuWidth();

        //Setting size and initial position of menu
        this._menuContentRectTrans.sizeDelta = new Vector2(_menuWidth, this._menuContentRectTrans.sizeDelta.y);
        this._menuContentRectTrans.anchoredPosition = new Vector3(0, 0);

        this._backgroundGO.SetActive(false);

        //Speed should be normalized according to menu width
        _normalizeDrawOutSpeed();
    }

   

    public float MenuContentOffset
    {
        get
        {
            return _menuContentRectTrans.anchoredPosition.x;
        }
    }

    public bool DrawnOut
    {
        get
        {
            return MenuContentOffset >= _menuWidth;
        }
    }

    public bool DrawnIn
    {
        get
        {
            return MenuContentOffset == 0;
        }
    }

    private bool _shouldBeDrawnOut = false;

    private void _initBackground()
    {
    }

    private void _initMenuContent()
    {

    }

    private void _normalizeDrawOutSpeed()
    {
       this._normalizedDrawOutSpeed =  this.DrawOutSpeed * _menuWidth / 860;
    }


    private void _closeButtonClickHandler(PointerEventData eventData)
    {
        this.DrawIn();
    }


    private void _calculateMenuWidth()
    {
        this._menuWidth = (MenuWidthPercentage / 100) * _rectTrans.rect.width;
    }

    public void DrawIn()
    {
        this._shouldBeDrawnOut = false;
    }

    public void DrawOut()
    {
        this._shouldBeDrawnOut = true;
    }

    private void Update()
    {
        //Draw in or out animation
        if(_shouldBeDrawnOut && !DrawnOut)
        {
            _moveMenuContent(Time.deltaTime * _normalizedDrawOutSpeed);
        }

        if(!_shouldBeDrawnOut && !DrawnIn)
        {

            _moveMenuContent(-Time.deltaTime * _normalizedDrawOutSpeed);
        }

        //Setting background
        if(_shouldBeDrawnOut && !_backgroundGO.activeSelf)
        {
            _backgroundGO.SetActive(true);
        }

        //Setting background
        if (!_shouldBeDrawnOut && _backgroundGO.activeSelf)
        {
            _backgroundGO.SetActive(false);
        }


        _userMenuItem.SetText(Translator.GetTranslation("Menu.UserMenuItemText"));
        _settingsMenuItem.SetText(Translator.GetTranslation("Menu.SettingsMenuItemText"));
        _languageMenuItem.SetText(Translator.GetTranslation("Menu.LanguageMenuItemText"));
        _helpMenuItem.SetText(Translator.GetTranslation("Menu.HelpMenuItemText"));
        _exitMenuItem.SetText(Translator.GetTranslation("Menu.ExitMenuItemText"));
        _logOutMenuItem.SetText(Translator.GetTranslation("Menu.LogOutMenuItemText"));


    }

    private void _moveMenuContent(float numberOfPixels)
    {
        var newOffset = MenuContentOffset + numberOfPixels;

        if (newOffset < 0)
        {
            newOffset = 0;
        }

        if(newOffset > _menuWidth)
        {
            newOffset = _menuWidth;
        }

        _menuContentRectTrans.anchoredPosition = new Vector3(newOffset, _menuContentRectTrans.anchoredPosition.y);
    }
    
    public void HandleHelpMenuItemClicked()
    {
        var translationWindowContentText = Translator.GetTranslation("TranslationWindow.ContentText");
        var translationWindowPolishButtonText = Translator.GetTranslation("TranslationWindow.PolishButtonText");
        var translationWindowEnglishButtonText = Translator.GetTranslation("TranslationWindow.EnglishButtonText");

        MainCanvas.ShowDialogWindow(
            translationWindowContentText, 
            translationWindowPolishButtonText, 
            () => { Translator.SetLang("pl"); this.DrawIn(); }, 
            "#41ABAB", 
            translationWindowEnglishButtonText, 
            () => { Translator.SetLang("en"); this.DrawIn(); }, 
            "#41ABAB");
    }

    public void HandleExitMenuItemClicked()
    {
        var contentText = Translator.GetTranslation("Menu.ExitWindowDialog.ContentText");
        var yesButtonText = Translator.GetTranslation("Menu.ExitWindowDialog.YesButtonText");
        var cancelButtonText = Translator.GetTranslation("Menu.ExitWindowDialog.CancelButtonText");

        MainCanvas.ShowDialogWindow(
            contentText,
            yesButtonText, 
            () => { Application.Quit(); },
            "#FF0266",
            cancelButtonText, 
            () => { this.DrawIn(); }, 
            "#41ABAB");
    }

}
