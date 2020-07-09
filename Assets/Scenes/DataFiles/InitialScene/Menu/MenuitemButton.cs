using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuitemButton : MonoBehaviour, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null) OnClick.Invoke();
    }

}
