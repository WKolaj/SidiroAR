using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private GameObject topLine;
    private GameObject bottomLine;
    private GameObject rightLine;
    private GameObject leftLine;
    private GameObject arrow;
    private GameObject arrowLeftTopLine;
    private GameObject arrowRightTopLine;
    private GameObject arrowLeftMiddleHorizontalLine;
    private GameObject arrowLeftMiddleVerticalLine;
    private GameObject arrowRightMiddleHorizontalLine;
    private GameObject arrowRightMiddleVerticalLine;
    private GameObject arrowBottomLine;

    [SerializeField]
    private Material _borderMaterial;
    public Material BorderMaterial
    {
        get
        {
            return _borderMaterial;
        }

        set
        {
            _borderMaterial = value;
        }
    }

    [SerializeField]
    private float _borderWidth;
    public float BorderWidth
    {
        get
        {
            return _borderWidth;
        }

        set
        {
            _borderWidth = value;
        }
    }

    [SerializeField]
    private float _arrowSizePercentage = 0.6f;
    public float ArrowSizePercentage
    {
        get
        {
            return _arrowSizePercentage;
        }

        set
        {
            _arrowSizePercentage = value;
        }
    }

    public void Init(Vector3 size, Vector3 position)
    {

        //Initializing border
        _initBorder(size);

        //Initializing arrow
        _initArrow(size);

        //Getting setting position of element
        this.transform.localPosition = new Vector3(-size.x / 2, 0, -size.z / 2);

    }

    private void _initBorder(Vector3 size)
    {

        //Getting border elements from project
        this.topLine = transform.Find("TopLine").gameObject;
        this.bottomLine = transform.Find("BottomLine").gameObject;
        this.rightLine = transform.Find("RightLine").gameObject;
        this.leftLine = transform.Find("LeftLine").gameObject;


        //Initialize material of border
        var topLineMeshRenderer = topLine.GetComponent<MeshRenderer>();
        var bottomLineMeshRenderer = bottomLine.GetComponent<MeshRenderer>();
        var rightLineMeshRenderer = rightLine.GetComponent<MeshRenderer>();
        var leftLineMeshRenderer = leftLine.GetComponent<MeshRenderer>();
        topLineMeshRenderer.material = BorderMaterial;
        bottomLineMeshRenderer.material = BorderMaterial;
        rightLineMeshRenderer.material = BorderMaterial;
        leftLineMeshRenderer.material = BorderMaterial;

        //Initializing size of border lines
        topLine.transform.localScale = new Vector3(size.x + BorderWidth, BorderWidth, 1);
        bottomLine.transform.localScale = new Vector3(size.x + BorderWidth, BorderWidth, 1);
        rightLine.transform.localScale = new Vector3(BorderWidth, size.z + BorderWidth, 1);
        leftLine.transform.localScale = new Vector3(BorderWidth, size.z + BorderWidth, 1);

        //Initializing position of border lines
        topLine.transform.localPosition = new Vector3((size.x / 2) + (BorderWidth / 2), 0, size.z + (BorderWidth / 2));
        bottomLine.transform.localPosition = new Vector3((size.x / 2) + (BorderWidth / 2), 0, (BorderWidth / 2));
        rightLine.transform.localPosition = new Vector3(size.x + (BorderWidth / 2), 0, (size.z / 2) + (BorderWidth / 2));
        leftLine.transform.localPosition = new Vector3((BorderWidth / 2), 0, (size.z / 2) + (BorderWidth / 2));
    }

    private void _initArrow(Vector3 size)
    {
        //Getting arrow element
        this.arrow = transform.Find("Arrow").gameObject;

        //Getting arrow line elements
        this.arrowLeftTopLine = arrow.transform.Find("LeftTopLine").gameObject;
        this.arrowRightTopLine = arrow.transform.Find("RightTopLine").gameObject;
        this.arrowLeftMiddleHorizontalLine = arrow.transform.Find("LeftMiddleHorizontalLine").gameObject;
        this.arrowRightMiddleHorizontalLine = arrow.transform.Find("RightMiddleHorizontalLine").gameObject;
        this.arrowLeftMiddleVerticalLine = arrow.transform.Find("LeftMiddleVerticalLine").gameObject;
        this.arrowRightMiddleVerticalLine = arrow.transform.Find("RightMiddleVerticalLine").gameObject;
        this.arrowBottomLine = arrow.transform.Find("BottomLine").gameObject;

        //Setting arrow size
        float arrowSize = _getArrowSize(size);

        //Setting arrow position
        arrow.transform.localPosition = new Vector3((size.x / 2) + (BorderWidth / 2), 0, (size.z / 2) + (BorderWidth / 2));

        //Setting arrow line positions
        float arrowTopLineLength = (Convert.ToSingle(Math.Sqrt(2)) * arrowSize / 2);
        float arrowHorizontalLineLength = (BorderWidth / 2) + (arrowSize / 3);
        float arrowVerticalLineLength = (arrowSize / 2);
        float arrowBottomLineLength = BorderWidth + (arrowSize / 3);

        arrowLeftTopLine.transform.localScale = new Vector3(arrowTopLineLength, BorderWidth, 1);
        arrowLeftTopLine.transform.localPosition = new Vector3((-arrowSize / 4) + (BorderWidth / (2 * (Convert.ToSingle(Math.Sqrt(2))))), 0, (arrowSize / 4) - (BorderWidth / (2 * (Convert.ToSingle(Math.Sqrt(2))))));
        arrowLeftTopLine.transform.localRotation = Quaternion.Euler(90, -45, 0);

        arrowRightTopLine.transform.localScale = new Vector3(arrowTopLineLength, BorderWidth, 1);
        arrowRightTopLine.transform.localPosition = new Vector3((arrowSize / 4) - (BorderWidth / (2 * (Convert.ToSingle(Math.Sqrt(2))))), 0, (arrowSize / 4) - (BorderWidth / (2 * (Convert.ToSingle(Math.Sqrt(2))))));
        arrowRightTopLine.transform.localRotation = Quaternion.Euler(90, 45, 0);

        arrowLeftMiddleHorizontalLine.transform.localScale = new Vector3(arrowHorizontalLineLength, BorderWidth, 1);
        arrowLeftMiddleHorizontalLine.transform.localPosition = new Vector3(-(arrowSize / 2) + (arrowHorizontalLineLength / 2), 0, -(BorderWidth / 2));

        arrowRightMiddleHorizontalLine.transform.localScale = new Vector3(arrowHorizontalLineLength, BorderWidth, 1);
        arrowRightMiddleHorizontalLine.transform.localPosition = new Vector3((arrowSize / 2) - (arrowHorizontalLineLength / 2), 0, -(BorderWidth / 2));

        arrowLeftMiddleVerticalLine.transform.localScale = new Vector3(BorderWidth, arrowVerticalLineLength, 1);
        arrowLeftMiddleVerticalLine.transform.localPosition = new Vector3((-arrowBottomLineLength / 2) + (BorderWidth / 2), 0, -(arrowVerticalLineLength / 2));

        arrowRightMiddleVerticalLine.transform.localScale = new Vector3(BorderWidth, arrowVerticalLineLength, 1);
        arrowRightMiddleVerticalLine.transform.localPosition = new Vector3((arrowBottomLineLength / 2) - (BorderWidth / 2), 0, -(arrowVerticalLineLength / 2));

        arrowBottomLine.transform.localScale = new Vector3(arrowBottomLineLength, BorderWidth, 1);
        arrowBottomLine.transform.localPosition = new Vector3(0, 0, -arrowVerticalLineLength + (BorderWidth / 2));

        //Setting arrow line material
        var arrowLeftTopLineMeshRenderer = arrowLeftTopLine.GetComponent<MeshRenderer>();
        var arrowRightTopLineMeshRenderer = arrowRightTopLine.GetComponent<MeshRenderer>();
        var arrowLeftMiddleHorizontalLineMeshRenderer = arrowLeftMiddleHorizontalLine.GetComponent<MeshRenderer>();
        var arrowRightMiddleHorizontalLineMeshRenderer = arrowRightMiddleHorizontalLine.GetComponent<MeshRenderer>();
        var arrowLeftMiddleVerticalLineMeshRenderer = arrowLeftMiddleVerticalLine.GetComponent<MeshRenderer>();
        var arrowRightMiddleVerticalLineMeshRenderer = arrowRightMiddleVerticalLine.GetComponent<MeshRenderer>();
        var arrowBottomLineMeshRenderer = arrowBottomLine.GetComponent<MeshRenderer>();

        arrowLeftTopLineMeshRenderer.material = BorderMaterial;
        arrowRightTopLineMeshRenderer.material = BorderMaterial;
        arrowLeftMiddleHorizontalLineMeshRenderer.material = BorderMaterial;
        arrowRightMiddleHorizontalLineMeshRenderer.material = BorderMaterial;
        arrowLeftMiddleVerticalLineMeshRenderer.material = BorderMaterial;
        arrowRightMiddleVerticalLineMeshRenderer.material = BorderMaterial;
        arrowBottomLineMeshRenderer.material = BorderMaterial;
    }

    private float _getArrowSize(Vector3 borderSize)
    {
        //Getting smallest dimension and mulitping it by arrow percentage size
        return (borderSize.x < borderSize.z ? borderSize.x : borderSize.z) * ArrowSizePercentage;
    }
}
