using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ARCanvas : MonoBehaviour
{

    /// <summary>
    /// Method for showing dialog window
    /// </summary>
    /// <param name="windowText">
    /// Text embeded in window
    /// </param>
    /// <param name="button0Text">
    /// first button text
    /// </param>
    /// <param name="button0Method">
    /// method connected to first button
    /// </param>
    /// <param name="button0Color">
    /// color of first button
    /// </param>
    /// <param name="button1Text">
    /// second button text
    /// </param>
    /// <param name="button1Method">
    /// method connected to second button
    /// </param>
    /// <param name="button1Color">
    /// color of second button
    /// </param>
    public static void ShowDialogWindow(string windowText = "", string button0Text = null, Action button0Method = null, string button0Color = "#FFFF0266", string button1Text = null, Action button1Method = null, string button1Color = "#FF3EACAB")
    {
        _actualARCanvas._dialogWindow.Show(windowText, button0Text, button0Method, button0Color, button1Text, button1Method, button1Color);
    }

    /// <summary>
    /// AR, actual canvas
    /// </summary>
    private static ARCanvas _actualARCanvas = null;

    /// <summary>
    /// Reference to dialog window
    /// </summary>
    private DialogWindow _dialogWindow = null;

    /// <summary>
    /// Waiting for surface label
    /// </summary>
    private TextMeshProUGUI _waitingForSurfaceLabel = null;

    /// <summary>
    /// Go back button
    /// </summary>
    private ClickableImage _goBackButton = null;

    /// <summary>
    /// Pick up model button
    /// </summary>
    private ClickableImage _pickUpModelButton = null;

    /// <summary>
    /// Hide doors button
    /// </summary>
    private ClickableImage _hideDoorsButton = null;

    /// <summary>
    /// Show doors button
    /// </summary>
    private ClickableImage _showDoorsButton = null;

    /// <summary>
    /// Hide covers button
    /// </summary>
    private ClickableImage _hideCoversButton = null;

    /// <summary>
    /// Show covers button
    /// </summary>
    private ClickableImage _showCoversButton = null;

    /// <summary>
    /// Reference to circle controller
    /// </summary>
    private CircleController _circleController = null;

    [SerializeField]
    private GameObject _placementControllerGO;
    public GameObject PlacementControllerGO
    {
        get
        {
            return _placementControllerGO;
        }

        set
        {
            _placementControllerGO = value;
        }
    }

    private PlacementController _placementController;

    /// <summary>
    /// Method called on application start
    /// </summary>
    private void Awake()
    {
        ARCanvas._actualARCanvas = this;

        this._placementController = PlacementControllerGO.GetComponent<PlacementController>();

        this._dialogWindow = this.GetComponentInChildren<DialogWindow>(true);
        this._circleController = this.GetComponentInChildren<CircleController>(true);

        var waitingForSurfaceLabelGO = this.transform.Find("WaitingForSurfaceLabel").gameObject;

        var backButtonGO = this.transform.Find("BackButton").gameObject;
        var pickUpButtonGO = this.transform.Find("PickUpModelButton").gameObject;
        var showDoorsButtonGO = this.transform.Find("ShowDoorsButton").gameObject;
        var hideDoorButtonGO = this.transform.Find("HideDoorsButton").gameObject;
        var showCoversButtonGO = this.transform.Find("ShowCoversButton").gameObject;
        var hideCoversButtonGO = this.transform.Find("HideCoversButton").gameObject;

        this._waitingForSurfaceLabel = waitingForSurfaceLabelGO.GetComponent<TextMeshProUGUI>();

        this._goBackButton = backButtonGO.GetComponent<ClickableImage>();
        this._pickUpModelButton = pickUpButtonGO.GetComponent<ClickableImage>();
        this._showDoorsButton = showDoorsButtonGO.GetComponent<ClickableImage>();
        this._hideDoorsButton = hideDoorButtonGO.GetComponent<ClickableImage>();
        this._showCoversButton = showCoversButtonGO.GetComponent<ClickableImage>();
        this._hideCoversButton = hideCoversButtonGO.GetComponent<ClickableImage>();

        this._waitingForSurfaceLabel.text = Translator.GetTranslation("WaitingForSurfaceLabel.ContentText");

        //Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Trying to update when back button clicked
            _showBackFromARSceneDialogWindow();
        }

        //Refreshing UI
        _refreshUIBasedOnPlacementControllerState();
    }

    private void _refreshUIBasedOnPlacementControllerState()
    {
        if(_placementController.PlacementControllerState == PlacementControllerState.waitingForSurface)
        {
            this._waitingForSurfaceLabel.gameObject.SetActive(true);
            this._pickUpModelButton.gameObject.SetActive(false);
            this._circleController.gameObject.SetActive(false);
            this._showDoorsButton.gameObject.SetActive(false);
            this._hideDoorsButton.gameObject.SetActive(false);
            this._showCoversButton.gameObject.SetActive(false);
            this._hideCoversButton.gameObject.SetActive(false);
        }
        else if (_placementController.PlacementControllerState == PlacementControllerState.indicatorPresented)
        {
            this._waitingForSurfaceLabel.gameObject.SetActive(false);
            this._pickUpModelButton.gameObject.SetActive(false);
            this._circleController.gameObject.SetActive(true);
            this._showDoorsButton.gameObject.SetActive(false);
            this._hideDoorsButton.gameObject.SetActive(false);
            this._showCoversButton.gameObject.SetActive(false);
            this._hideCoversButton.gameObject.SetActive(false);
        }
        else if (_placementController.PlacementControllerState == PlacementControllerState.modelPresented)
        {
            this._waitingForSurfaceLabel.gameObject.SetActive(false);
            this._pickUpModelButton.gameObject.SetActive(true);
            this._circleController.gameObject.SetActive(false);

            if(_placementController.DoorsShown)
            {
                this._showDoorsButton.gameObject.SetActive(false);
                this._hideDoorsButton.gameObject.SetActive(true);
            }
            else
            {
                this._showDoorsButton.gameObject.SetActive(true);
                this._hideDoorsButton.gameObject.SetActive(false);
            }

            if (_placementController.CoversShown)
            {
                this._showCoversButton.gameObject.SetActive(false);
                this._hideCoversButton.gameObject.SetActive(true);
            }
            else
            {
                this._showCoversButton.gameObject.SetActive(true);
                this._hideCoversButton.gameObject.SetActive(false);
            }
        }

    }

    /// <summary>
    /// Method invoked after back button was clicked
    /// </summary>
    public void HandleBackButtonClicked()
    {
        _showBackFromARSceneDialogWindow();
    }

    /// <summary>
    /// Method invoked after pick up model button was clicked
    /// </summary>
    public void HandlePickUpModelButtonClicked()
    {
        _placementController.PickModel();

        //Showing doors in order to present them after placing the model once again
        _placementController.ShowDoorComponents();
    }

    /// <summary>
    /// Method invoked after show doors button was clicked
    /// </summary>
    public void HandleShowDoorsButtonClicked()
    {
        _placementController.ShowDoorComponents();
    }

    /// <summary>
    /// Method invoked after hide doors button was clicked
    /// </summary>
    public void HandleHideDoorsButtonClicked()
    {
        _placementController.HideDoorComponents();
    }

    /// <summary>
    /// Method invoked after show covers button was clicked
    /// </summary>
    public void HandleShowCoversButtonClicked()
    {
        _placementController.ShowCoverComponents();
    }

    /// <summary>
    /// Method invoked after hide covers button was clicked
    /// </summary>
    public void HandleHideCoversButtonClicked()
    {
        _placementController.HideCoverComponents();
    }

    /// <summary>
    /// Method invoked after circle controller middle button is clicked
    /// </summary>
    public void HandleCircleControllerMiddleButtonClicked()
    {
        _placementController.PlaceModel();
    }

    /// <summary>
    /// Method invoked when circle controller value (angle) changes
    /// </summary>
    /// <param name="newValue">
    /// new value
    /// </param>
    public void HandleCircleControllerValueChange(float newValue)
    {
        _placementController.AssingContainerAngle(newValue);
        _circleController.ChangeValue(newValue);
    }

    /// <summary>
    /// Method used for displaying dialog window to go back to previous scene
    /// </summary>
    private void _showBackFromARSceneDialogWindow()
    {
        _actualARCanvas._dialogWindow.Show(
           Translator.GetTranslation("BackFromARSceneDialogWindow.ContentText"),
           Translator.GetTranslation("BackFromARSceneDialogWindow.YesButtonText"),
           () => Common.LoadInitialScene(),
           "#FF0266",
           Translator.GetTranslation("BackFromARSceneDialogWindow.NoButtonText"),
           () => { },
           "#3EACAB");
    }

}
