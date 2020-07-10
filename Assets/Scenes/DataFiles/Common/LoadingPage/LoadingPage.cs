using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPage : MonoBehaviour
{
    private static LoadingPage _actualLoadingPage = null;

    public static void ShowStatic()
    {
        if (_actualLoadingPage != null)
            _actualLoadingPage.Show();
    }

    public static void HideStatic()
    {
        if (_actualLoadingPage != null)
            _actualLoadingPage.Hide();
    }

    private void Awake()
    {
        LoadingPage._actualLoadingPage = this;
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
