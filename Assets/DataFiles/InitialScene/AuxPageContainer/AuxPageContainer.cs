using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxPageContainer : MonoBehaviour
{
    
    [SerializeField]
    private float _drawOutSpeed = 2000;
    public float DrawOutSpeed
    {
        get
        {
            return _drawOutSpeed;
        }

        set
        {
            _drawOutSpeed = value;
        }
    }

    //Used for checking if content in show changes
    private GameObject _actualContent = null;

    public float PageContainerOffset
    {
        get
        {
            return _pageContainerRectTrans.anchoredPosition.y;
        }
    }

    public bool DrawnOut
    {
        get
        {
            return PageContainerOffset >= _actualHeight;
        }
    }

    public bool DrawnIn
    {
        get
        {
            return PageContainerOffset == 0;
        }
    }

    private bool _shouldBeDrawnOut = false;
    private float _normalizedDrawOutSpeed;

    private float _actualHeight;

    private RectTransform _rectTrans;
    private GameObject _pageContainerGO;
    private RectTransform _pageContainerRectTrans;

    //ITEM DEPENDS ON SIZE AFTER UI SCALING - SO METHODS MUST BE INVOKED AFTER SCALING (IN START), AWAKE IS USED TO SET UP REFERENCES
    private void Awake()
    {
        this._rectTrans = this.GetComponent<RectTransform>();

        var maskGO = this.transform.Find("AuxPageContainerMask").gameObject;

        this._pageContainerGO = maskGO.transform.Find("PageContainer").gameObject;
        this._pageContainerRectTrans = _pageContainerGO.GetComponent<RectTransform>();


    }

    //ITEM DEPENDS ON SIZE AFTER UI SCALING - SO METHODS MUST BE INVOKED AFTER SCALING (IN START), START IS USED TO SET UP UI
    private void Start()
    {
        this._actualHeight = this._rectTrans.rect.height;

        //Setting size and initial position of page container
        this._pageContainerRectTrans.sizeDelta = new Vector2(this._pageContainerRectTrans.sizeDelta.x, _actualHeight);
        this._pageContainerRectTrans.anchoredPosition = new Vector3(0, 0);

        _normalizeDrawOutSpeed();
    }

    private void Update()
    {
        //Draw in or out animation
        if (_shouldBeDrawnOut && !DrawnOut)
        {
            _movePageContainer(Time.deltaTime * _normalizedDrawOutSpeed);
        }

        if (!_shouldBeDrawnOut && !DrawnIn)
        {

            _movePageContainer(-Time.deltaTime * _normalizedDrawOutSpeed);
        }

    }

    private void _normalizeDrawOutSpeed()
    {
        this._normalizedDrawOutSpeed = this.DrawOutSpeed * _actualHeight / 2240;
    }


    public void DrawIn()
    {
        this._shouldBeDrawnOut = false;
    }

    public void DrawOut(GameObject content)
    {
        //Setting content to be presented
        this.SetContent(content);
        
        this._shouldBeDrawnOut = true;

    }

    /// <summary>
    /// Method for drawing out aux page container - user for logging window to be presented on the begining
    /// </summary>
    /// <param name="content"></param>
    public void DrawOutInstantly(GameObject content)
    {
        DrawOut(content);

        _movePageContainer(_actualHeight);
    }

    public void SetContent(GameObject content)
    {
        //Changing content only if it changes
        if(_actualContent != content)
        {
            this._actualContent = content;

            //Always clearing - in order to clear if null was provided
            _clearPageContainer();

            if (content != null)
            {
                Instantiate(content, this._pageContainerGO.transform);
            }
        }
    }

    private void _clearPageContainer()
    {
        foreach (Transform child in _pageContainerGO.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void _movePageContainer(float numberOfPixels)
    {
        var newOffset = PageContainerOffset + numberOfPixels;

        if (newOffset < 0)
        {
            newOffset = 0;
        }

        if (newOffset > _actualHeight)
        {
            newOffset = _actualHeight;
        }

        _pageContainerRectTrans.anchoredPosition = new Vector3(_pageContainerRectTrans.anchoredPosition.x, newOffset);
    }
}
