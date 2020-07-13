using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginPage : MonoBehaviour
{
    private GameObject _loginInputGO;
    private GameObject _passwordInputGO;
    private GameObject _loginButtonGO;
    private GameObject _languageButtonGO;
    private GameObject _loginInputPlaceholderGO;
    private GameObject _passwordInputPlaceholderGO;
    private GameObject _mainLabelGO;

    private TMP_InputField _loginInput;
    private TMP_InputField _passwordInput;
    private LoginButton _loginButton;
    private ClickableImage _languageButton;
    private TextMeshProUGUI _loginInputPlaceholder;
    private TextMeshProUGUI _passwordInputPlaceholder;
    private TextMeshProUGUI _mainLabel;


    private void Awake()
    {
        this._languageButtonGO = transform.Find("LanguageButton").gameObject;

        this._languageButton = _languageButtonGO.GetComponent<ClickableImage>();
        
        var loginComponent = transform.Find("LoginComponent").gameObject;

        this._loginInputGO = loginComponent.transform.Find("LoginInput").gameObject;
        this._passwordInputGO = loginComponent.transform.Find("PasswordInput").gameObject;
        this._loginButtonGO = loginComponent.transform.Find("LoginButton").gameObject;
        this._mainLabelGO = loginComponent.transform.Find("MainLabel").gameObject;

        var loginInputTextArea = _loginInputGO.transform.Find("Text Area").gameObject;
        _loginInputPlaceholderGO = loginInputTextArea.transform.Find("Placeholder").gameObject;

        var passwordInputTextArea = _passwordInputGO.transform.Find("Text Area").gameObject;
        _passwordInputPlaceholderGO = passwordInputTextArea.transform.Find("Placeholder").gameObject;

        this._loginInput = _loginInputGO.GetComponent<TMP_InputField>();
        this._passwordInput = _passwordInputGO.GetComponent<TMP_InputField>();
        this._loginButton = _loginButtonGO.GetComponent<LoginButton>();
        this._mainLabel = _mainLabelGO.GetComponent<TextMeshProUGUI>();

        this._loginInputPlaceholder = _loginInputPlaceholderGO.GetComponent<TextMeshProUGUI>();
        this._passwordInputPlaceholder = _passwordInputPlaceholderGO.GetComponent<TextMeshProUGUI>();

        this._loginButton.OnClick.AddListener(_handleLoginButtonClicked);

        this._languageButton.OnClick.AddListener(_handleLanguageButtonClicked);
    }

    private void _handleLanguageButtonClicked()
    {
        MainCanvas.ShowLanguageWindow();
    }

    private void _handleLoginButtonClicked()
    {
        Debug.Log("Login");
    }

    private bool _validateLoginAndPassword()
    {
        if (string.IsNullOrEmpty(this._loginInput.text) || string.IsNullOrEmpty(this._passwordInput.text)) return false;

        if (this._passwordInput.text.Length < 8) return false;

        return true;
    }

    private void Update()
    {
        //Controling log in button disable
        if (_validateLoginAndPassword())
            _loginButton.Enable();
        else
            _loginButton.Disable();

        //Setting translation
        _loginInputPlaceholder.text = Translator.GetTranslation("LoginPage.LoginInputPlaceholder");
        _passwordInputPlaceholder.text = Translator.GetTranslation("LoginPage.PasswordInputPlaceholder");
        _loginButton.SetText(Translator.GetTranslation("LoginPage.LoginButtonText"));
        _mainLabel.text = Translator.GetTranslation("LoginPage.MainLabelText");
    }

}
