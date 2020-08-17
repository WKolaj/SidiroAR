using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrecisePositioningComponent : MonoBehaviour
{
    private bool _shouldBeDrawnOut = false;
    public bool ShouldBeDrawnOut
    {
        get
        {
            return _shouldBeDrawnOut;
        }
    }

    [SerializeField]
    private float _drawOutSpeed = 3000;
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

    [SerializeField]
    private float _componentYPosition = 750;
    public float ComponentYPosition
    {
        get
        {
            return _componentYPosition;
        }

        set
        {
            _componentYPosition = value;
        }
    }

    public float ComponentOffset
    {
        get
        {
            return _componentRectTrans.anchoredPosition.y;
        }
    }

    private RectTransform _componentRectTrans = null;

    private GameObject _moveUpButtonGO = null;
    private GameObject _moveDownButtonGO = null;
    private GameObject _moveForwardButtonGO = null;
    private GameObject _moveBackButtonGO = null;
    private GameObject _moveLeftButtonGO = null;
    private GameObject _moveRightButtonGO = null;
    private GameObject _rotateLeftButtonGO = null;
    private GameObject _rotateRightButtonGO = null;

    private PrecizePositioningButton _moveUpButton = null;
    private PrecizePositioningButton _moveDownButton = null;
    private PrecizePositioningButton _moveForwardButton = null;
    private PrecizePositioningButton _moveBackButton = null;
    private PrecizePositioningButton _moveLeftButton = null;
    private PrecizePositioningButton _moveRightButton = null;
    private PrecizePositioningButton _rotateLeftButton = null;
    private PrecizePositioningButton _rotateRightButton = null;

    // Start is called before the first frame update
    void Start()
    {
        _moveUpButton.OnClick = MoveUpButtonClicked;
        _moveDownButton.OnClick = MoveDownButtonClicked;
        _moveBackButton.OnClick = MoveBackButtonClicked;
        _moveForwardButton.OnClick = MoveForwardButtonClicked;
        _moveRightButton.OnClick = MoveRightButtonClicked;
        _moveLeftButton.OnClick = MoveLeftButtonClicked;
        _rotateLeftButton.OnClick = RotateLeftButtonClicked;
        _rotateRightButton.OnClick = RotateRightButtonClicked;
    }


    private void Awake()
    {
        _componentRectTrans = transform.GetComponent<RectTransform>();

        _moveUpButtonGO = transform.Find("MoveUpButton").gameObject;
        _moveDownButtonGO = transform.Find("MoveDownButton").gameObject;
        _moveBackButtonGO = transform.Find("MoveBackButton").gameObject;
        _moveForwardButtonGO = transform.Find("MoveForwardButton").gameObject;
        _moveRightButtonGO = transform.Find("MoveRightButton").gameObject;
        _moveLeftButtonGO = transform.Find("MoveLeftButton").gameObject;
        _rotateLeftButtonGO = transform.Find("RotateLeftButton").gameObject;
        _rotateRightButtonGO = transform.Find("RotateRightButton").gameObject;

        _moveUpButton = _moveUpButtonGO.GetComponent<PrecizePositioningButton>();
        _moveDownButton = _moveDownButtonGO.GetComponent<PrecizePositioningButton>();
        _moveBackButton = _moveBackButtonGO.GetComponent<PrecizePositioningButton>();
        _moveForwardButton = _moveForwardButtonGO.GetComponent<PrecizePositioningButton>();
        _moveRightButton = _moveRightButtonGO.GetComponent<PrecizePositioningButton>();
        _moveLeftButton = _moveLeftButtonGO.GetComponent<PrecizePositioningButton>();
        _rotateLeftButton = _rotateLeftButtonGO.GetComponent<PrecizePositioningButton>();
        _rotateRightButton = _rotateRightButtonGO.GetComponent<PrecizePositioningButton>();

    }

    [SerializeField]
    private Button.ButtonClickedEvent _moveUpButtonClicked = null;
    public Button.ButtonClickedEvent MoveUpButtonClicked
    {
        get
        {
            return _moveUpButtonClicked;
        }

        set
        {
            _moveUpButtonClicked = value;
        }
    }

    [SerializeField]
    private Button.ButtonClickedEvent _moveDownButtonClicked = null;
    public Button.ButtonClickedEvent MoveDownButtonClicked
    {
        get
        {
            return _moveDownButtonClicked;
        }

        set
        {
            _moveDownButtonClicked = value;
        }
    }

    [SerializeField]
    private Button.ButtonClickedEvent _moveRightButtonClicked = null;
    public Button.ButtonClickedEvent MoveRightButtonClicked
    {
        get
        {
            return _moveRightButtonClicked;
        }

        set
        {
            _moveRightButtonClicked = value;
        }
    }

    [SerializeField]
    private Button.ButtonClickedEvent _moveLeftButtonClicked = null;
    public Button.ButtonClickedEvent MoveLeftButtonClicked
    {
        get
        {
            return _moveLeftButtonClicked;
        }

        set
        {
            _moveLeftButtonClicked = value;
        }
    }

    [SerializeField]
    private Button.ButtonClickedEvent _moveBackButtonClicked = null;
    public Button.ButtonClickedEvent MoveBackButtonClicked
    {
        get
        {
            return _moveBackButtonClicked;
        }

        set
        {
            _moveBackButtonClicked = value;
        }
    }

    [SerializeField]
    private Button.ButtonClickedEvent _moveForwardButtonClicked = null;
    public Button.ButtonClickedEvent MoveForwardButtonClicked
    {
        get
        {
            return _moveForwardButtonClicked;
        }

        set
        {
            _moveForwardButtonClicked = value;
        }
    }

    [SerializeField]
    private Button.ButtonClickedEvent _rotateLeftButtonClicked = null;
    public Button.ButtonClickedEvent RotateLeftButtonClicked
    {
        get
        {
            return _rotateLeftButtonClicked;
        }

        set
        {
            _rotateLeftButtonClicked = value;
        }
    }

    [SerializeField]
    private Button.ButtonClickedEvent _rotateRightButtonClicked = null;
    public Button.ButtonClickedEvent RotateRightButtonClicked
    {
        get
        {
            return _rotateRightButtonClicked;
        }

        set
        {
            _rotateRightButtonClicked = value;
        }
    }

    public bool DrawnOut
    {
        get
        {
            return ComponentOffset >= ComponentYPosition;
        }
    }

    public bool DrawnIn
    {
        get
        {
            return ComponentOffset == 0;
        }
    }

    public void DrawOut()
    {
        this._shouldBeDrawnOut = true;
    }

    public void DrawIn()
    {
        this._shouldBeDrawnOut = false;
    }

    private void Update()
    {
        //Draw in or out animation
        if (_shouldBeDrawnOut && !DrawnOut)
        {
            _moveComponent(Time.deltaTime * DrawOutSpeed);
        }

        if (!_shouldBeDrawnOut && !DrawnIn)
        {

            _moveComponent(-Time.deltaTime * DrawOutSpeed);
        }
    }

    private void _moveComponent(float numberOfPixels)
    {
        var newOffset = ComponentOffset + numberOfPixels;

        if (newOffset < 0)
        {
            newOffset = 0;
        }

        if (newOffset > ComponentYPosition)
        {
            newOffset = ComponentYPosition;
        }

        _componentRectTrans.anchoredPosition = new Vector3(_componentRectTrans.anchoredPosition.x, newOffset);

    }
}
