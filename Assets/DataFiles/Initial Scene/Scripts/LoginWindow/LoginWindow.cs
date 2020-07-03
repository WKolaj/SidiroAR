using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginWindow : MonoBehaviour
{

    private MainCanvas mainCanvas;
    private UserLoader userLoader;
    private GameObject inputArea;
    private GameObject userIDInputGO;
    private TMP_InputField userIDInput;
    private GameObject userPasswordIDInputGO;
    private TMP_InputField userPasswordIDInput;
    private GameObject loginButtonGO;
    private Button loginButton;
    private GameObject offlineButtonGO;
    private Button offlineButton;

    public void Awake()
    {
    }

    /// <summary>
    /// Method for initializing login window
    /// </summary>
    /// <param name="canvas">
    /// Main canvas
    /// </param>
    /// <param name="loader">
    /// User loader
    /// </param>
    public void Init(MainCanvas canvas, UserLoader loader)
    {
        InitComponents(canvas,loader);
    }


    private void InitComponents(MainCanvas canvas, UserLoader loader)
    {
        this.mainCanvas = canvas;
        this.userLoader = loader;

        this.inputArea = transform.Find("InputArea").gameObject;
        this.userIDInputGO = this.inputArea.transform.Find("UserIDInput").gameObject;
        this.userPasswordIDInputGO = this.inputArea.transform.Find("UserPasswordInput").gameObject;

        this.userIDInput = userIDInputGO.GetComponent<TMP_InputField>();
        this.userPasswordIDInput = userPasswordIDInputGO.GetComponent<TMP_InputField>();

        this.loginButtonGO = this.inputArea.transform.Find("LoginButton").gameObject;
        this.loginButton = this.loginButtonGO.GetComponent<Button>();

        this.offlineButtonGO = this.inputArea.transform.Find("OfflineButton").gameObject;
        this.offlineButton = this.loginButtonGO.GetComponent<Button>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method for asynchronusly try to log in
    /// </summary>
    /// <returns>
    /// </returns>
    private async Task AsyncTryLogIn()
    {
        try
        {
            mainCanvas.ShowProgressWindow();
            await userLoader.LoginUserFromServer(this.userIDInput.text, this.userPasswordIDInput.text);
            mainCanvas.HideProgressWindow();
            Hide();
            mainCanvas.RefreshUserDisplay(UserLoader.LoggedUser);
        }
        catch(Exception err)
        {
            mainCanvas.HideProgressWindow();
            mainCanvas.ShowDialogBox("Błąd logowania", err.Message, DialogBoxMode.Warning, DialogBoxType.Ok);
        }
    }

    public async void HandleLoginButtonClicked()
    {
        await AsyncTryLogIn();
        this.userPasswordIDInput.text = "";
    }


    public void HandleOfflineButtonClicked()
    {
        this.userIDInput.text = "";
        this.userPasswordIDInput.text = "";
        Hide();
        mainCanvas.RefreshUserDisplay(UserLoader.LoggedUser);
    }

    public void Update()
    {
        //Checking if login button should be enabled
        this.loginButton.interactable = (!String.IsNullOrEmpty(userIDInput.text) && !String.IsNullOrEmpty(userPasswordIDInput.text) && userPasswordIDInput.text.Length >= 8);
    }

}
