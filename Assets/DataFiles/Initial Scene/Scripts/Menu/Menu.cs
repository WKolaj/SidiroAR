using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainCanvasGO;
    /// <summary>
    /// Main canvas Game Object - for running file browser
    /// </summary>
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
    /// <summary>
    /// Main canvas  - for running file browser
    /// </summary>
    public MainCanvas MainCanvas
    {
        get
        {
            return _mainCanvas;
        }

        set
        {
            _mainCanvas = value;
        }
    }

    private Animator slidingAnimator;

    // Start is called before the first frame update
    void Start()
    {
        InitAnimator();
        InitMainCanvas();
    }


    /// <summary>
    /// Method for initializing animator
    /// </summary>
    private void InitAnimator()
    {
        this.slidingAnimator = GetComponentInChildren<Animator>();
    }

    private void InitMainCanvas()
    {
        MainCanvas = MainCanvasGO.GetComponent<MainCanvas>();
    }

    /// <summary>
    /// Causes slider to slide out
    /// </summary>
    public void SlideOut()
    {
        this.slidingAnimator.SetBool("SlidingIn", false);
    }

    /// <summary>
    /// Causes slider to slide in
    /// </summary>
    public void SlideIn()
    {
        this.slidingAnimator.SetBool("SlidingIn", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method for loading file
    /// </summary>
    public void LoadFromFile()
    {
        var explorer = MainCanvas.OpenModelExplorer();
    }

    /// <summary>
    /// Method for handling closing of file browser when file is selected
    /// </summary>
    /// <param name="filePath">
    /// Path to selected file
    /// </param>
    private void HandleFileBrowserFileSelected(string filePath)
    {
        Debug.Log(filePath);
    }

}
