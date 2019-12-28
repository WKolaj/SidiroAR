using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetModel : MonoBehaviour
{
    private static string doorComponentName = "Door";

    /// <summary>
    /// Creator of obj model
    /// </summary>
    private AssetModelCreator _creator;
    public AssetModelCreator Creator
    {
        get
        {
            return _creator;
        }

    }

    private List<GameObject> _doorComponents = new List<GameObject>();
    /// <summary>
    /// List containing components for door elements
    /// </summary>
    public List<GameObject> DoorComponents
    {
        get
        {
            return _doorComponents;
        }
    }

    /// <summary>
    /// Name of OBJ model
    /// </summary>
    public string Name
    {
        get
        {
            return Creator.ModelName;
        }
    }

    /// <summary>
    /// Property to check whether door components are available withing OBJ Model
    /// </summary>
    public bool AreDoorComponentsAvailable
    {
        get
        {
            return DoorComponents.Count > 0;
        }
    }

    private Vector3 _size = default;
    /// <summary>
    /// Size of OBJ Model
    /// </summary>
    public Vector3 Size
    {
        get
        {
            return _size;
        }
    }

    private Vector3 _initialPosition = default;
    /// <summary>
    /// Initial position of OBJ model
    /// </summary>
    public Vector3 InitialPosition
    {
        get
        {
            return _initialPosition;
        }
    }

    /// <summary>
    /// Is Model Shown
    /// </summary>
    public bool Shown
    {
        get
        {
            return gameObject.activeSelf;
        }
    }


    /// <summary>
    /// Method for generating size and position of model
    /// </summary>
    /// <param name="model">
    /// Model to measure
    /// </param>
    /// <returns>
    /// Array of vectors
    /// [0] - size
    /// [1] - position
    /// </returns>
    private static Vector3[] GetSizeAndPositionOfCombinedMesh(GameObject model)
    {
        GameObject objectToReturn = new GameObject("object with mesh");

        objectToReturn.AddComponent<MeshFilter>();
        objectToReturn.AddComponent<MeshRenderer>();
        MeshFilter[] meshFilters = model.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            i++;
        }

        objectToReturn.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        objectToReturn.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        Mesh mesh = objectToReturn.transform.GetComponent<MeshFilter>().mesh;

        var sizeVector = new Vector3(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z);
        var positionVector = new Vector3(mesh.bounds.min.x, mesh.bounds.min.y, mesh.bounds.min.z);

        if (objectToReturn != null)
            Destroy(objectToReturn);

        return new Vector3[] { sizeVector, positionVector };
    }

    private void InitDoorComponents()
    {
        DoorComponents.Clear();

        foreach(var child in gameObject.transform)
        {
            if(typeof(Transform).IsAssignableFrom(child.GetType()))
            {
                var component = (Transform)child;
                if (component.name.Contains(doorComponentName)) DoorComponents.Add(component.gameObject);
            }
        }


    }

    public void Init(AssetModelCreator creator)
    {
        this._creator = creator;

        //Get and assign model size and position
        var sizeAndPosition = GetSizeAndPositionOfCombinedMesh(gameObject);
        _size = sizeAndPosition[0];
        _initialPosition = sizeAndPosition[1];

        //Center the model according to container
        gameObject.transform.Translate(-InitialPosition);
        gameObject.transform.Translate(new Vector3(-0.5f * Size.x, 0, -0.5f * Size.z));

        //Initializing door components
        InitDoorComponents();

    }

    /// <summary>
    /// Method for hiddining visiblity of all door components
    /// </summary>
    public void HideDoorComponents()
    {
        foreach(var component in DoorComponents)
        {
            component.SetActive(false);
        }
    }

    /// <summary>
    /// Method for showing visiblity of all door components
    /// </summary>
    public void ShowDoorComponents()
    {
        foreach (var component in DoorComponents)
        {
            component.SetActive(true);
        }
    }

    /// <summary>
    /// Hide Model
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Show Model
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

}
