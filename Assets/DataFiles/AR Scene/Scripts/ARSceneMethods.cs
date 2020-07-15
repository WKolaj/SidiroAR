using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSceneMethods : MonoBehaviour
{
    [SerializeField]
    private GameObject _arCanvasGO;
    public GameObject ARCanvasGO
    {
        get
        {
            return _arCanvasGO;
        }

        set
        {
            _arCanvasGO = value;
        }
    }

    private ARCanvas _arCanvas;
    public ARCanvas ARCanvas
    {
        get
        {
            return _arCanvas;
        }

    }

    private void Awake()
    {
        InitARCanvas();
    }

    private void InitARCanvas()
    {
        _arCanvas = ARCanvasGO.GetComponent<ARCanvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Trying to update when back button clicked
            TryGoBackToInitialScene();
        }
    }


    public void TryGoBackToInitialScene()
    {
        var dialogBox = ARCanvas.ShowDialogBox("Powrót","Czy na pewno chcesz wrócić do wyboru rozdzielnicy",DialogBoxMode.Question,DialogBoxType.YesNo);

        dialogBox.onYesClicked += new System.Action(() =>
        {
            Common.LoadInitialScene();
        });
    }
}
