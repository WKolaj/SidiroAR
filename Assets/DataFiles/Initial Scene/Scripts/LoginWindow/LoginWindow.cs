using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LoginWindow : MonoBehaviour
{

    private MainCanvas mainCanvas;
    private UserLoader userLoader;
    private GameObject userIDInputGO;
    private TMP_InputField userIDInput;
    private GameObject userPasswordIDInputGO;
    private TMP_InputField userPasswordIDInput;

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

        this.userIDInputGO = transform.Find("UserIDInput").gameObject;
        var userPasswordElement = transform.Find("UserPasswordElement").gameObject;
        this.userPasswordIDInputGO = userPasswordElement.transform.Find("UserPasswordInput").gameObject;

        this.userIDInput = userIDInputGO.GetComponent<TMP_InputField>();
        this.userPasswordIDInput = userPasswordIDInputGO.GetComponent<TMP_InputField>();

    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void AppendPIN(string characterToAdd)
    {
        this.userPasswordIDInput.text += characterToAdd;
    }

    public void CutPIN()
    {
        if(!string.IsNullOrEmpty(this.userPasswordIDInput.text))
        {
            this.userPasswordIDInput.text = this.userPasswordIDInput.text.Remove(this.userPasswordIDInput.text.Length-1);
        }
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
        }
        catch(Exception err)
        {
            mainCanvas.HideProgressWindow();
            mainCanvas.ShowDialogBox("Błąd logowania", err.Message, DialogBoxMode.Warning, DialogBoxType.Ok);
        }
    }

    public void TryLogIn()
    {
        AsyncTryLogIn();
    }

    public void HandleOfflineButtonClicked()
    {
        Hide();
    }
}
