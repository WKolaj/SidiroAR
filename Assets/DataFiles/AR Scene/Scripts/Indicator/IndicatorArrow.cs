using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorArrow : MonoBehaviour
{

    public void AdjustSize(Vector3 newSize)
    {
        var yScale = newSize.z / gameObject.transform.localScale.y;
        var xScale = newSize.x / gameObject.transform.localScale.x;

        //Rescale indicator to the size of model
        gameObject.transform.localScale = new Vector3(0.125f*yScale, 0.125f * yScale, 1);
    }
}
