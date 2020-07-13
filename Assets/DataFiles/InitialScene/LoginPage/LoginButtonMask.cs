using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginButtonMask : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Button.ButtonClickedEvent _onClick = null;
    public Button.ButtonClickedEvent OnClick
    {
        get
        {
            return _onClick;
        }

        set
        {
            _onClick = value;
        }
    }

    [SerializeField]
    private bool _disabled = false;
    public bool Disabled
    {
        get
        {
            return _disabled;
        }

        set
        {
            _disabled = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //If disabled - return imidiatelly
        if (Disabled) return;

        if (OnClick != null) OnClick.Invoke();
    }
}
