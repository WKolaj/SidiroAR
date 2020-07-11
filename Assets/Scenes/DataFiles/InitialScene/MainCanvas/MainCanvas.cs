using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    private static MainCanvas _actualMainCanvas = null;

    public static void ShowMenu()
    {
        _actualMainCanvas._menu.DrawOut();
    }

    public static void HideMenu()
    {
        _actualMainCanvas._menu.DrawIn();
    }

    public static void ShowLoadingPage()
    {
        _actualMainCanvas._loadingPage.Show();
    }

    public static void HideLoadingPage()
    {
        _actualMainCanvas._loadingPage.Hide();
    }

    public static void ShowAuxPage(GameObject pageContent)
    {
        _actualMainCanvas._auxPageContainer.DrawOut(pageContent);
    }

    public static void HideAuxPage()
    {
        _actualMainCanvas._auxPageContainer.DrawIn();
    }

    public static void ShowDialogWindow(string windowText = "", string button0Text = null, Action button0Method = null, string button0Color = "#FFFF0266", string button1Text = null, Action button1Method = null, string button1Color = "#FF3EACAB")
    {
        _actualMainCanvas._dialogWindow.Show(windowText, button0Text, button0Method, button0Color, button1Text, button1Method, button1Color);
    }

    public static void HideDialogWindow()
    {
        _actualMainCanvas._dialogWindow.Hide();
    }



    private Menu _menu = null;
    private LoadingPage _loadingPage = null;
    private AuxPageContainer _auxPageContainer = null;
    private DialogWindow _dialogWindow = null;

    private void Awake()
    {
        MainCanvas._actualMainCanvas = this;

        this._menu = this.GetComponentInChildren<Menu>(true);
        this._loadingPage = this.GetComponentInChildren<LoadingPage>(true);
        this._auxPageContainer = this.GetComponentInChildren<AuxPageContainer>(true);
        this._dialogWindow = this.GetComponentInChildren<DialogWindow>(true);

    }


    
    public void ShowDialog1()
    {
        MainCanvas.ShowDialogWindow("To jest jakaś wiadomość", "OK", () => Debug.Log("OK Clicked"), "#eb4334");
    }

}
