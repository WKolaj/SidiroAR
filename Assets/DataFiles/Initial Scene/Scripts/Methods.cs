using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Methods : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainCanvasGO;
    public GameObject MainCanvasGO
    {
        get
        {
            return _mainCanvasGO;
        }

        set
        {
            _mainCanvasGO = value;
        }
    }

    private MainCanvas _mainCanvas;
    public MainCanvas MainCanvas
    {
        get
        {
            return _mainCanvas;
        }

    }

    void Awake()
    {
        InitMainCanvas();
    }

    private void InitMainCanvas()
    {
        _mainCanvas = MainCanvasGO.GetComponent<MainCanvas>();
    }


    public void LoadARScene()
    {
        Common.LoadARScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Trying to quit when back button clicked
            TryQuitApp();
        }
    }

    public void TryQuitApp()
    {
        var dialogBox = MainCanvas.ShowDialogBox("Wyjście", "Czy na pewno chcesz opuścić program?", DialogBoxMode.Question, DialogBoxType.YesNo);

        dialogBox.onYesClicked += new System.Action(() =>
        {
            Common.QuitApp();
        });
    }

}
