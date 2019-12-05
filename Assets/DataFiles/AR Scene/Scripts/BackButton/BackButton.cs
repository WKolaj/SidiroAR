using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : ClickableButton
{
    protected override void onClicked()
    {
        base.onClicked();

        //Go back to previous scene
        Common.LoadInitialScene();
    }
}
