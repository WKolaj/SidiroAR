using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPage : MonoBehaviour
{
    private void Start()
    {
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
